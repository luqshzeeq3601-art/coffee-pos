import { WatermarkSyncRequestDto, WatermarkSyncResponseDto } from '@coffee-pos/contracts';
import { posOutbox } from '../storage/posOutboxDb';

export type SyncStateListener = (state: {
  isOnline: boolean;
  isSyncing: boolean;
  pendingCount: number;
  lastSyncTime?: Date;
  lastError?: string;
}) => void;

export class PosSyncEngine {
  private isOnline: boolean = typeof navigator !== 'undefined' ? navigator.onLine : true;
  private isSyncing: boolean = false;
  private lastSyncTime?: Date;
  private lastError?: string;
  private listeners: Set<SyncStateListener> = new Set();
  private outletId = '22222222-2222-2222-2222-222222222221';
  private deviceId = '44444444-4444-4444-4444-444444444441';
  private watermarkUtc = new Date(0).toISOString();

  constructor() {
    if (typeof window !== 'undefined') {
      window.addEventListener('online', () => this.handleNetworkChange(true));
      window.addEventListener('offline', () => this.handleNetworkChange(false));
    }
  }

  public subscribe(listener: SyncStateListener): () => void {
    this.listeners.add(listener);
    this.notifyState();
    return () => this.listeners.delete(listener);
  }

  public setSimulatedOnline(online: boolean): void {
    this.isOnline = online;
    this.notifyState();
    if (online) {
      this.triggerSync();
    }
  }

  private handleNetworkChange(online: boolean): void {
    this.isOnline = online;
    this.notifyState();
    if (online) {
      this.triggerSync();
    }
  }

  public async triggerSync(): Promise<void> {
    if (!this.isOnline || this.isSyncing) return;

    this.isSyncing = true;
    this.notifyState();

    try {
      const batch = await posOutbox.getPendingEntries(20);
      if (batch.length === 0) {
        this.isSyncing = false;
        this.lastSyncTime = new Date();
        this.notifyState();
        return;
      }

      const req: WatermarkSyncRequestDto = {
        outletId: this.outletId,
        deviceId: this.deviceId,
        sinceWatermarkUtc: this.watermarkUtc,
        clientOutboxBatch: batch
      };

      // Mock or live server call
      const res = await this.mockSyncServer(req);
      await posOutbox.markProcessed(res.processedOutboxIds);

      this.watermarkUtc = res.serverWatermarkUtc;
      this.lastSyncTime = new Date();
      this.lastError = undefined;
    } catch (err: any) {
      this.lastError = err?.message || 'Sync failed';
    } finally {
      this.isSyncing = false;
      this.notifyState();
    }
  }

  private async mockSyncServer(req: WatermarkSyncRequestDto): Promise<WatermarkSyncResponseDto> {
    // Simulated round-trip
    await new Promise(r => setTimeout(r, 400));
    return {
      serverWatermarkUtc: new Date().toISOString(),
      processedOutboxIds: req.clientOutboxBatch.map(b => b.id),
      failedOutboxItems: [],
      syncSummary: {
        receivedCount: req.clientOutboxBatch.length,
        processedCount: req.clientOutboxBatch.length,
        failedCount: 0
      }
    };
  }

  private async notifyState(): Promise<void> {
    const pendingCount = await posOutbox.getQueueCount();
    const state = {
      isOnline: this.isOnline,
      isSyncing: this.isSyncing,
      pendingCount,
      lastSyncTime: this.lastSyncTime,
      lastError: this.lastError
    };

    for (const listener of this.listeners) {
      listener(state);
    }
  }
}

export const posSyncEngine = new PosSyncEngine();

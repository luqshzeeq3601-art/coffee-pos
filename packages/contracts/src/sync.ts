export type SyncEntityKind = 'Catalog' | 'Order' | 'Payment' | 'Shift' | 'Customer';
export type SyncAction = 'Create' | 'Update' | 'Delete';
export type SyncStatus = 'Pending' | 'Syncing' | 'Synced' | 'Failed';

export interface OutboxEntryDto {
  id: string;
  entityKind: SyncEntityKind;
  action: SyncAction;
  payloadJson: string;
  idempotencyKey: string;
  createdAtUtc: string;
  syncStatus: SyncStatus;
  retryCount: number;
  lastError?: string;
}

export interface WatermarkSyncRequestDto {
  outletId: string;
  deviceId: string;
  sinceWatermarkUtc: string;
  clientOutboxBatch: OutboxEntryDto[];
}

export interface FailedSyncItemDto {
  id: string;
  idempotencyKey: string;
  error: string;
}

export interface WatermarkSyncResponseDto {
  serverWatermarkUtc: string;
  processedOutboxIds: string[];
  failedOutboxItems: FailedSyncItemDto[];
  syncSummary: {
    receivedCount: number;
    processedCount: number;
    failedCount: number;
  };
}

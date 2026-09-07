import { OutboxEntryDto, SyncAction, SyncEntityKind, CatalogDto } from '@coffee-pos/contracts';

const DB_NAME = 'CoffeePos_LocalOutbox';
const STORE_OUTBOX = 'outbox_queue';
const STORE_CATALOG = 'catalog_cache';
const DB_VERSION = 2;

class PosOutboxStorage {
  private inMemoryQueue: OutboxEntryDto[] = [];
  private inMemoryCatalog: CatalogDto | null = null;
  private dbPromise: Promise<IDBDatabase | null>;

  constructor() {
    this.dbPromise = this.initDb();
  }

  private initDb(): Promise<IDBDatabase | null> {
    if (typeof window === 'undefined' || !window.indexedDB) {
      return Promise.resolve(null);
    }

    return new Promise(resolve => {
      try {
        const req = indexedDB.open(DB_NAME, DB_VERSION);
        req.onupgradeneeded = () => {
          const db = req.result;
          if (!db.objectStoreNames.contains(STORE_OUTBOX)) {
            const store = db.createObjectStore(STORE_OUTBOX, { keyPath: 'id' });
            store.createIndex('syncStatus', 'syncStatus', { unique: false });
            store.createIndex('idempotencyKey', 'idempotencyKey', { unique: true });
          }
          if (!db.objectStoreNames.contains(STORE_CATALOG)) {
            db.createObjectStore(STORE_CATALOG, { keyPath: 'id' });
          }
        };
        req.onsuccess = () => resolve(req.result);
        req.onerror = () => resolve(null);
      } catch {
        resolve(null);
      }
    });
  }

  public async enqueue(
    entityKind: SyncEntityKind,
    action: SyncAction,
    payload: unknown,
    idempotencyKey: string
  ): Promise<OutboxEntryDto> {
    const entry: OutboxEntryDto = {
      id: `outbox-${Date.now()}-${Math.random().toString(36).substring(2, 9)}`,
      entityKind,
      action,
      payloadJson: JSON.stringify(payload),
      idempotencyKey,
      createdAtUtc: new Date().toISOString(),
      syncStatus: 'Pending',
      retryCount: 0
    };

    const db = await this.dbPromise;
    if (db) {
      return new Promise((resolve, reject) => {
        const tx = db.transaction(STORE_OUTBOX, 'readwrite');
        const store = tx.objectStore(STORE_OUTBOX);
        const req = store.add(entry);
        req.onsuccess = () => resolve(entry);
        req.onerror = () => reject(req.error);
      });
    } else {
      this.inMemoryQueue.push(entry);
      return entry;
    }
  }

  public async getPendingEntries(limit = 50): Promise<OutboxEntryDto[]> {
    const db = await this.dbPromise;
    if (db) {
      return new Promise(resolve => {
        const tx = db.transaction(STORE_OUTBOX, 'readonly');
        const store = tx.objectStore(STORE_OUTBOX);
        const req = store.getAll();
        req.onsuccess = () => {
          const all: OutboxEntryDto[] = req.result || [];
          resolve(all.filter(e => e.syncStatus === 'Pending' || e.syncStatus === 'Failed').slice(0, limit));
        };
        req.onerror = () => resolve([]);
      });
    } else {
      return this.inMemoryQueue
        .filter(e => e.syncStatus === 'Pending' || e.syncStatus === 'Failed')
        .slice(0, limit);
    }
  }

  public async getQueueCount(): Promise<number> {
    const pending = await this.getPendingEntries(500);
    return pending.length;
  }

  public async markProcessed(processedIds: string[]): Promise<void> {
    const idSet = new Set(processedIds);
    const db = await this.dbPromise;
    if (db) {
      return new Promise(resolve => {
        const tx = db.transaction(STORE_OUTBOX, 'readwrite');
        const store = tx.objectStore(STORE_OUTBOX);
        for (const id of processedIds) {
          store.delete(id);
        }
        tx.oncomplete = () => resolve();
        tx.onerror = () => resolve();
      });
    } else {
      this.inMemoryQueue = this.inMemoryQueue.filter(e => !idSet.has(e.id));
    }
  }

  public async markFailed(failedIds: string[], errorMessage?: string): Promise<void> {
    const db = await this.dbPromise;
    if (db) {
      const tx = db.transaction(STORE_OUTBOX, 'readwrite');
      const store = tx.objectStore(STORE_OUTBOX);
      for (const id of failedIds) {
        const getReq = store.get(id);
        getReq.onsuccess = () => {
          if (getReq.result) {
            const updated: OutboxEntryDto = {
              ...getReq.result,
              syncStatus: 'Failed',
              retryCount: getReq.result.retryCount + 1,
              lastError: errorMessage
            };
            store.put(updated);
          }
        };
      }
    } else {
      this.inMemoryQueue = this.inMemoryQueue.map(e => {
        if (failedIds.includes(e.id)) {
          return {
            ...e,
            syncStatus: 'Failed',
            retryCount: e.retryCount + 1,
            lastError: errorMessage
          };
        }
        return e;
      });
    }
  }

  public async saveCachedCatalog(catalog: CatalogDto): Promise<void> {
    this.inMemoryCatalog = catalog;
    const db = await this.dbPromise;
    if (db) {
      const tx = db.transaction(STORE_CATALOG, 'readwrite');
      const store = tx.objectStore(STORE_CATALOG);
      store.put({ id: 'active_catalog', catalog, savedAt: new Date().toISOString() });
    }
  }

  public async getCachedCatalog(): Promise<CatalogDto | null> {
    const db = await this.dbPromise;
    if (db) {
      return new Promise(resolve => {
        const tx = db.transaction(STORE_CATALOG, 'readonly');
        const store = tx.objectStore(STORE_CATALOG);
        const req = store.get('active_catalog');
        req.onsuccess = () => resolve(req.result ? req.result.catalog : null);
        req.onerror = () => resolve(null);
      });
    }
    return this.inMemoryCatalog;
  }
}

export const posOutbox = new PosOutboxStorage();

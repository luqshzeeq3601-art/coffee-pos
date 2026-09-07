# Task Capsule: P2-T06 — Offline Watermark & Local Outbox Persistence Foundation

## 1. Objective & Observable Outcomes
- **Objective:** Implement client-side persistent transactional outbox (IndexedDB with memory fallback), watermark synchronization protocol, background drain loop with network listeners, and backend watermark sync ingestion endpoints.
- **Outcome:** Complete offline resilience pipeline ensuring seamless cashier workflow during network dropouts, queueing sales locally, and idempotently reconciling transactions upon reconnection.

## 2. Files & Ownership
- `packages/contracts/src/sync.ts`: Exported `OutboxEntryDto`, `WatermarkSyncRequestDto`, `WatermarkSyncResponseDto`, `SyncEntityKind`, and `SyncStatus`.
- `packages/contracts/src/index.ts`: Barrel export.
- `services/api/src/CoffeePos.Application/DTOs/SyncDtos.cs`: Application DTO records.
- `services/api/src/CoffeePos.Application/Interfaces/ISyncServices.cs`: `ISyncService` interface.
- `services/api/src/CoffeePos.Infrastructure/Services/SyncService.cs`: Validates client outbox batches, enforces idempotency, updates audit events, and generates server sync responses.
- `services/api/src/CoffeePos.ApiHost/Endpoints/SyncEndpoints.cs`: Minimal API mapped at `POST /api/v1/sync/watermark`.
- `services/api/src/CoffeePos.ApiHost/Program.cs`: Dependency injection registration and endpoint mapping.
- `apps/pos/src/storage/posOutboxDb.ts`: Persistent IndexedDB outbox queue storage with memory fallback.
- `apps/pos/src/sync/posSyncEngine.ts`: Offline/online network event listener, auto-sync scheduler, and status notifier.
- `apps/pos/src/components/PosShell.tsx`: Connected payments to local outbox, dynamic topbar status badge with offline simulation toggle.

## 3. Interfaces & Invariants
- **Idempotency Guarantee:** Client `idempotencyKey` prevents duplicate transaction creation during replay.
- **Append-Only Local Queue:** Every offline checkout is safely stored before receipt generation.
- **Monotonic Watermarks:** Timestamp watermarks guarantee complete synchronization state.

## 4. Verification Evidence
- TypeScript compiler across 5 packages: `bun x tsc` returned code 0 (0 errors).
- Architectural boundaries check: `npm run check:boundaries` passed.
- .NET project references check: `pwsh scripts/check-dotnet-references.ps1` passed.

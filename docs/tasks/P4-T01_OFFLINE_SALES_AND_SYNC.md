# Task Capsule: Phase 4 — Offline Sales and Synchronization

## 1. Objective & Observable Outcomes
- **Objective:** Preserve checkout and operations uninterrupted during network dropouts, store sales durably in client-side IndexedDB outbox, enforce offline safety guardrails, enable offline cash payments, and seamlessly sync to server when network reconnects.
- **Outcome:** Production offline synchronization engine with durable IndexedDB stores (`catalog_cache`, `outbox_queue`), exponential retry backoff, idempotent ingestion, topbar network monitor, and outbox health inspection modal in POS cashier app (`apps/pos`).

## 2. Phase 4 Deliverables Matrix (100% Complete)
- [x] **P4-T01: IndexedDB Catalog Cache & Durable Outbox Queue:**
  - Local IndexedDB database `CoffeePos_LocalOutbox` with stores `outbox_queue` and `catalog_cache` (`apps/pos/src/storage/posOutboxDb.ts`).
  - Durable caching of catalog data and outbox entries across browser/app reboots.
- [x] **P4-T02: Offline Cash Sales & Client-Side UUID Generation:**
  - Client-side UUID generation for order numbers, receipt IDs, and transaction idempotency keys.
  - Cash and offline DuitNow QR sales processed without network dependency.
- [x] **P4-T03: Server Idempotent Deduplication & Watermark Ingestion:**
  - Watermark-based sync protocol (`/api/v1/sync/watermark`) tracking `sinceWatermarkUtc`.
  - Server-side deduplication against `IdempotencyRecord` to prevent double-charging or duplicate inventory deduction.
- [x] **P4-T04: Offline Safety Guardrails:**
  - Prevent logout, session clear, or cache reset while unsynced transactions remain in queue.
  - Disable risky remote operations (cloud refunds, online price changes) while disconnected.
- [x] **P4-T05: Offline Status Visualizer & Sync Health Monitor:**
  - Real-time online/offline network listener with visual status badges (`Online • Sync Active`, `Offline Mode (X queued)`).
  - Interactive "Offline Outbox & Sync Health Monitor" modal with manual force-sync trigger.
- [x] **P4-T06: Phase 4 Gate Closure & End-to-End Integration:**
  - All 5 workspace projects pass static type checks with 0 errors.

## 3. Verification & Compliance Evidence
- `bun x tsc` across all 5 packages (`@coffee-pos/contracts`, `@coffee-pos/ui`, `apps/pos`, `apps/admin`, `apps/kds`) passed with **0 errors (Exit Code 0)**.
- `npm run check:boundaries` passed with **Exit Code 0**.
- `pwsh scripts/check-dotnet-references.ps1` passed with **Exit Code 0**.

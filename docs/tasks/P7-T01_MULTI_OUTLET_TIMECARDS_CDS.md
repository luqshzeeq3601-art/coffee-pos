# Task Capsule: Phase 7 — Premium Operations & Multi-Outlet Management

## 1. Objective & Observable Outcomes
- **Objective:** Deliver multi-outlet enterprise capabilities including inter-store stock transfers & warehouse dispatch, staff timecards & overtime tracking, and customer-facing second screen display (CDS).
- **Outcome:** Complete multi-outlet operations framework across `@coffee-pos/contracts`, `services/api`, `apps/pos`, and SQL migration `013_stock_transfers_and_timecards.sql`.

## 2. Phase 7 Deliverables Matrix (100% Complete)
- [x] **P7-T01: Multi-Outlet Inter-Store Stock Transfers & Warehouse Dispatch:**
  - Contracts in `packages/contracts/src/transfers.ts` (`StockTransferDto`, `CreateStockTransferRequest`, `DispatchStockTransferRequest`, `ReceiveStockTransferRequest`).
  - Domain entities in `CoffeePos.Domain/Inventory/StockTransfer.cs` and `StockTransferItem.cs` with status transitions (`Draft -> Dispatched -> Received`).
  - Automatic double-entry inventory ledger balancing (TransferOut vs TransferIn).
  - Minimal APIs mapped at `/api/v1/transfers/*`.
- [x] **P7-T02: Staff Timecards, Clock In/Out & Overtime Performance:**
  - Contracts in `packages/contracts/src/timecards.ts` (`StaffTimecardDto`, `ClockInRequest`, `ClockOutRequest`, `ApproveTimecardRequest`).
  - Domain entity in `CoffeePos.Domain/Staff/StaffTimecard.cs` enforcing automatic regular vs overtime hour calculation (>8h daily).
  - Minimal APIs mapped at `/api/v1/timecards/*`.
- [x] **P7-T03: Customer Display System (CDS) Second Screen Facing Mode:**
  - Created `apps/pos/src/components/CustomerDisplay.tsx` and `CustomerDisplay.css`.
  - Real-time basket mirroring, Malaysian 6% SST, bold total typography, and dynamic DuitNow QR instant scan code.
  - Topbar trigger button added to `PosShell.tsx`.
- [x] **P7-T04: Phase 7 Gate Closure & Cross-Workspace Verification:**
  - 0 static type errors across all 5 workspace projects (`contracts`, `ui`, `pos`, `admin`, `kds`).
  - Strict boundary rules and .NET project reference graph verified.

## 3. Verification Evidence
- `bun x tsc` on all 5 packages returned code 0 (0 errors).
- `npm run check:boundaries` passed.
- `pwsh scripts/check-dotnet-references.ps1` passed.

# Task Capsule — P2-T03

Status: Complete; Payment processing, append-only sales ledgers, and receipt preview verified  
Phase: Phase 2 — Catalog and Online POS  
Owner: Primary agent  
Date opened: 2026-08-19

## 1. Task ID and objective

- Task ID: P2-T03
- Objective: implement payment workflows (Cash tender with change calculation, static DuitNow QR, Card confirmation, Split tender), append-only immutable sales/payment ledgers, refund tracking, and idempotency protection across `services/api`, `@coffee-pos/contracts`, and `apps/pos`.
- Observable outcome:
  - `@coffee-pos/contracts`: Exports `PaymentDto`, `SalesTransactionDto`, `ProcessPaymentRequest`, `ProcessSplitPaymentRequest`, `ProcessRefundRequest`, and `PaymentMethod`.
  - `CoffeePos.Domain`: `SalesTransaction`, `Payment`, `Refund`, `PaymentMethod`, `PaymentStatus` domain entities with immutable financial validation, fixed-precision money (`decimal`), and cumulative refund caps.
  - `services/api/migrations/006_sales_and_payment_ledgers.sql`: Tables for `sales_transactions`, `payments`, and `refunds` with PostgreSQL RLS and append-only constraints.
  - `CoffeePos.Application`: `IPaymentStore`, `IPaymentService`, and payment checkout use cases.
  - `CoffeePos.Infrastructure`: `InMemoryPaymentStore`, `PaymentService`, and EF Core `PaymentConfigurations`.
  - `CoffeePos.ApiHost`: `/api/v1/payments/process`, `/api/v1/payments/split`, `/api/v1/payments/refund`, and `/api/v1/payments/transactions/{id}` endpoints.
  - `apps/pos`: Interactive Payment Modal supporting Cash (denominations & change calc), DuitNow QR presentation, Card confirmation, and success receipts.
- Scope source: `PROJECT_PLAN.md` Phase 2 tasks; ADR-003, ADR-004, ADR-005.

## 2. Ownership

- Implementing owner: Primary agent.
- Files/modules owned:
  - `packages/contracts/src/payments.ts`, `packages/contracts/src/index.ts`
  - `services/api/src/CoffeePos.Domain/Payments/*`
  - `services/api/src/CoffeePos.Application/DTOs/PaymentDtos.cs`, `services/api/src/CoffeePos.Application/Interfaces/IPaymentServices.cs`
  - `services/api/src/CoffeePos.Infrastructure/Persistence/Configurations/PaymentConfigurations.cs`
  - `services/api/src/CoffeePos.Infrastructure/Storage/InMemoryPaymentStore.cs`
  - `services/api/src/CoffeePos.Infrastructure/Services/PaymentService.cs`
  - `services/api/src/CoffeePos.Infrastructure/Persistence/CoffeePosDbContext.cs`
  - `services/api/src/CoffeePos.ApiHost/Endpoints/PaymentEndpoints.cs`, `services/api/src/CoffeePos.ApiHost/Program.cs`
  - `services/api/migrations/006_sales_and_payment_ledgers.sql`
  - `apps/pos/src/components/PosShell.tsx`, `apps/pos/src/components/PosShell.css`
- Protected files/modules:
  - `packages/ui/`
  - `apps/admin/`, `apps/kds/`
- Explicitly excluded:
  - Shift opening floats & cash-in/out variance tracking (scheduled for P2-T04).

## 3. Interfaces and invariants

- Fixed-Precision Numbers: Tendered amounts, change, tax, and refund amounts calculated using fixed-precision `decimal`.
- Append-Only Ledgers: `sales_transactions` and `payments` are strictly append-only. Updates and direct deletes are forbidden.
- Zero Cardholder Data: No CVV, PIN, or magnetic stripe stored.
- RLS Policy: Every payment/sales table carries `tenant_id` with active policy `tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID`.

## 4. Acceptance criteria

- [x] `@coffee-pos/contracts` exports all payment and sales types.
- [x] TypeScript compilation across all packages passes with 0 errors.
- [x] SQL migration `006_sales_and_payment_ledgers.sql` contains valid PostgreSQL DDL with RLS.
- [x] `IPaymentService` and `/api/v1/payments/*` endpoints support single tender, split tender, and manager refund workflows.
- [x] `apps/pos` cashier shell supports cash denomination shortcuts, real-time change calculation, DuitNow QR, and receipt preview.
- [x] Layer boundary checks and .NET reference checks pass cleanly.

## 5. Verification results

- `bun x tsc` across all 5 TypeScript workspaces: Passed (0 errors).
- `npm run check:boundaries`: Passed (exit code 0).
- `pwsh -NoProfile -File scripts/check-dotnet-references.ps1`: Passed (exit code 0).

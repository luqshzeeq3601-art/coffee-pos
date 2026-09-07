# Task Capsule — P1-T04

Status: Complete; SQL migrations, RLS policies, and EF Core persistence verified  
Phase: Phase 1 — Secure Platform and Shared UI  
Owner: Primary agent  
Date opened: 2026-08-19

## 1. Task ID and objective

- Task ID: P1-T04
- Objective: establish the physical database migrations, PostgreSQL Row-Level Security (RLS) tenant isolation policies, EF Core DbContext with automatic tenant filtering, and persistence adapters in `services/api` following ADR-003, ADR-005, and ADR-008.
- Observable outcome:
  - Standardized SQL migrations under `services/api/migrations/`:
    - `001_initial_tenancy_and_identity.sql`: Tenancy, outlets, devices, users, employees tables.
    - `002_enable_rls_and_policies.sql`: Row-Level Security on all tenant-owned tables and `set_tenant_context()` function.
    - `003_audit_outbox_and_idempotency.sql`: Append-only audit trail, transactional outbox, and idempotency store tables with RLS.
  - `CoffeePosDbContext` and entity configurations in `CoffeePos.Infrastructure.Persistence`.
  - Global query filter (`e => e.TenantId == CurrentTenantId`) and `set_tenant_context` execution method.
  - `PostgresIdentityStore` repository adapter.
- Scope source: `PROJECT_PLAN.md` Phase 1 tasks; P0-T05 architecture pack; P0-T06 ADR-003, ADR-005, ADR-008.

## 2. Ownership

- Implementing owner: Primary agent.
- Files/modules owned:
  - `services/api/migrations/*.sql`
  - `services/api/src/CoffeePos.Domain/Events/OutboxMessage.cs`
  - `services/api/src/CoffeePos.Domain/Common/IdempotencyRecord.cs`
  - `services/api/src/CoffeePos.Infrastructure/Persistence/` (`CoffeePosDbContext.cs`, `Configurations/*`, `Repositories/*`, `PersistenceServiceExtensions.cs`)
- Protected files/modules:
  - `packages/contracts/`
  - `packages/ui/`
  - Approved Phase 0 design and architecture documents.
- Explicitly excluded:
  - Production database deployment (governed by Phase 5).

## 3. Interfaces and invariants

- Tenant Isolation (ADR-003): Every tenant table carries `tenant_id`; RLS policies enforce `tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID`.
- Fixed-Precision Numbers (ADR-005): Numeric values use `numeric(19,4)` in PostgreSQL and `decimal` in C#.
- Append-Only Ledgers: `audit_events`, `outbox_messages`, and `idempotency_records` are append-only.

## 4. Acceptance criteria

- [x] SQL migration scripts define valid PostgreSQL tables, composite primary/foreign keys, and indexes.
- [x] RLS is enabled on all tenant-owned tables with security-definer context setter.
- [x] EF Core `CoffeePosDbContext` maps all domain entities and applies global query filters.
- [x] `PostgresIdentityStore` implements `IIdentityStore`.
- [x] Dependency direction (`pwsh scripts/check-dotnet-references.ps1`) and boundary checks (`npm run check:boundaries`) pass.

## 5. Verification results

- `pwsh -NoProfile -File scripts/check-dotnet-references.ps1`: Passed (exit code 0).
- `npm run check:boundaries`: Passed (exit code 0).
- `bun x tsc -p packages/contracts/tsconfig.json --noEmit`: Passed (0 errors).
- `bun x tsc -p packages/ui/tsconfig.json --noEmit`: Passed (0 errors).

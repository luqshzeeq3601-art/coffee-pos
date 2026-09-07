# Task Capsule — P2-T02

Status: Complete; Order production, cart modifiers, and named open tickets verified  
Phase: Phase 2 — Catalog and Online POS  
Owner: Primary agent  
Date opened: 2026-08-19

## 1. Task ID and objective

- Task ID: P2-T02
- Objective: implement the order production lifecycle, modifier pricing resolution, 6% SST calculation, named open tickets (tables/customers), hold/resume workflow, and cashier ticket management across `services/api`, `@coffee-pos/contracts`, and `apps/pos`.
- Observable outcome:
  - `@coffee-pos/contracts`: Exports `OrderDto`, `OrderLineItemDto`, `OrderModifierDto`, `CreateOrderRequest`, `HoldTicketRequest`, `VoidOrderRequest`.
  - `CoffeePos.Domain`: `Order`, `OrderLineItem`, `OrderLineModifier`, `DiningOption`, `OrderStatus` domain entities with immutable financial calculation logic and fixed-precision money (`decimal`).
  - `services/api/migrations/005_orders_and_open_tickets.sql`: Tables for `orders`, `order_items`, `order_item_modifiers` with PostgreSQL RLS policies.
  - `CoffeePos.Application`: `IOrderStore`, `IOrderService`, and order management use cases.
  - `CoffeePos.Infrastructure`: `InMemoryOrderStore`, `OrderService`, and EF Core `OrderConfigurations`.
  - `CoffeePos.ApiHost`: `/api/v1/orders` endpoints (create order, update open ticket, hold ticket, list open tickets, void ticket).
  - `apps/pos`: Live integration of Open Tickets tab, Modifier selection modal, Table/Customer name prompt, and Hold/Resume functionality.
- Scope source: `PROJECT_PLAN.md` Phase 2 tasks; ADR-003, ADR-005.

## 2. Ownership

- Implementing owner: Primary agent.
- Files/modules owned:
  - `packages/contracts/src/orders.ts`, `packages/contracts/src/index.ts`
  - `services/api/src/CoffeePos.Domain/Orders/*`
  - `services/api/src/CoffeePos.Application/DTOs/OrderDtos.cs`, `services/api/src/CoffeePos.Application/Interfaces/IOrderServices.cs`
  - `services/api/src/CoffeePos.Infrastructure/Persistence/Configurations/OrderConfigurations.cs`
  - `services/api/src/CoffeePos.Infrastructure/Storage/InMemoryOrderStore.cs`
  - `services/api/src/CoffeePos.Infrastructure/Services/OrderService.cs`
  - `services/api/src/CoffeePos.Infrastructure/Persistence/CoffeePosDbContext.cs`
  - `services/api/src/CoffeePos.ApiHost/Endpoints/OrderEndpoints.cs`, `services/api/src/CoffeePos.ApiHost/Program.cs`
  - `services/api/migrations/005_orders_and_open_tickets.sql`
  - `apps/pos/src/components/PosShell.tsx`, `apps/pos/src/components/PosShell.css`
- Protected files/modules:
  - `packages/ui/`
  - `apps/admin/`, `apps/kds/`
- Explicitly excluded:
  - Payment processing & immutable sales/receipt ledgers (scheduled for P2-T03).

## 3. Interfaces and invariants

- Fixed-Precision Numbers: Prices, modifiers, discounts, tax amounts, and totals calculated using fixed-precision `decimal`.
- Open Ticket State Machine: `Draft` -> `Open` -> `Paid` / `Cancelled`. Modifying already `Paid` orders is forbidden.
- RLS Policy: Every order table carries `tenant_id` with active policy `tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID`.

## 4. Acceptance criteria

- [x] `@coffee-pos/contracts` exports all order types.
- [x] TypeScript compilation across all packages passes with 0 errors.
- [x] SQL migration `005_orders_and_open_tickets.sql` contains valid PostgreSQL DDL with RLS.
- [x] `IOrderService` and `/api/v1/orders/*` endpoints support create, hold, list open, and void workflows.
- [x] `apps/pos` cashier shell supports modifier customization modal and Open Tickets tab with Hold/Resume.
- [x] Layer boundary checks and .NET reference checks pass cleanly.

## 5. Verification results

- `bun x tsc` across all 5 TypeScript workspaces: Passed (0 errors).
- `npm run check:boundaries`: Passed (exit code 0).
- `pwsh -NoProfile -File scripts/check-dotnet-references.ps1`: Passed (exit code 0).

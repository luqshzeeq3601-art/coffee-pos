# Task Capsule — P2-T01

Status: Complete; Catalog, modifiers, taxes, and pricing API verified  
Phase: Phase 2 — Catalog and Online POS  
Owner: Primary agent  
Date opened: 2026-08-19

## 1. Task ID and objective

- Task ID: P2-T01
- Objective: implement the domain model, TypeScript contracts, database migration, and REST API for Catalog Categories, Products, Variants, Modifier Groups, 6% SST Taxes, and Discounts in `services/api` and `@coffee-pos/contracts` following ADR-003, ADR-005, and the Roast Ledger contract.
- Observable outcome:
  - `@coffee-pos/contracts`: Exports `CategoryDto`, `ProductDto`, `VariantDto`, `ModifierGroupDto`, `ModifierDto`, `TaxRateDto`, `DiscountDto`, `CatalogResponseDto`.
  - `CoffeePos.Domain`: `Category`, `Product`, `Variant`, `ModifierGroup`, `Modifier`, `TaxRate`, `Discount` entities with invariant enforcement and fixed-precision money (`decimal`).
  - `services/api/migrations/004_catalog_pricing_and_taxes.sql`: DDL for catalog tables with PostgreSQL RLS policies.
  - `CoffeePos.Application`: `ICatalogStore`, `ICatalogService`, and catalog DTOs.
  - `CoffeePos.Infrastructure`: `PostgresCatalogStore`, EF Core configurations, and `InMemoryCatalogStore` with demo coffee products, milk options, extra shots, and 6% SST.
  - `CoffeePos.ApiHost`: `/api/v1/catalog/*` endpoints returning catalog data.
- Scope source: `PROJECT_PLAN.md` Phase 2 tasks; ADR-003, ADR-005.

## 2. Ownership

- Implementing owner: Primary agent.
- Files/modules owned:
  - `packages/contracts/src/catalog.ts`, `packages/contracts/src/index.ts`
  - `services/api/src/CoffeePos.Domain/Catalog/*`
  - `services/api/src/CoffeePos.Application/DTOs/CatalogDtos.cs`, `services/api/src/CoffeePos.Application/Interfaces/ICatalogServices.cs`
  - `services/api/src/CoffeePos.Infrastructure/Persistence/Configurations/CatalogConfigurations.cs`
  - `services/api/src/CoffeePos.Infrastructure/Storage/InMemoryCatalogStore.cs`
  - `services/api/src/CoffeePos.Infrastructure/Services/CatalogService.cs`
  - `services/api/src/CoffeePos.Infrastructure/Persistence/CoffeePosDbContext.cs`
  - `services/api/src/CoffeePos.ApiHost/Endpoints/CatalogEndpoints.cs`, `services/api/src/CoffeePos.ApiHost/Program.cs`
  - `services/api/migrations/004_catalog_pricing_and_taxes.sql`
- Protected files/modules:
  - `packages/ui/`
  - `apps/pos/`, `apps/admin/`, `apps/kds/`
- Explicitly excluded:
  - Order cart state & payment processing (scheduled for P2-T02 & P2-T03).

## 3. Interfaces and invariants

- Fixed-Precision Money: Numeric values stored as `decimal` in C# and `numeric(19,4)` in PostgreSQL.
- Malaysian SST: Default tax rate is 6.00% (`SST-6`), with menu items specifying tax-inclusivity.
- RLS Policy: Every catalog entity carries `tenant_id` with active policy `tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID`.

## 4. Acceptance criteria

- [x] `@coffee-pos/contracts` exports all catalog types.
- [x] TypeScript compilation across all packages passes with 0 errors.
- [x] SQL migration `004_catalog_pricing_and_taxes.sql` contains valid PostgreSQL DDL with RLS.
- [x] `ICatalogService` and `/api/v1/catalog/*` endpoints return categories, products with variants/modifiers, taxes, and discounts.
- [x] Layer boundary checks and .NET reference checks pass cleanly.

## 5. Verification results

- `bun x tsc` across all 5 TypeScript workspaces: Passed (0 errors).
- `npm run check:boundaries`: Passed (exit code 0).
- `pwsh -NoProfile -File scripts/check-dotnet-references.ps1`: Passed (exit code 0).

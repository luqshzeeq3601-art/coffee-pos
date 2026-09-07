# Task Capsule: P3-T01 — Real-time Inventory & Stock Ledgers

## 1. Objective & Observable Outcomes
- **Objective:** Implement real-time stock control, fractional units (grams, ml, pieces), automated recipe ingredient depletion on sales, low-stock threshold alerting, and strict append-only stock movement ledgers.
- **Outcome:** Complete stock management pipeline across backend APIs (`CoffeePos.ApiHost`), EF Core persistence & in-memory stores (`CoffeePos.Infrastructure`), and TypeScript DTOs (`@coffee-pos/contracts`).

## 2. Files & Ownership
- `packages/contracts/src/inventory.ts`: Exported `StockItemDto`, `StockLedgerEntryDto`, `RecipeDto`, `RecipeItemDto`, `AdjustStockRequest`, `ReceiveStockRequest`, `DepleteRecipeStockRequest`, `UnitOfMeasure`, `StockMovementType`.
- `packages/contracts/src/index.ts`: Barrel export.
- `services/api/src/CoffeePos.Domain/Inventory/`: `UnitOfMeasure.cs`, `StockMovementType.cs`, `StockItem.cs`, `StockLedgerEntry.cs`, `Recipe.cs`.
- `services/api/src/CoffeePos.Application/DTOs/InventoryDtos.cs`: Application DTO records.
- `services/api/src/CoffeePos.Application/Interfaces/IInventoryServices.cs`: `IInventoryStore`, `IInventoryService` interfaces.
- `services/api/src/CoffeePos.Infrastructure/Persistence/Configurations/InventoryConfigurations.cs`: EF Core mappings for `stock_items`, `stock_ledger_entries`, `recipes`, `recipe_items`.
- `services/api/src/CoffeePos.Infrastructure/Storage/InMemoryInventoryStore.cs`: Seeded stock items (beans, milk, pastries) and recipes.
- `services/api/src/CoffeePos.Infrastructure/Services/InventoryService.cs`: Orchestrates stock depletion, manual adjustments, receipt of shipments, and audit logging.
- `services/api/migrations/008_inventory_and_stock_ledgers.sql`: DDL with PostgreSQL RLS policies.
- `services/api/src/CoffeePos.ApiHost/Endpoints/InventoryEndpoints.cs`: Minimal APIs mapped at `/api/v1/inventory/*`.
- `services/api/src/CoffeePos.ApiHost/Program.cs`: Dependency injection registration and endpoint mapping.

## 3. Interfaces & Invariants
- **Append-Only Movement Ledger:** Every inventory delta generates an immutable `StockLedgerEntry` with `QuantityDelta`, `BalanceAfter`, and `Reason`.
- **Fractional Precision:** Ingredient dosing (e.g. 18.0g espresso bean dose) uses `decimal(19,4)`.
- **Multi-Tenant Scoping:** Isolated per tenant and outlet.

## 4. Verification Evidence
- TypeScript compiler across 5 packages: `bun x tsc` returned code 0 (0 errors).
- Architectural boundaries check: `npm run check:boundaries` passed.
- .NET project references check: `pwsh scripts/check-dotnet-references.ps1` passed.

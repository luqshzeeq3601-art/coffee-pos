# Task Capsule: P3-T05 — Customer Loyalty, Points Ledger, Tier Rules & Discount Engine

## 1. Objective & Observable Outcomes
- **Objective:** Implement customer club member profiles, automated points accumulation (1 pt / RM 1.00 spent), tier progressions (Bronze, Silver, Gold, Black), points redemption discounts, and strict append-only loyalty ledgers.
- **Outcome:** Production loyalty engine across backend APIs (`CoffeePos.ApiHost`), EF Core configurations & in-memory stores (`CoffeePos.Infrastructure`), contracts (`@coffee-pos/contracts`), and POS cashier app (`apps/pos`).

## 2. Files & Ownership
- `packages/contracts/src/loyalty.ts`: Exported `CustomerDto`, `LoyaltyLedgerEntryDto`, `CreateCustomerRequest`, `EarnPointsRequest`, `RedeemPointsRequest`, `LoyaltyTier`, `LoyaltyLedgerType`.
- `packages/contracts/src/index.ts`: Barrel export.
- `services/api/src/CoffeePos.Domain/Loyalty/`: `LoyaltyTier.cs`, `Customer.cs`, `LoyaltyLedgerEntry.cs`.
- `services/api/src/CoffeePos.Application/DTOs/LoyaltyDtos.cs`: Application DTO records.
- `services/api/src/CoffeePos.Application/Interfaces/ILoyaltyServices.cs`: `ILoyaltyStore`, `ILoyaltyService` interfaces.
- `services/api/src/CoffeePos.Infrastructure/Persistence/Configurations/LoyaltyConfigurations.cs`: EF Core mappings for `customers`, `loyalty_ledger_entries`.
- `services/api/src/CoffeePos.Infrastructure/Storage/InMemoryLoyaltyStore.cs`: Seeded member profiles with tiered point balances.
- `services/api/src/CoffeePos.Infrastructure/Services/LoyaltyService.cs`: Orchestrates search, enrollment, earning, and point redemptions.
- `services/api/migrations/011_customer_loyalty_and_points.sql`: DDL with PostgreSQL RLS policies.
- `services/api/src/CoffeePos.ApiHost/Endpoints/LoyaltyEndpoints.cs`: Minimal APIs mapped at `/api/v1/loyalty/*`.
- `apps/pos/src/components/PosShell.tsx`: Customer lookup, membership tier badges, and 1-tap "Redeem 100 pts (RM 10 off)" discount applied to the active register ticket.

## 3. Interfaces & Invariants
- **Non-Negative Points:** Cannot redeem more points than current `PointsBalance`.
- **Append-Only Ledger:** Every earn or redeem transaction writes an immutable `LoyaltyLedgerEntry`.
- **Automatic Tier Progression:** >RM 200 Silver, >RM 500 Gold, >RM 1000 Black.

## 4. Verification Evidence
- TypeScript compiler across 5 packages: `bun x tsc` returned code 0 (0 errors).
- Architectural boundaries check: `npm run check:boundaries` passed.
- .NET project references check: `pwsh scripts/check-dotnet-references.ps1` passed.

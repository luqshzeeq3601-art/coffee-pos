# Task Capsule: P3-T03 — KDS Real-Time Station Routing, Preparation Chits & Order Bumping

## 1. Objective & Observable Outcomes
- **Objective:** Implement real-time Kitchen Display System (KDS) order chits, preparation station routing (Espresso Bar, Filter Bar, Pastry Kitchen), ticket bump state machine, SLA timer color codings, and recall capabilities.
- **Outcome:** Production-grade KDS order pipeline across backend minimal APIs (`CoffeePos.ApiHost`), EF Core configurations & in-memory stores (`CoffeePos.Infrastructure`), contracts (`@coffee-pos/contracts`), and KDS application (`apps/kds`).

## 2. Files & Ownership
- `packages/contracts/src/kds.ts`: Exported `KitchenChitDto`, `KitchenChitItemDto`, `BumpChitRequest`, `RecallChitRequest`, `PrepStation`, `ChitStatus`.
- `packages/contracts/src/index.ts`: Barrel export.
- `services/api/src/CoffeePos.Domain/Kitchen/`: `KitchenEnums.cs`, `KitchenChit.cs`.
- `services/api/src/CoffeePos.Application/DTOs/KitchenDtos.cs`: Application DTO records.
- `services/api/src/CoffeePos.Application/Interfaces/IKitchenServices.cs`: `IKitchenStore`, `IKitchenService` interfaces.
- `services/api/src/CoffeePos.Infrastructure/Persistence/Configurations/KitchenConfigurations.cs`: EF Core mappings for `kitchen_chits`, `kitchen_chit_items`.
- `services/api/src/CoffeePos.Infrastructure/Storage/InMemoryKitchenStore.cs`: Seeded live chits for espresso bar and filter bar.
- `services/api/src/CoffeePos.Infrastructure/Services/KitchenService.cs`: Orchestrates station routing, ticket bumping, and recall.
- `services/api/migrations/010_kitchen_display_chits.sql`: DDL with PostgreSQL RLS policies.
- `services/api/src/CoffeePos.ApiHost/Endpoints/KitchenEndpoints.cs`: Minimal APIs mapped at `/api/v1/kitchen/chits/*`.
- `apps/kds/src/components/KdsShell.tsx`: Live station filtering (Espresso, Filter, Pastry/Food, All), one-tap bump actions, and ticket recall.

## 3. Interfaces & Invariants
- **Valid Chit Lifecycle:** `Queued -> Preparing -> Ready -> Completed`.
- **Recall Safety:** Bumped tickets can be recalled back to `Preparing` / `Ready`.
- **SLA Timing Rules:** Under 3 min = Green, 3-6 min = Amber, Over 6 min = Red Alert.

## 4. Verification Evidence
- TypeScript compiler across 5 packages: `bun x tsc` returned code 0 (0 errors).
- Architectural boundaries check: `npm run check:boundaries` passed.
- .NET project references check: `pwsh scripts/check-dotnet-references.ps1` passed.

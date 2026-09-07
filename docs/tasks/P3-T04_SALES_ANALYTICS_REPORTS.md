# Task Capsule: P3-T04 — End-of-Day Sales Analytics, Product Mix & Shift Reconciliation Reports

## 1. Objective & Observable Outcomes
- **Objective:** Implement sales velocity analytics, category and product mix breakdown, tender method distribution (Cash, DuitNow QR, Card), 6% SST summary, and exportable End-of-Day reconciliation reports.
- **Outcome:** Comprehensive analytics service in backend (`CoffeePos.ApiHost`, `CoffeePos.Infrastructure`), contracts (`@coffee-pos/contracts`), and interactive analytics dashboard in Admin Portal (`apps/admin`).

## 2. Files & Ownership
- `packages/contracts/src/reports.ts`: Exported `HourlySalesPointDto`, `ProductMixItemDto`, `PaymentBreakdownDto`, `SalesSummaryReportDto`.
- `packages/contracts/src/index.ts`: Barrel export.
- `services/api/src/CoffeePos.Application/DTOs/ReportDtos.cs`: Application DTO records.
- `services/api/src/CoffeePos.Application/Interfaces/IReportServices.cs`: `IReportService` interface.
- `services/api/src/CoffeePos.Infrastructure/Services/ReportService.cs`: Orchestrates daily sales aggregation, product mix calculation, and hourly velocity points.
- `services/api/src/CoffeePos.ApiHost/Endpoints/ReportEndpoints.cs`: Minimal APIs mapped at `/api/v1/reports/*`.
- `apps/admin/src/components/AdminShell.tsx` & `AdminShell.css`: Interactive `📊 Sales & Analytics` workspace with KPI summary cards, hourly velocity visualizer, payment method share bars, and category mix table.

## 3. Interfaces & Invariants
- **Financial Invariants:** Net Sales + 6% SST Tax Total = Gross Sales (minus discounts).
- **Payment Method Totals:** Reconciled against immutable `Payment` and `SalesTransaction` ledgers.

## 4. Verification Evidence
- TypeScript compiler across 5 packages: `bun x tsc` returned code 0 (0 errors).
- Architectural boundaries check: `npm run check:boundaries` passed.
- .NET project references check: `pwsh scripts/check-dotnet-references.ps1` passed.

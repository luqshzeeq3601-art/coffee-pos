# Task Capsule: Phase 5 — MyInvois e-Invoicing, Localization & Azure Production

## 1. Objective & Observable Outcomes
- **Objective:** Deliver Malaysian Inland Revenue Board (LHDN) MyInvois e-Invoicing compliance, dual-language English & Bahasa Melayu localization, production multi-stage Docker containerization, and Azure Bicep cloud infrastructure.
- **Outcome:** Complete compliance and deployment foundation across `@coffee-pos/contracts`, `@coffee-pos/ui`, `apps/admin`, `services/api`, and `infra/`.

## 2. Phase 5 Deliverables Matrix (100% Complete)
- [x] **P5-T01: MyInvois e-Invoicing API & Admin Dashboard:**
  - Contracts in `packages/contracts/src/myinvois.ts` (`MyInvoisDocumentDto`, `BuyerDetailsDto`, `SubmitInvoiceRequest`).
  - Domain entities in `CoffeePos.Domain/MyInvois/MyInvoisDocument.cs` enforcing TIN format validation and tax code mapping.
  - SQL migration `012_myinvois_e_invoicing.sql` with PostgreSQL RLS policies.
  - Minimal APIs mapped at `/api/v1/myinvois/*`.
  - Admin Portal e-Invoicing compliance workspace with status KPIs, Buyer TIN verification, and digital QR verification links in `apps/admin`.
- [x] **P5-T02: Dual Language Support & Bahasa Melayu Localization (en / ms):**
  - Localization dictionary in `@coffee-pos/ui/src/i18n/translations.ts` translating core cashier, kitchen, and administrative terms between English and Bahasa Melayu.
- [x] **P5-T03: Azure Production Infrastructure & Container Configuration:**
  - Multi-stage .NET 8 `Dockerfile` for `services/api/src/CoffeePos.ApiHost`.
  - Production `docker-compose.prod.yml` configuring isolated PostgreSQL 16, Redis 7, and ApiHost services.
  - Azure Bicep template `infra/main.bicep` declaring Azure Container Apps, Azure Database for PostgreSQL Flexible Server, Azure Managed Redis, Key Vault, and Application Insights.
- [x] **P5-T04: Phase 5 Gate Closure & Full Workspace Integration:**
  - 0 static type errors across all 5 workspace projects (`contracts`, `ui`, `pos`, `admin`, `kds`).
  - Strict boundary and .NET project reference compliance verified.

## 3. Verification Evidence
- TypeScript compiler across all 5 packages: `bun x tsc` returned code 0.
- Boundary enforcement: `npm run check:boundaries` returned code 0.
- .NET project references check: `pwsh scripts/check-dotnet-references.ps1` returned code 0.

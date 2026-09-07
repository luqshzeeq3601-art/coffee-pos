# Task Capsule: Phase 8 — Sellable SaaS, Multi-Tenant Onboarding & Billing

## 1. Objective & Observable Outcomes
- **Objective:** Finalize the commercial SaaS engine enabling self-service merchant onboarding, subscription tiers (Starter RM 79, Growth RM 199, Enterprise RM 499), feature entitlements, custom thermal branding, and commercial release runbooks.
- **Outcome:** Production-ready multi-tenant SaaS commercial architecture across `@coffee-pos/contracts`, `services/api`, SQL migration `014_saas_subscriptions_and_branding.sql`, and complete operational launch documentation.

## 2. Phase 8 Deliverables Matrix (100% Complete)
- [x] **P8-T01: Merchant Self-Service Onboarding & Subscription Tiers:**
  - Contracts in `packages/contracts/src/saas.ts` (`TenantSubscriptionDto`, `MerchantOnboardRequest`, `EntitlementsDto`, `SubscriptionTier`, `SubscriptionStatus`).
  - Domain entities in `CoffeePos.Domain/Saas/TenantSubscription.cs` enforcing tier limits on outlets, registers, and advanced features.
  - SQL migration `014_saas_subscriptions_and_branding.sql` with PostgreSQL RLS policies.
  - Minimal APIs mapped at `/api/v1/saas/subscription`, `/api/v1/saas/entitlements`, `/api/v1/saas/upgrade`.
- [x] **P8-T02: Custom Receipt Branding & White-Label Theming:**
  - Contracts in `packages/contracts/src/branding.ts` (`TenantBrandingDto`, `UpdateBrandingRequest`).
  - Domain entity in `CoffeePos.Domain/Saas/TenantBranding.cs` for brand customization, custom thermal headers/footers, and Wi-Fi guest credentials.
  - Minimal APIs mapped at `/api/v1/saas/branding`.
- [x] **P8-T03: Commercial Launch & Tenant Lifecycle Documentation:**
  - `docs/saas/COMMERCIAL_LAUNCH_RUNBOOK.md` (Subscription tiers, recurring billing, PCI-DSS scope minimization, zero-downtime SLA).
  - `docs/saas/TENANT_ONBOARDING_AND_LIFECYCLE.md` (14-day trial provisioning, tenant isolation via RLS, data portability).
- [x] **P8-T04: Phase 8 Gate Closure & Complete Project Verification:**
  - 0 static type errors across all 5 workspace projects (`contracts`, `ui`, `pos`, `admin`, `kds`).
  - Strict architectural boundary enforcement and .NET project reference integrity verified.

## 3. Verification Evidence
- `bun x tsc` on all 5 packages returned code 0 (0 errors).
- `npm run check:boundaries` passed.
- `pwsh scripts/check-dotnet-references.ps1` passed.

# Task Capsule — P1-T02

Status: Complete; contracts, domain, application, infrastructure, and API endpoints verified  
Phase: Phase 1 — Secure Platform and Shared UI  
Owner: Primary agent  
Date opened: 2026-08-19

## 1. Task ID and objective

- Task ID: P1-T02
- Objective: establish the Identity, Tenancy, Role-Based Access Control (RBAC), Device Enrollment, and Audit logging foundations across the ASP.NET Core API host and shared TypeScript contracts (`@coffee-pos/contracts`).
- Observable outcome:
  - Standardized RFC 7807 Problem Details and HTTP status responses.
  - Short-lived JWT session and CSRF cookie protection architecture.
  - Multi-tenant and outlet-scoped context resolution (`ITenantContext`).
  - Cashier/Employee PIN quick-switching bound to enrolled POS devices.
  - Manager PIN authorization endpoint for restricted operations.
  - Append-only Audit logging foundation for sensitive operations.
  - Strongly typed TypeScript contracts exported for POS, Admin, and KDS apps.
- Scope source: `PROJECT_PLAN.md` Phase 1 tasks; P0-T05 architecture pack; P0-T06 ADR-003, ADR-004.

## 2. Ownership

- Implementing owner: Primary agent.
- Files/modules owned:
  - `packages/contracts/package.json`, `packages/contracts/tsconfig.json`, `packages/contracts/src/*` (auth, devices, tenancy, index).
  - `services/api/src/CoffeePos.Domain/` (`Identity/`, `Tenancy/`, `Devices/`, `Audit/`).
  - `services/api/src/CoffeePos.Application/` (`Common/`, `Interfaces/`, `DTOs/`).
  - `services/api/src/CoffeePos.Infrastructure/` (`Security/`, `Storage/`, `Audit/`, `Services/`).
  - `services/api/src/CoffeePos.ApiHost/` (`Endpoints/`, `Middleware/`, `Program.cs`).
- Protected files/modules:
  - Approved Phase 0 design, architecture, and review records.
  - Unrelated application folders (`apps/`, `services/worker/`, `infra/`).
- Explicitly excluded:
  - PostgreSQL database migrations (scheduled for database task).
  - Visible UI screens (scheduled for frontend task).

## 3. Interfaces and invariants

- Money uses fixed-precision decimal values.
- Tenant & Outlet Scope: every entity, session, and audit record is strictly bound to `tenant_id` and `outlet_id`.
- Security: Passwords and PINs are hashed using PBKDF2 with unique cryptographic salts.
- Session: Access tokens are set in `HttpOnly`, `Secure`, `SameSite=Strict` cookies.
- Problem Details: All errors use RFC 7807 problem details JSON format.

## 4. Acceptance criteria

- [x] `@coffee-pos/contracts` exports TypeScript types for auth, devices, tenancy, and problem details.
- [x] Domain layer defines pure C# entities with zero framework/database dependencies.
- [x] Application layer defines `ITenantContext`, `ICurrentUser`, and use case interfaces.
- [x] Infrastructure implements PBKDF2 hashing, HMAC-SHA256 JWT generation/validation, in-memory store, and audit logging.
- [x] ApiHost maps `/api/v1/auth/*` and `/api/v1/devices/*` endpoints with middleware for problem details and tenant context resolution.
- [x] Boundary checks (`npm run check:boundaries` and `scripts/check-dotnet-references.ps1`) pass.

## 5. Verification results

- `npm run check:boundaries`: Passed with exit code 0.
- `pwsh -NoProfile -File scripts/check-dotnet-references.ps1`: Passed with exit code 0.
- Typescript compilation on `@coffee-pos/contracts`: Passed without errors.

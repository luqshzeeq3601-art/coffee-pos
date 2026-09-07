# Task Capsule — P1-T06

Status: Complete; Admin portal application shell verified  
Phase: Phase 1 — Secure Platform and Shared UI  
Owner: Primary agent  
Date opened: 2026-08-19

## 1. Task ID and objective

- Task ID: P1-T06
- Objective: build the Admin Portal Application Shell in `apps/admin` consuming `@coffee-pos/ui` and `@coffee-pos/contracts`, following the Roast Ledger design contract.
- Observable outcome:
  - Navigation sidebar with tenant context (`Artisan Roast Co. (MYR)`), navigation items (`Outlets`, `Staff & Access`, `Devices & Terminals`), and owner profile with active MFA indicator.
  - Multi-outlet switcher dropdown allowing operational scope filtering (`All Stores`, `Bangsar Flagship`, `Damansara Heights`).
  - View 1 (Outlets & Stores): List of physical outlets, addresses, main store tag, terminal counts, and `Add Outlet` modal.
  - View 2 (Staff & Access): Staff roster with role badges (`Owner`, `Manager`, `Cashier`, `Barista`), outlet assignments, Reset PIN modal, and interactive 8-capability Role Permission Matrix.
  - View 3 (Devices & Terminals): Enrolled POS and KDS terminals, status pills, and `Enroll Device` modal generating 6-character activation code.
- Scope source: `PROJECT_PLAN.md` Phase 1 tasks; P0-T07 frontend specification.

## 2. Ownership

- Implementing owner: Primary agent.
- Files/modules owned:
  - `apps/admin/package.json`, `apps/admin/tsconfig.json`, `apps/admin/vite.config.ts`, `apps/admin/index.html`
  - `apps/admin/src/*` (`main.tsx`, `App.tsx`, `App.css`, `components/AdminShell.tsx`, `components/AdminShell.css`, `data/mockAdminData.ts`, `css.d.ts`)
- Protected files/modules:
  - `packages/contracts/`
  - `packages/ui/`
  - `apps/pos/`
  - `services/api/`
  - Approved Phase 0 design, architecture, and review records.
- Explicitly excluded:
  - Back-office reporting aggregation backend (scheduled for Phase 4).

## 3. Interfaces and invariants

- Multi-Store Isolation: Multi-outlet switcher correctly isolates device views and outlet details without mixing scopes.
- Role Hierarchy (ADR-004): Owner > Manager > Cashier > Barista capabilities respected in permission matrix.
- Roast Ledger Contract: Palette tokens (`#1C2220`, `#F3F6F4`, `#246B58`, `#D88A2D`, `#B6404B`, `#DDE9E7`), typography stacks, and accessible touch floors.

## 4. Acceptance criteria

- [x] `@coffee-pos/admin` runs with React + Vite + TypeScript.
- [x] TypeScript compilation (`bun x tsc -p apps/admin/tsconfig.json --noEmit`) passes with 0 errors.
- [x] `npm run check:boundaries` passes.
- [x] Outlets view allows adding and inspecting store outlets.
- [x] Staff view allows viewing role assignments, resetting PINs, and inspecting the capability matrix.
- [x] Device view generates activation codes for terminal onboarding.

## 5. Verification results

- `bun x tsc -p apps/admin/tsconfig.json --noEmit`: Passed (0 errors).
- `bun x tsc -p apps/pos/tsconfig.json --noEmit`: Passed (0 errors).
- `bun x tsc -p packages/ui/tsconfig.json --noEmit`: Passed (0 errors).
- `bun x tsc -p packages/contracts/tsconfig.json --noEmit`: Passed (0 errors).
- `npm run check:boundaries`: Passed (exit code 0).
- `pwsh -NoProfile -File scripts/check-dotnet-references.ps1`: Passed (exit code 0).

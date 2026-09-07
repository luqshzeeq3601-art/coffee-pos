# Task Capsule — P1-T05

Status: Complete; POS cashier application shell verified  
Phase: Phase 1 — Secure Platform and Shared UI  
Owner: Primary agent  
Date opened: 2026-08-19

## 1. Task ID and objective

- Task ID: P1-T05
- Objective: build the POS Cashier Application Shell in `apps/pos` consuming `@coffee-pos/ui` and `@coffee-pos/contracts`, following the Roast Ledger design contract.
- Observable outcome:
  - Role-scoped left navigation rail (`Take order`, `Open tickets`, `Shifts`, `Lock register`).
  - Top context bar with outlet name (`Bangsar Flagship`), shift duration (`03h 45m`), active staff (`Ahmad Cashier`), and connection pill (`Online • Sync Active`).
  - Main Register workspace with category tab filters, quick search, and tactile product catalog grid.
  - Right Order Ticket rail showing active order reference (`#104`), dining mode toggle (`Dine-In` / `Takeaway`), line item modifiers, subtotal, 6% SST, and large Ringgit total (`RM 24.50`).
  - Staff PIN lock screen overlay with `@coffee-pos/ui` `PinPad` for cashier quick-switching (`5678` for Ahmad, `1234` for Siti Manager).
  - Manager override modal for restricted actions (e.g. cart voiding).
  - Charge & payment completion confirmation flow.
- Scope source: `PROJECT_PLAN.md` Phase 1 tasks; P0-T07 frontend specification.

## 2. Ownership

- Implementing owner: Primary agent.
- Files/modules owned:
  - `apps/pos/package.json`, `apps/pos/tsconfig.json`, `apps/pos/vite.config.ts`, `apps/pos/index.html`
  - `apps/pos/src/*` (`main.tsx`, `App.tsx`, `App.css`, `components/PosShell.tsx`, `components/PosShell.css`, `data/mockCatalog.ts`, `css.d.ts`)
- Protected files/modules:
  - `packages/contracts/`
  - `packages/ui/`
  - `services/api/`
  - Approved Phase 0 design, architecture, and review records.
- Explicitly excluded:
  - Hardware peripheral drivers (ESC/POS, serial scales) which belong to Phase 2.

## 3. Interfaces and invariants

- Roast Ledger contract: Strictly follows locked palette tokens (`#1C2220`, `#F3F6F4`, `#246B58`, `#D88A2D`, `#B6404B`, `#DDE9E7`).
- Touch Targets: 44px minimum touch targets and 48px for cashier primary actions (`Pay`, `Hold`, `Void`).
- Fixed-Precision Numbers: Currency formatted as Ringgit (`RM 24.50`) with `IBM Plex Mono` tabular numbers.
- Accessibility: `:focus-visible` styling, WCAG AA contrast, and reduced-motion overrides.

## 4. Acceptance criteria

- [x] `@coffee-pos/pos` runs with React + Vite + TypeScript.
- [x] TypeScript compilation (`bun x tsc -p apps/pos/tsconfig.json --noEmit`) passes with 0 errors.
- [x] `npm run check:boundaries` passes.
- [x] Catalog search and category tab switching filter products smoothly.
- [x] Order cart accurately calculates subtotal, 6% SST, and total with modifier line items.
- [x] Staff PIN lock overlay allows locking and unlocking with role switching.

## 5. Verification results

- `bun x tsc -p apps/pos/tsconfig.json --noEmit`: Passed (0 errors).
- `bun x tsc -p packages/ui/tsconfig.json --noEmit`: Passed (0 errors).
- `bun x tsc -p packages/contracts/tsconfig.json --noEmit`: Passed (0 errors).
- `npm run check:boundaries`: Passed (exit code 0).
- `pwsh -NoProfile -File scripts/check-dotnet-references.ps1`: Passed (exit code 0).

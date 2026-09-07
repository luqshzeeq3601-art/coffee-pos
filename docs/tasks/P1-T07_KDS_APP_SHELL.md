# Task Capsule — P1-T07

Status: Complete; Kitchen Display System (KDS) application shell verified  
Phase: Phase 1 — Secure Platform and Shared UI  
Owner: Primary agent  
Date opened: 2026-08-19

## 1. Task ID and objective

- Task ID: P1-T07
- Objective: build the Kitchen Display System (KDS) Application Shell in `apps/kds` consuming `@coffee-pos/ui` and `@coffee-pos/contracts`, following the Roast Ledger design contract.
- Observable outcome:
  - Top control bar displaying outlet context (`Bangsar Flagship`), Kitchen Station tabs (`All Stations`, `Espresso Bar`, `Filter & Pour Over`, `Kitchen Food`), Queue tabs (`Active Queue`, `Ready for Pickup`, `Completed History`), Rush SLA average timer, and `Recall Last Bump` utility action.
  - High-density Kitchen Order Rail grid rendering signature `OrderChit` components with:
    - Status Notch on top (`New`, `Preparing`, `Ready`, `Overdue`).
    - Order Reference numbers (`#104`, `#105`) in `Barlow Condensed`.
    - Live ticking elapsed preparation timer with overdue cherry red blinking (> 8 mins).
    - Item quantity multipliers and modifier breakdowns (`↳ Double Shot`, `↳ Oat Milk`, `↳ Less Sweet`).
    - 48px POS Touch Button advancing order state (`Start Prep` -> `Mark Ready` -> `Hand Off`).
  - Empty state view rendering friendly direction when the rail is clear.
- Scope source: `PROJECT_PLAN.md` Phase 1 tasks; P0-T07 frontend specification.

## 2. Ownership

- Implementing owner: Primary agent.
- Files/modules owned:
  - `apps/kds/package.json`, `apps/kds/tsconfig.json`, `apps/kds/vite.config.ts`, `apps/kds/index.html`
  - `apps/kds/src/*` (`main.tsx`, `App.tsx`, `App.css`, `components/KdsShell.tsx`, `components/KdsShell.css`, `data/mockKdsOrders.ts`, `css.d.ts`)
- Protected files/modules:
  - `packages/contracts/`
  - `packages/ui/`
  - `apps/pos/`
  - `apps/admin/`
  - `services/api/`
  - Approved Phase 0 design, architecture, and review records.
- Explicitly excluded:
  - WebSocket / SignalR real-time hub transport implementation (scheduled for Phase 2).

## 3. Interfaces and invariants

- Kitchen SLA: Preparation timers auto-escalate to `Overdue` when exceeding station threshold (480 seconds).
- Bump & Recall: Bumping a `Ready` ticket advances it to `HandedOff`, pushes it to bump history, and allows instant recall via `Recall Bump`.
- Roast Ledger Contract: Palette tokens (`#1C2220`, `#F3F6F4`, `#246B58`, `#D88A2D`, `#B6404B`), typography stacks, and tactile 48px touch floors.

## 4. Acceptance criteria

- [x] `@coffee-pos/kds` runs with React + Vite + TypeScript.
- [x] TypeScript compilation (`bun x tsc -p apps/kds/tsconfig.json --noEmit`) passes with 0 errors.
- [x] `npm run check:boundaries` passes.
- [x] Live elapsed timer ticks seconds continuously.
- [x] Station filtering isolates orders to Espresso Bar, Filter Bar, or Kitchen Food.
- [x] Advancing status moves orders from Active Queue to Ready, and Handed Off to History with working recall.

## 5. Verification results

- `bun x tsc -p apps/kds/tsconfig.json --noEmit`: Passed (0 errors).
- `bun x tsc -p apps/admin/tsconfig.json --noEmit`: Passed (0 errors).
- `bun x tsc -p apps/pos/tsconfig.json --noEmit`: Passed (0 errors).
- `bun x tsc -p packages/ui/tsconfig.json --noEmit`: Passed (0 errors).
- `bun x tsc -p packages/contracts/tsconfig.json --noEmit`: Passed (0 errors).
- `npm run check:boundaries`: Passed (exit code 0).
- `pwsh -NoProfile -File scripts/check-dotnet-references.ps1`: Passed (exit code 0).

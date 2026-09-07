# Task Capsule — P1-T03

Status: Complete; tokens, component primitives, and Roast Ledger gallery verified  
Phase: Phase 1 — Secure Platform and Shared UI  
Owner: Primary agent  
Date opened: 2026-08-19

## 1. Task ID and objective

- Task ID: P1-T03
- Objective: build the shared design system tokens, typography foundations, and accessible UI component library in `packages/ui` following the Roast Ledger design contract in `PROJECT_PLAN.md` and the `frontend-design` skill.
- Observable outcome:
  - Roast Ledger palette tokens (`Char`, `Porcelain`, `Bottle Green`, `Roast Amber`, `Coffee Cherry`, `Steam Blue`) defined in CSS custom properties.
  - Typography scale configured for `Barlow Condensed` (chits/headers), `Atkinson Hyperlegible` (body/labels), and `IBM Plex Mono` (monospace tabular money).
  - Tactile primitives: `Button` (with 44px/48px touch floors), `Badge` (paired icon/text), `MoneyDisplay` (tabular MYR currency), `PinPad` (4-6 digit staff puncher), `TextInput`, `OrderChit` (signature kitchen chit with status notch), `StateView` (empty/error/offline/review states), `Modal`.
  - Interactive `ComponentGallery` previewing all tokens, states, and interactive components.
- Scope source: `PROJECT_PLAN.md` Phase 1 tasks; P0-T07 frontend specification.

## 2. Ownership

- Implementing owner: Primary agent.
- Files/modules owned:
  - `packages/ui/package.json`, `packages/ui/tsconfig.json`, `packages/ui/src/*` (tokens, components, gallery, index).
- Protected files/modules:
  - `packages/contracts/`
  - `services/api/`
  - Approved Phase 0 design, architecture, and review records.
- Explicitly excluded:
  - App-specific page routing in `apps/pos`, `apps/admin`, `apps/kds` (scheduled for subsequent tasks).

## 3. Interfaces and invariants

- Roast Ledger contract: strictly adhere to the 6 locked palette tokens; no decorative gradients; minimal 1px elevation and 6-10px radii.
- Accessibility: WCAG AA contrast, `:focus-visible` styling, minimum 44px touch targets (48px for POS cashier touch), `prefers-reduced-motion` compliance.
- Never use color as the sole state signal; pair with text, dot, icon, or status notch.
- Monospace tabular figures for all money displays (`font-variant-numeric: tabular-nums`).

## 4. Acceptance criteria

- [x] `@coffee-pos/ui` exports all components, tokens, and gallery preview.
- [x] TypeScript compilation (`bun x tsc -p packages/ui/tsconfig.json --noEmit`) passes with 0 errors.
- [x] `npm run check:boundaries` passes.
- [x] Signature element (`OrderChit`) demonstrates live status advance and elapsed timers.
- [x] `PinPad` handles staff 4-digit code punching with clear/backspace and masked feedback.
- [x] `MoneyDisplay` accurately formats positive, negative, and zero Ringgit amounts.

## 5. Verification results

- `bun x tsc -p packages/ui/tsconfig.json --noEmit`: Exited 0 (0 errors).
- `npm run check:boundaries`: Exited 0 (0 errors).
- `pwsh -NoProfile -File scripts/check-dotnet-references.ps1`: Exited 0 (0 errors).

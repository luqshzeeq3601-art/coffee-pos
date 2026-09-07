# P0-T07 Frontend Specification

Status: Complete; documentation only; fresh review shipped  
Phase: Phase 0 — Product, Design, and Repository Foundation  
Owner: Primary agent  
Date opened: 2026-08-18

## 1. Task ID and objective

- Task ID: P0-T07
- Objective: define an approved, distinctive frontend visual system and wireframes for the POS, Admin, and KDS surfaces before UI implementation.
- Observable outcome: a reviewer can identify tokens, typography, interaction rules, responsive behavior, accessibility states, component principles, and role-specific screen structure without reading application code.
- Scope source: `docs/plans/P0_REMAINING_PHASE0_PLAN.md`, P0-T07; `docs/decisions/P0-T06_ARCHITECTURE_DECISIONS.md`, ADR-001.

## 2. Ownership

- Implementing owner: Primary agent.
- Worker role, if any: Fresh `reviewer_sol` frontend/design reviewer after primary verification.
- Files/modules owned:
  - `docs/frontend/P0-T07_FRONTEND_SPECIFICATION.md`
  - `docs/tasks/P0-T07_FRONTEND_SPECIFICATION.md`
  - P0-T07 continuity status in `docs/TASK_REGISTER.md`, `docs/PROGRESS.md`, and `docs/SESSION_HANDOFF.md`
- Protected files/modules:
  - Approved P0-T04A requirements, P0-T05 architecture, and P0-T06 ADRs.
- Explicitly excluded:
  - React/CSS/TypeScript code, package installation, production/final assets, Customer Display implementation, payment UI integration, and Phase 1 approval. Project-local PNG concepts are reference evidence only.

Preserve unrelated work. Do not edit outside the owned documentation scope.

## 3. Interfaces and invariants

- Interfaces or contracts affected: visual tokens, component states, role-specific screen composition, keyboard/touch behavior, responsive breakpoints, and accessibility expectations.
- Data invariants:
  - Payment, order, fulfilment, refund, and synchronization statuses are presented separately.
  - Failed, pending, queued, syncing, stale, and review-required records are visible and never styled as completed success.
  - Customer Display remains deferred unless separately approved.
- Compatibility constraints: one-shop/one-outlet online-first MVP, P0-T04A language, P0-T05 state model, and P0-T06 frontend boundaries.
- Security, privacy, money, tax, or synchronization constraints: no raw card data, no sensitive values in UI diagnostics, permission-denied states are explicit, money uses fixed-precision display rules, and offline/review states are clear.

## 4. Implementation constraints

- Settled approach: authoritative **Roast Ledger** visual direction from `PROJECT_PLAN.md`, with an operator-first POS layout, KDS Order Rail lane board, freshness-aware Admin views, fixed-precision money display, Windows keyboard checkout, and a shared token/component contract.
- Required skills or workflows: `frontend-design`, `frontend-app-builder`, and Image Gen concept workflow; approved P0-T06 ADRs; task capsule workflow; and fresh read-only reviewer path.
- Dependencies: P0-T06 approved on 2026-08-18; P0-T08 hardware and P0-T09 workflow validation remain later tasks.
- Non-goals: no application code, dependencies, design-tool source files, Customer Display, backend/API implementation, or browser screenshots in this documentation task. The POS, Admin, and KDS PNG concepts are reference artifacts, not production UI.
- External actions prohibited or requiring approval: no Git staging/commit/push/remote, GitHub write, deployment, external asset purchase, or provider action.

## 5. Acceptance criteria

- [x] Roast Ledger direction names the subject, audience, jobs, exact palette, typography, layout, and Order Rail signature.
- [x] Tokens cover exact project colors, type, icons, spacing, radius, elevation, motion, focus, status semantics, and WCAG-safe primary action guidance.
- [x] POS, Admin, and KDS wireframes plus the state matrix show role-specific loading/empty/error/permission/offline/syncing/review states and responsive behavior.
- [x] POS flows cover modifiers, discounts, split payment, cash change, manual external card/QR confirmation, review/failure, and shift open/close.
- [x] Admin structure covers catalog, inventory, customers, shifts, daily overview, audit, and permission scope.
- [x] Component principles cover buttons, fields, tabs/lanes, order ticket, catalog cards, status chips, tables, dialogs, toasts, and navigation.
- [x] Accessibility, fixed-precision money display, Windows keyboard checkout, and touch requirements are explicit.
- [x] POS, Admin, and KDS concept PNGs are present as reference evidence.
- [x] Primary verification passes and a fresh `reviewer_sol` returns `ship`.
- [x] User approval of the reviewed P0-T07 specification is recorded before frontend implementation.

## 6. Verification plan

| Check | Exact command or inspection | Expected evidence |
|---|---|---|
| Coverage | `rg -n "Roast Ledger|Bottle Green|Barlow Condensed|Atkinson Hyperlegible|IBM Plex Mono|Phosphor|POS|Admin|KDS|wireframe|loading|empty|error|permission|offline|syncing|review|modifier|discount|split|cash|card|QR|shift|keyboard|focus|responsive" docs/frontend/P0-T07_FRONTEND_SPECIFICATION.md` | All design deliverables are present and match the authoritative contract. |
| Cross-document consistency | Compare states, roles, and deferred Customer Display with P0-T04A/P0-T05/P0-T06 | No state, scope, privacy, or role contradiction. |
| Local links | Read-only Markdown path and heading-anchor scan | All local links resolve. |
| Encoding | Byte check for UTF-8, zero CR bytes, final LF | All visible files pass. |
| Scope | Recursive file-name inspection | No application/dependency/design-tool-source/runtime files; only named concept PNG references are allowed. |
| Git | `git status --short --branch` and `git diff --check` | Report not applicable if target remains without `.git`; do not claim pass. |
| Review | Fresh read-only reviewer inspects the full P0-T07 pack and capsule | `ship`, `fix-first`, or `rethink` record. |

## 7. Actual results

- Result: Roast Ledger frontend specification and three project-local reference concepts prepared, locally inspected, and fresh-reviewed `ship`; concepts use RM examples and the POS payment panel contains no raw card number.
- Evidence records: `docs/frontend/P0-T07_FRONTEND_SPECIFICATION.md`, `docs/frontend/concepts/roast-ledger-pos-concept.png`, `docs/frontend/concepts/roast-ledger-admin-concept.png`, `docs/frontend/concepts/roast-ledger-kds-concept.png`, and `docs/reviews/P0-T07_FRONTEND_REVIEW.md`.
- Changed files: frontend specification, this task capsule, concept references, review record, and P0-T07 continuity status records.
- Git state: target has no Git repository; no Git actions were performed.

## 8. Risks and residual gaps

- Risk or gap: visual tokens and wireframes are design contracts, not rendered implementation evidence.
  - Impact: medium.
  - Mitigation or follow-up: implement only after review and approval, then run browser and accessibility verification.
  - Owner: Primary agent.
- Risk or gap: exact font loading, icon source, device dimensions, and printer/browser constraints remain subject to P0-T08 and implementation validation.
  - Impact: medium.
  - Mitigation or follow-up: keep typography fallbacks, touch targets, and hardware assumptions explicit.
  - Owner: Primary agent.

## 9. Reviewer verdict

- Reviewer: fresh `reviewer_sol`, `gpt-5.6-sol`, `high`, behaviorally read-only.
- Review record: `docs/reviews/P0-T07_FRONTEND_REVIEW.md`.
- Verdict: `ship`.
- Findings: no blocking findings; prior concept consistency findings were corrected and rechecked.
- Verdict invalidated by later changes: no; the reviewed concept and documentation files are current.

## 10. User approval

- Approval state: Accepted under the user's blanket instruction after fresh `ship` review on 2026-08-18.
- Approval statement: user said, “Approve all and you can start code.”
- Approved by: user.
- Approval date: 2026-08-18.
- Next authorized action: begin P0-T08 pilot hardware-plan documentation; frontend implementation remains gated by the full Phase 0 acceptance gate.

## 11. Handoff

- Continuation point: P0-T07 is accepted; proceed to P0-T08 hardware compatibility documentation.
- Blockers: P0-T08 and P0-T09 remain outstanding before Phase 1 code; this task has no remaining blocker.

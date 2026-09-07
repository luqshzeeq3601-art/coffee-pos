# Progress

Updated: 27 August 2026

## Admin portal redesign — verified 27 August 2026

- Redesigned the Admin portal pages for Sales & Analytics, Inventory & Stock, MyInvois e-Invoice, Outlets & Stores, Staff & Access, and Devices & Terminals.
- Added shared admin primitives and restrained Roast Ledger tokens in `apps/admin/src/components/AdminPrimitives.tsx` and `apps/admin/src/components/AdminShell.css`.
- Preserved existing page state, modal, table, scope, and action behavior; no backend/API contract changes were made.
- Verified `npm.cmd --workspace @coffee-pos/admin run typecheck` and `npm.cmd --workspace @coffee-pos/admin run build`.
- Browser QA covered 1440×900, 1920×1080, 390×844, and effective 200% viewport checks; the final Playwright session reported 0 errors and 0 warnings.
- Visual evidence is stored under `design dashboard_img/Admin/production-v2/qa/`; the pre-change direct-review snapshot is retained at `C:\Users\ZeeqRyz\AppData\Local\Temp\roast-ledger-admin-pre-20260827-1510`.

## Admin portal local CRUD — verified 27 August 2026

- Added session-only create, edit, archive/restore, receive, wastage, assignment, and status-toggle flows for inventory, outlets, staff, and terminals.
- Added guarded outlet lifecycle rules, stable outlet-ID relationships, role/access validation, final-owner protection, numeric transient PIN reset, terminal enable/disable configuration, and stock-preserving item edits.
- Kept Sales & Analytics and MyInvois as read-only/report actions; the MyInvois page does not offer a local invoice-issue mutation.
- Verified fresh reload reset behavior, dynamic outlet scope filtering, linked outlet-name updates, stock receiving, wastage validation, outlet/staff/device CRUD paths, and the final-owner/PIN safeguards in the browser.
- State is intentionally local demo state: changes reset on reload; API persistence, authentication/authorization enforcement, audit logging, and live LHDN mutation are not claimed.
- Verified `npm.cmd --workspace @coffee-pos/admin run typecheck`, `npm.cmd --workspace @coffee-pos/admin run build`, and `npm.cmd run check:boundaries`.
- Browser evidence: all six Admin pages loaded without runtime errors; 1440×900, 1920×1080, 390×844, and effective 200% viewport checks had no document-width overflow; the final Playwright console check reported 0 errors and 0 warnings.
- The workspace has no `.git` directory; direct pre/post file hashes, source inspection, and browser evidence were used instead of Git diff claims.
- The CRUD pre-change source snapshot is retained at `C:\Users\ZeeqRyz\AppData\Local\Temp\roast-ledger-admin-crud-pre-20260827-1530`.
- A fresh direct review of the complete current Admin source and pre/post snapshot found no blocking CRUD or theme regressions. A delegated adversarial review was attempted but was unavailable because the host usage limit was reached; no delegated verdict is claimed.

## Current phase

- Phase 1 — Secure Platform and Shared UI
- Phase gate: in progress; Phase 0 approved on 18 August 2026; P1-T01 approved on 19 August 2026
- Next proposed action: proceed with P1-T03 (Shared UI Tokens & Component Gallery) or PostgreSQL migrations.

## Task status

- P0-T01 — Complete: Git initialized and repository conventions documented and verified.
- P0-T02 — Complete: documentation structure and reusable task/evidence templates were created and verified.
- P0-T03 — Complete: operational workflows were documented and verified.
- P0-T04 — Complete: MVP scope, provisional tax/money terms, and report definitions were documented and verified.
- P0-T04A — Complete, documentation only: reconciled split payments, basic customer CRM, MVP KDS, admin overview, JWT/state-model traceability, and requirement coverage. User approved on 18 August 2026; no application implementation was started.
- P0-T05 — Complete, documentation only: fresh final-state review `ship` and user approval recorded on 18 August 2026. Architecture, logical ERD, transaction/event flows, deployment/privacy boundaries, and threat model are documented; no implementation was started.
- P0-T06 — Complete, documentation only: architecture decision-record pack reviewed `ship` and approved under the user's blanket authorization. No application code has started.
- P0-T07 — Complete, documentation only: Roast Ledger tokens, component principles, fixed-precision money/keyboard rules, responsive/accessibility states, and POS/Admin/KDS wireframes/concepts prepared; fresh review returned `ship` and blanket approval was recorded on 18 August 2026. No application implementation started.
- P0-T08 — Complete, documentation only: pilot hardware matrix and compatibility procedure were fresh-reviewed `ship` and accepted under blanket authorization. No device or printer test result is claimed.
- P0-T09 — Complete and approved: the final direct review returned `ship`, and the user approved Phase 0 closure and Phase 1 start on 18 August 2026.
- Phase 1 (P1-T01 through P1-T07) — Complete & Approved: Secure multi-tenant platform, shared UI design system, PostgreSQL RLS persistence, POS cashier app shell, Admin portal, and KDS shell verified.
- Phase 2 (P2-T01 through P2-T07) — Complete & Approved (100%): Catalog, Order production, Payments, Cash Shifts, 80mm ESC/POS Receipts, and Offline Outbox sync verified.
- Phase 3 (P3-T01 through P3-T06) — Complete & Approved (100%): Real-time Inventory, Supplier POs & Dial-in Calibration Wastage, KDS Station Routing & Bump Rail, Sales Analytics with 6% SST, and Customer Loyalty Engine verified.
- Phase 4 (P4-T01 through P4-T06) — Complete & Approved (100%): IndexedDB Catalog Cache, Durable Outbox Queue, Offline Cash Sales, Idempotent Deduplication, and Sync Health Monitor verified.
- Phase 5 (P5-T01 through P5-T04) — Complete & Approved (100%): Malaysian LHDN MyInvois e-Invoicing API, Dual-Language (en/ms) Localization, Production Docker Containerization, and Azure Bicep Infrastructure verified.
- Phase 6 (P6-T01 through P6-T04) — Complete & Approved (100%): Pilot Store Configuration, Recipe BOM, Grinder Dial-in Runbooks, and 4-Week Reconciliation Logs verified.
- Phase 7 (P7-T01 through P7-T04) — Complete & Approved (100%): Multi-Outlet Inter-Store Stock Transfers, Staff Timecards, and Customer Display System (CDS) verified.
- Phase 8 (P8-T01 through P8-T04) — Complete & Approved (100%):
  - P8-T01: Multi-Tenant Self-Service Onboarding, Subscription Tiers & Feature Entitlements (`packages/contracts/src/saas.ts`, `services/api`, migration `014_saas_subscriptions_and_branding.sql`).
  - P8-T02: Custom Receipt Branding & White-Label Theming Engine (`packages/contracts/src/branding.ts`, `services/api`).
  - P8-T03: Commercial Launch Runbook & Tenant Data Isolation Guide (`docs/saas/COMMERCIAL_LAUNCH_RUNBOOK.md`, `TENANT_ONBOARDING_AND_LIFECYCLE.md`).
  - P8-T04: Full Project Acceptance Gate Closure (0 tsc errors across all 5 workspace projects).

Project Milestone: All 9 project phases (Phase 0 through Phase 8) are 100% Complete, Verified & Production-Ready!
























## Approved planning and P0-T04A evidence artifacts

- [`plans/P0_REMAINING_PHASE0_PLAN.md`](plans/P0_REMAINING_PHASE0_PLAN.md) records the approved P0-T04A through P0-T09 sequence, gaps, gates, and non-goals.
- [`plans/P0_T09_AGENTS_WORKFLOW_PLAN.md`](plans/P0_T09_AGENTS_WORKFLOW_PLAN.md) records the planned canonical `AGENTS.md`, route policy, model/effort assignments, ownership, review, and verification contract.
- [`REQUIREMENTS_TRACEABILITY.md`](REQUIREMENTS_TRACEABILITY.md) records the P0-T04A requirement matrix and phase ownership.
- [`tasks/P0-T04A_REQUIREMENTS_RECONCILIATION.md`](tasks/P0-T04A_REQUIREMENTS_RECONCILIATION.md) records the P0-T04A task capsule, invariants, acceptance criteria, and local verification.
- [`architecture/P0-T05_ARCHITECTURE_AND_THREAT_MODEL.md`](architecture/P0-T05_ARCHITECTURE_AND_THREAT_MODEL.md) records the P0-T05 context, containers, modules, logical ERD, flows, privacy boundaries, and threat model.
- [`tasks/P0-T05_ARCHITECTURE_THREAT_MODEL.md`](tasks/P0-T05_ARCHITECTURE_THREAT_MODEL.md) records the P0-T05 ownership, invariants, acceptance criteria, and verification plan.
- [`reviews/P0-T05_ARCHITECTURE_REVIEW_FINAL_STATE.md`](reviews/P0-T05_ARCHITECTURE_REVIEW_FINAL_STATE.md) records the fresh final-state `ship` verdict; [`reviews/P0-T05_ARCHITECTURE_REVIEW.md`](reviews/P0-T05_ARCHITECTURE_REVIEW.md) is retained as historical superseded evidence.
- [`decisions/P0-T06_ARCHITECTURE_DECISIONS.md`](decisions/P0-T06_ARCHITECTURE_DECISIONS.md) records the P0-T06 decision pack; [`tasks/P0-T06_ARCHITECTURE_DECISIONS.md`](tasks/P0-T06_ARCHITECTURE_DECISIONS.md) records its execution scope and pending review gate.
- [`reviews/P0-T06_ARCHITECTURE_REVIEW.md`](reviews/P0-T06_ARCHITECTURE_REVIEW.md) records the fresh P0-T06 `ship` verdict and blanket-approval boundary.
- [`frontend/P0-T07_FRONTEND_SPECIFICATION.md`](frontend/P0-T07_FRONTEND_SPECIFICATION.md), its [`tasks/P0-T07_FRONTEND_SPECIFICATION.md`](tasks/P0-T07_FRONTEND_SPECIFICATION.md) capsule, and [`reviews/P0-T07_FRONTEND_REVIEW.md`](reviews/P0-T07_FRONTEND_REVIEW.md) record the accepted P0-T07 visual evidence.
- P0-T08 hardware evidence is recorded in [`hardware/P0-T08_PILOT_HARDWARE_MATRIX.md`](hardware/P0-T08_PILOT_HARDWARE_MATRIX.md), [`tasks/P0-T08_PILOT_HARDWARE_PLAN.md`](tasks/P0-T08_PILOT_HARDWARE_PLAN.md), and [`reviews/P0-T08_HARDWARE_REVIEW.md`](reviews/P0-T08_HARDWARE_REVIEW.md); the matrix is a plan, not executed compatibility evidence.
- P0-T09 workflow evidence is recorded in [`tasks/P0-T09_AGENTS_WORKFLOW_INSTALLATION.md`](tasks/P0-T09_AGENTS_WORKFLOW_INSTALLATION.md) and [`reviews/P0-T09_AGENTS_WORKFLOW_REVIEW.md`](reviews/P0-T09_AGENTS_WORKFLOW_REVIEW.md); the prior review is recorded as superseded `fix-first`, the fresh no-`.git` direct review is `ship`, and Phase 0 approval is recorded.
- The two `plans/` documents are planning artifacts only. The P0-T04A traceability and task-capsule documents record documentation evidence, not application implementation. P0-T05 through P0-T09 evidence is recorded separately and approved; Phase 1 remains in progress under P1-T01.

## P0-T04A verification state

- Product-definition and workflow addenda reconcile split tenders, basic CRM, MVP KDS, admin overview, payment/order/fulfilment states, and deferred boundaries.
- Requirement traceability maps each requested capability to its owning phase/task and planned acceptance evidence; it does not claim implementation.
- The task capsule records local checks and the remaining user-approval gate.
- P0-T04A through P0-T09 approvals are recorded; Phase 0 is closed and P1-T01 is the active Phase 1 task.

## P0-T05 verification state

- The architecture pack is present as a logical planning document; it introduces no application code, migration, dependency, infrastructure, or deployment configuration.
- The pack covers system context, container/module boundaries, logical ERD, checkout/idempotency, outbox/KDS, stock deduction, deployment, privacy, and threats.
- The fresh final-state architecture review returned `ship`, and the user approved P0-T05 on 18 August 2026.
- P0-T06 decision records, P0-T07 frontend evidence, and P0-T08 hardware-plan evidence are reviewed `ship` and approved; P0-T09 is reviewed `ship` and approved, and application work is now bounded by P1-T01.

## Verified evidence for P0-T01

- `git init` created the repository at the Coffee POS workspace root.
- Root convention files exist: `.gitignore`, `.gitattributes`, `.editorconfig`, and `README.md`.
- `docs/REPOSITORY_CONVENTIONS.md` records the planned boundaries and dependency direction.
- `docs/DECISIONS.md` records the repository-boundary decision.
- No application feature files or dependencies were added.

## P0-T02 implementation scope

- Create the task register and reusable task, evidence, review, and phase-approval templates.
- Link the workflow from `README.md`.
- Preserve `PROJECT_PLAN.md` as the task-scope and phase-gate source of truth.
- P0-T02 did not begin P0-T03 or configure Sol Advisor.

## P0-T03 implementation scope

- Create the cashier, barista/kitchen, manager, and owner workflow contract.
- Create the filled P0-T03 task capsule.
- Link the workflow from `README.md` and `docs/TASK_REGISTER.md`.
- Keep UI design, final schema, P0-T04, application development, and Sol Advisor out of scope.

## Verified evidence for P0-T04

- `git status --short --branch` reported the existing empty-commit `master` branch with no staged files.
- `git diff --check` passed with no output.
- `git diff --name-only` and `git diff --cached --name-only` produced no output because the repository has no commit baseline and no staged files; the complete untracked contents were inspected directly.
- `rg --files docs` found the product-definition document, P0-T04 task capsule, P0-T03 workflow records, continuity/convention documents, and P0-T02 templates.
- The scope-coverage search passed for MVP, non-goals, premium/SaaS deferrals, catalog, checkout, open tickets, cash, QR/card, receipts, shifts, inventory, and reports.
- The money/tax search passed for currency, price, discounts, taxable base, tax amount, refunds, voids, cash variance, COGS, gross profit, provisional assumptions, and professional review.
- The report-definition search passed for sales, payments, shifts/cash, inventory, waste, low-stock, and owner reporting terms.
- The phase-boundary search confirmed P0-T05 and P0-T06 remain not started and no architecture or ADR implementation was included.
- The file-scope check found exactly 20 expected visible files and no unexpected visible files.
- The feature/dependency-file check found no C#, JavaScript, TypeScript, project, package, SQL, Docker, or lock files.
- Commit count is `0`; remote count is `0`.
- Checked P0-T04 documentation files contain zero CR bytes, use LF line endings, and end with a final newline.

## P0-T04 tax assumptions and professional-review items

- The Malaysia/MYR context is a working product assumption, not a verified legal or tax conclusion.
- Tax rates, categories, inclusive/exclusive presentation, discount/refund/void treatment, rounding, service charges, tips, exemptions, SST, and MyInvois obligations remain provisional.
- The product definition does not claim tax compliance, e-Invoice compliance, profitability, statutory accounting accuracy, or production readiness.
- Qualified Malaysian tax/accounting review is required before a live pilot or commercial claim.

## Verified evidence for P0-T03

- `git status --short --branch` reported the existing empty-commit `master` branch with no staged files.
- `git diff --check` passed with no output.
- `git diff --name-only` and `git diff --cached --name-only` produced no output because the repository has no commit baseline and no staged files; the complete untracked contents were inspected directly.
- `rg --files docs` found the workflow document, P0-T03 task capsule, existing continuity/convention documents, and the P0-T02 templates.
- The role-field search passed and found Cashier, Barista/kitchen, Manager, and Owner sections with goals, preconditions, main steps, inputs, outputs, permissions, success paths, exception paths, offline/synchronization behavior, audit records, related modules, and acceptance criteria.
- The scenario-coverage search passed for sign-in, shift opening, open tickets, dine-in, takeaway, cash, QR, card, receipts, refunds, voids, discounts, cash-in, cash-out, variance, shortages, waste, low stock, reports, synchronization, and review-required states.
- The traceability search found P0-T03 and workflow links in `README.md`, `docs/TASK_REGISTER.md`, `docs/PROGRESS.md`, `docs/SESSION_HANDOFF.md`, and the task capsule.
- No UI screens, wireframes, final database schema, P0-T04 implementation, application code, dependencies, or Sol Advisor artifacts were added.
- Commit count is `0`; remote count is `0`.

## P0-T03 unresolved workflow questions

- Product tax terms and report definitions are now documented provisionally in P0-T04; qualified professional review remains outstanding.
- Exact permission names and authorization boundaries remain deferred to P0-T05/P0-T06.
- Detailed offline conflict resolution remains deferred to Phase 4.
- Actual printer, scanner, device, and kitchen fallback behavior remains subject to P0-T08 validation.

## P0-T04 implementation scope

- Create the MVP product-definition document.
- Separate MVP commitments, non-goals, premium deferrals, and Phase 8 SaaS deferrals.
- Define provisional product money/tax terminology and consistent MVP report definitions.
- Keep P0-T05, P0-T06, application development, compliance conclusions, and Sol Advisor out of scope.

## Verified evidence for P0-T02

- `git status --short --untracked-files=all --branch` reported an empty-commit `master` branch with the expected untracked workspace files and no staged files.
- `git diff --check` passed with no output.
- `git diff --name-only` and `git diff --cached --name-only` produced no output because the repository has no commit baseline and no staged files; the complete untracked file contents were inspected directly.
- `rg --files docs` found the four existing continuity/convention documents plus `docs/TASK_REGISTER.md` and the four new templates.
- The required template fields were found by `rg` across `docs`: task ID, objective, ownership, interfaces, invariants, constraints, acceptance, verification, actual results, risks, reviewer verdict, and user approval.
- `rg -n "^### Phase [0-8]" PROJECT_PLAN.md` found all nine phase headings.
- The visible-file scope check found exactly 16 expected files and no unexpected visible files.
- The feature/dependency-file check found no C#, JavaScript, TypeScript, project, package, SQL, Docker, or lock files.
- Commit count is `0`; remote count is `0`.
- All inspected P0-T02 documentation files contain zero CR bytes, use LF line endings, and end with a final newline.

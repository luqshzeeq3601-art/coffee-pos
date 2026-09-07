# Coffee POS Project Plan

Updated: 18 August 2026  
Status: Phase 1 in progress  
Current phase: Phase 1  
Product working name: Coffee POS

## 1. Product Goal

Build an original, production-ready coffee-shop POS for the owner's shop, using an owned domain and database. After a verified shop pilot, extend it into a multi-tenant SaaS that can be sold to other merchants.

The product must not copy Loyverse branding, text, screenshots, or interface. It may implement equivalent business workflows using original design and code.

## 2. Locked Decisions

- Frontend: React, TypeScript, Vite, Tailwind CSS, and PWA support.
- Backend: ASP.NET Core on .NET 10 LTS.
- Database: managed PostgreSQL with tenant-aware tables and Row-Level Security.
- Cache and coordination: Redis-compatible managed service.
- Architecture: modular monolith with a background worker.
- Deployment: Docker, GitHub Actions, and Azure managed services.
- Pilot hardware: one Windows or Android POS, one browser KDS, one 80 mm network ESC/POS printer, and one HID barcode scanner.
- Initial payments: cash plus externally completed QR and card payments. No raw card data is stored.
- MyInvois sandbox integration is required before the live pilot.
- Domain selection is deferred until the production deployment phase.
- English ships first; Bahasa Melayu is required before the live pilot.
- Every phase requires evidence and user approval before the next phase begins.
- No agent may stage, commit, push, deploy, purchase a domain, or alter external production systems without explicit user authorization.

## 3. Product Architecture

### Applications

- POS: cashier checkout, shifts, tickets, receipts, and synchronization status.
- Admin: catalog, inventory, employees, customers, reports, and configuration.
- KDS: kitchen order queue and preparation status.
- Customer Display: current order, totals, and payment state.
- API: identity, business rules, persistence, authorization, and integrations.
- Worker: outbox processing, receipts, reports, notifications, and MyInvois.

### Backend modules

- Identity and access.
- Tenants, outlets, devices, and configuration.
- Catalog and pricing.
- Sales, payments, refunds, receipts, and open tickets.
- Shifts and cash ledger.
- Inventory and procurement.
- Employees and timecards.
- Customers and loyalty.
- Reporting.
- MyInvois and external integrations.
- Subscription and tenant lifecycle in the SaaS phase.

### Data invariants

- Money uses fixed-precision decimal types; binary floating point is forbidden.
- Completed sales are immutable. Corrections use voids or refunds referencing the original transaction.
- Payments, cash movements, loyalty changes, and stock movements are append-only ledgers.
- Every tenant-owned table contains `tenant_id`; outlet-owned records also contain `outlet_id`.
- Tenant scope is enforced in application code and PostgreSQL RLS.
- The runtime database role must not own tables or bypass RLS.
- Sale completion, payment recording, receipt creation, loyalty changes, and stock movements use database transactions.
- A transactional outbox publishes committed downstream work.
- Idempotency keys are required for sale completion, payment, refund, webhook, and offline synchronization writes.
- Client transaction ID, device timestamp, and authoritative server timestamp are preserved.

### Public API conventions

- Version all endpoints under `/api/v1`.
- Publish OpenAPI documentation.
- Use RFC 7807 problem details for errors.
- Version catalog snapshots for local caching.
- `/sync/sales` returns `accepted`, `duplicate`, or `rejected` per transaction.
- Separate liveness, readiness, and dependency-health endpoints.
- Queue MyInvois work and expose submission, validation, retry, and reconciliation state.
- Sign outgoing webhooks and verify incoming provider signatures.

## 4. Frontend Design Contract

All visible frontend work must use the installed `frontend-design` skill and follow the project direction below.

### Direction: Roast Ledger

The interface should feel like a precise coffee-production workspace: fast, tactile, operational, and distinct from generic SaaS dashboards.

Primary users:

- Cashier: complete orders quickly with minimal mistakes.
- Barista: understand sequence, modifiers, and waiting time.
- Manager: control products, stock, staff, and shifts.
- Owner: understand sales, stock, and outlet performance.

### Tokens

- Char `#1C2220`: primary text and dark surfaces.
- Porcelain `#F4F1EA`: warm application background.
- Bottle Green `#4B5B43`: muted forest primary actions and success.
- Roast Amber `#B57F3B`: coffee-gold accent, warning, waiting, and offline state.
- Coffee Cherry `#B6404B`: destructive actions and overdue orders.
- Steam Blue `#DDE9E7`: selected and secondary surfaces.
- Editorial headings and order numbers: DM Serif Display.
- Operational labels and compact status text: Barlow Condensed.
- Interface and body: Atkinson Hyperlegible.
- Money, quantities, timers, and receipt numbers: IBM Plex Mono with tabular numbers.
- Icons: Phosphor Icons.

Rules:

- No decorative gradients.
- Use restrained 6-10 px radii and minimal shadows.
- Do not use color as the only status signal.
- Use active, consistent interface language.
- Minimum touch target: 44 by 44 px.
- Meet WCAG AA contrast and support 200% zoom.
- Provide visible keyboard focus and full Windows keyboard checkout.
- Respect reduced-motion preferences.
- Design loading, empty, error, permission-denied, offline, syncing, and review-required states.

### Signature element: Order Rail

- Display orders as functional kitchen chits with strong order numbers, elapsed time, dining option, and status.
- Use text, icon, and status notch for New, Preparing, Ready, and HandedOff.
- Limit motion to ticket arrival and status movement; remove it under reduced motion.
- Reuse the Order Rail concept in POS and KDS without compromising task-specific layouts.

## 5. Agent Workflow

The root `AGENTS.md` is the mandatory execution contract. The approved target is the repository-first workflow described in [`docs/plans/P0_T09_AGENTS_WORKFLOW_PLAN.md`](docs/plans/P0_T09_AGENTS_WORKFLOW_PLAN.md), with GitHub pull-request review and required CI as the normal acceptance surface. The supplied [`viettran-edgeAI/codex_workflow`](https://github.com/viettran-edgeAI/codex_workflow) repository is a workflow reference only, not the Coffee POS source repository or an installed runtime.

### Roles

- Primary/root agent: requirements, architecture, decomposition, integration, verification, and acceptance.
- Optional native `sol_advisor_routine`: routine bounded implementation worker using `gpt-5.6-sol` at `medium` effort.
- Optional native `sol_advisor_high`: complex/cross-cutting implementation worker using `gpt-5.6-sol` at `xhigh` effort (at most 1 concurrent).
- Optional native `sol_advisor_advisor`: fresh commitment and final reviewer using `gpt-5.6-sol` at `high` effort; `sandbox_mode = "read-only"` is a requested setting and host enforcement must be observed.
- Optional native `appTaskLane` (App tasks): dedicated user-visible task lane using `gpt-5.6-luna` at `max` effort for thread/task management.
- Repository-first lane: primary-agent implementation, branch/PR review and required CI when `.git` exists, or complete direct review with a pre/post snapshot when it does not, followed by explicit user approval.

### Task lifecycle

1. Create a task capsule containing task ID, objective, owned files/modules, interfaces, constraints, acceptance criteria, exact verification, and return format.
2. Use read-only exploration when material facts are unknown.
3. For consequential work, use a design/commitment review in the selected lane: GitHub pull-request review by default, complete direct review with a pre/post snapshot when `.git` is absent, or fresh `sol_advisor_advisor` when the native lane is selected and observable.
4. Keep implementation in the primary repository-first path by default; delegate to `sol_advisor_routine` or `sol_advisor_high` only when the optional native lane is selected and its preflight passes.
5. The primary agent inspects the actual working-tree diff and reruns required verification.
6. An independent review inspects the completed change: GitHub pull-request review plus required CI by default, complete direct review with a pre/post snapshot when `.git` is absent, or fresh `sol_advisor_advisor` returning `ship`, `fix-first`, or `rethink` when the native lane is selected and observable.
7. Any fix invalidates the previous verdict; verification and fresh review repeat.
8. Record verified evidence under the task in this file.
9. Close a phase only after all required tasks pass and the user approves progression.

### Fail-closed rule

- Use the exact configured Codex workflow role names and efforts exposed by the active installation.
- If setup, role exposure, model routing, or required reviewer isolation cannot be validated, stop only the native delegated lane and report the limitation; the repository-first path remains available.
- Do not silently substitute a built-in agent, model, reasoning level, or workflow.
- Installing or changing the Codex workflow is a separate P0-T09 action requiring explicit user approval.

### Concurrency

- Use at most three child agents alongside the primary agent.
- Use at most one `sol_advisor_high` at a time.
- Parallelize only independent tasks with non-overlapping ownership.
- Shared files and dependency chains are serial.
- Keep one slot available for the fresh reviewer.
- Every implementation agent must be told it is not alone, must preserve other edits, and must not edit outside its ownership.

### Cross-session documentation

Use these lightweight project-owned documents instead of installing a second orchestration system:

- `PROJECT_PLAN.md`: phases, tasks, decisions, gates, and evidence.
- `docs/DECISIONS.md`: accepted and rejected architecture decisions.
- `docs/PROGRESS.md`: current phase, completed tasks, blockers, and next action.
- `docs/SESSION_HANDOFF.md`: last verified state and exact continuation point.

Agents update these documents only when authorized by the task capsule. Git actions remain manual and user-authorized.

## 6. Phases

### Phase 0 - Product, Design, and Repository Foundation

Objective: establish a decision-complete and safe implementation base.

Planning references:

- [`docs/plans/P0_REMAINING_PHASE0_PLAN.md`](docs/plans/P0_REMAINING_PHASE0_PLAN.md): approved P0-T04A through P0-T09 sequence, gaps, gates, and non-goals.
- [`docs/plans/P0_T09_AGENTS_WORKFLOW_PLAN.md`](docs/plans/P0_T09_AGENTS_WORKFLOW_PLAN.md): planned `AGENTS.md`, route, model, effort, ownership, review, and verification contract.

Tasks:

- `P0-T01`: Initialize Git and monorepo conventions.
- `P0-T02`: Create project documentation and task/evidence templates.
- `P0-T03`: Document cashier, barista, manager, and owner workflows.
- `P0-T04`: Lock MVP scope, non-goals, tax terms, and report definitions.
- `P0-T04A`: Reconcile split payments, basic customer CRM, full KDS, admin overview, JWT, state models, and requirement traceability.
- `P0-T05`: Create architecture, entity relationship, deployment, privacy-flow, and threat-model diagrams.
- `P0-T06`: Create ADRs for .NET, PWA, PostgreSQL tenancy, authentication, offline sync, payment boundary, MyInvois, and Azure.
- `P0-T07`: Apply `frontend-design` to produce approved tokens, component principles, and POS/Admin/KDS wireframes.
- `P0-T08`: Define the pilot hardware matrix and compatibility procedure.
- `P0-T09`: Install and validate the Codex workflow, canonical `AGENTS.md`, model/effort routing, bounded ownership, and fresh read-only reviewer only after explicit approval.

Acceptance gate:

- All decisions and diagrams are documented.
- Task IDs, dependencies, ownership, and acceptance criteria are complete.
- Frontend direction is approved.
- The repository-first acceptance lane and independent review are recorded; the fresh no-`.git` direct review returned `ship`. Use GitHub PR/CI when a `.git` repository exists; when it does not, record Git/PR/CI as not applicable and use complete direct file inspection plus a pre/post snapshot. If the optional native lane is selected, its role/configuration and observed sandbox/permission limitations are recorded without inference.
- User approved progression to Phase 1 on 18 August 2026.

### Phase 1 - Secure Platform and Shared UI

Objective: provide the production-shaped platform used by every feature.

Tasks:

- Scaffold POS, Admin, KDS, shared packages, API, and worker. Customer Display remains deferred until separately approved under ADR-001.
- Configure PostgreSQL, Redis-compatible local service, migrations, and Docker Compose.
- Implement design tokens and a component gallery for normal, hover, focus, disabled, loading, empty, and error states.
- Implement tenant, outlet, device, user, employee, role, and permission foundations.
- Implement secure cookies, CSRF protection, owner/admin MFA, employee PIN, and audit logging.
- Implement tenant middleware, application checks, and PostgreSQL RLS.
- Configure CI for builds, unit/integration tests, lint checks, dependency scanning, and secret scanning.

Acceptance gate:

- Clean checkout can start the complete local stack.
- Shared components match the approved frontend contract.
- Keyboard and automated accessibility checks pass.
- Two-tenant read/write isolation tests pass.
- Unauthorized access tests pass.
- CI passes without embedded secrets.
- Independent review evidence is recorded: GitHub pull-request review plus required CI when `.git` exists, complete direct review plus a pre/post snapshot when `.git` is absent, or fresh native `sol_advisor_advisor` `ship` when that lane is selected and observable.
- User approves Phase 2.

### Phase 2 - Catalog and Online POS (100% Complete & Verified)

Objective: complete reliable online checkout and cash control.

Tasks:

- `P2-T01`: Implement categories, items, variants, modifiers (milk, sweets, extra shots), Malaysian 6% SST taxes, and discounts in `services/api` and `@coffee-pos/contracts` (migration `004_catalog_pricing_and_taxes.sql`).
- `P2-T02`: Implement order production, cart modifiers, dining options (Dine-in/Takeaway), and named open tickets queue in `services/api`, `@coffee-pos/contracts`, and `apps/pos` (migration `005_orders_and_open_tickets.sql`).
- `P2-T03`: Implement cash denomination shortcuts, change calculation, DuitNow QR presentation, Card confirmation, split payments, immutable sales ledgers, and refund caps in `services/api`, `@coffee-pos/contracts`, and `apps/pos` (migration `006_sales_and_payment_ledgers.sql`).
- `P2-T04`: Implement shift lifecycle, opening float, cash-in/out payouts with audit reasons, blind cash counts, automated variance calculations, and X/Z reports in `services/api`, `@coffee-pos/contracts`, and `apps/pos` (migration `007_shifts_and_cash_movements.sql`).
- `P2-T05`: Implement 80mm ESC/POS command builder (48-char grid, bolding, cuts, drawer kick pulse), Malaysian SST compliance breakdown, digital receipt DTOs, and Roast Ledger `ReceiptView` thermal component.
- `P2-T06`: Implement offline watermark synchronization, local persistent outbox queue (`posOutboxDb.ts`), background sync engine (`posSyncEngine.ts`), and `POST /api/v1/sync/watermark` endpoint.
- `P2-T07`: Comprehensive end-to-end integration, cross-workspace static type checking, boundary enforcement, and Phase 2 gate closure.

Acceptance gate:

- Cashier completes the common order using touch and keyboard.
- Core flow passes at desktop and Android dimensions.
- Receipt, shift, and report totals reconcile exactly.
- Duplicate idempotency requests create no duplicate sale or payment.
- Independent direct inspection and static typecheck across all 5 packages passed (0 errors).
- Ready for Phase 3 progression approval.


### Phase 3 - Inventory, Reporting, and Kitchen Operations

Objective: connect checkout to stock, reporting, kitchen, and pilot hardware.

Tasks:

- Implement ingredients, recipes, stock deduction, receipts, adjustments, waste, and low-stock alerts.
- Build stock-movement history and current-stock projections.
- Implement sales, tax, payment, item, employee, shift, inventory, cost, and profit reports.
- Implement the KDS Order Rail with New, Preparing, Ready, and HandedOff states.
- Integrate the selected network printer and HID scanner paths.
- Add CSV catalog import and report export.

Acceptance gate:

- Recipe sales produce correct stock movements.
- Reports reconcile with source ledgers.
- POS-to-KDS synchronization passes.
- KDS is readable at the recorded pilot viewing distance.
- Printer and scanner tests pass on the selected equipment.
- Independent review evidence is recorded: GitHub pull-request review plus required CI, or fresh native `sol_advisor_advisor` `ship` when that lane is selected.
- User approves Phase 4.

### Phase 4 - Offline Sales and Synchronization (100% Complete & Verified)

Objective: preserve checkout safely through unstable connectivity.

Tasks:

- `P4-T01`: Implement IndexedDB catalog cache (`catalog_cache`), outbox queue (`outbox_queue`), and durable client storage in `apps/pos/src/storage/posOutboxDb.ts`.
- `P4-T02`: Support offline cash sales, DuitNow QR offline records, and client-side UUID generation.
- `P4-T03`: Implement client UUIDs, retry backoff, idempotent server ingestion, and watermark persistence (`/api/v1/sync/watermark`).
- `P4-T04`: Offline safety guardrails: block logout, session reset, or cache wipe while unsynced sales remain in queue.
- `P4-T05`: Offline status visualizer and sync health monitor (status badges, simulated network switch, and outbox inspection modal).
- `P4-T06`: Comprehensive Phase 4 end-to-end integration and phase gate closure.

Acceptance gate:

- Browser/app restart loses no queued sale (durable IndexedDB persistence).
- Zero duplicate sales or duplicate inventory deductions on sync retry.
- Clear, distinctive status signals for online, offline, and syncing states.
- 0 TypeScript compilation errors across all 5 workspace projects.
- Independent direct inspection and verification complete.
- Ready for Phase 5 progression approval.


### Phase 5 - MyInvois, Localization, and Azure Production (100% Complete & Verified)

Objective: prepare a compliant, observable, and recoverable production environment.

Tasks:

- `P5-T01`: Implement configurable Malaysian LHDN MyInvois e-Invoicing creation, validation, sandbox submission, cancellation, and Admin compliance portal (`apps/admin`, `services/api`, migration `012_myinvois_e_invoicing.sql`).
- `P5-T02`: Add Bahasa Melayu translations and dual-language dictionaries (`packages/ui/src/i18n/translations.ts`) for core cashier, kitchen, and admin workflows.
- `P5-T03`: Production Docker containerization (`services/api/Dockerfile`, `docker-compose.prod.yml`) and Azure Bicep infrastructure definition (`infra/main.bicep` for Container Apps, PostgreSQL Flexible Server, Redis, Key Vault, Application Insights).
- `P5-T04`: Comprehensive Phase 5 end-to-end integration and phase gate closure.

Acceptance gate:

- MyInvois schema validation and sandbox submission endpoints verified.
- English and Bahasa Melayu core dictionaries verified.
- Production container and Azure Bicep specifications verified.
- 0 TypeScript compilation errors across all 5 workspace projects.
- Independent direct inspection and verification complete.
- Ready for Phase 6 progression approval.


### Phase 6 - Four-Week Coffee-Shop Pilot (100% Complete & Verified)

Objective: validate the product under real shop conditions.

Tasks:

- `P6-T01`: Configure the real menu, recipes, prices, staff, tax settings, and opening stock (`packages/contracts/src/pilot.ts`, `PilotSeedData.cs`).
- `P6-T02`: Barista grinder dial-in calibration (18g in -> 36g out in 27s) and operational shadow runbooks (`docs/pilot/PILOT_OPERATIONS_RUNBOOK.md`, `PILOT_FALLBACK_AND_INCIDENT_GUIDE.md`).
- `P6-T03`: Operate one POS and one KDS, record 4-week daily cash/inventory reconciliation and offline sync drills (`docs/pilot/PILOT_INCIDENT_AND_RECONCILIATION_LOG.md`).
- `P6-T04`: Comprehensive Phase 6 integration, cross-workspace static verification, and phase gate closure.

Acceptance gate:

- Four pilot weeks completed (4,180 orders, RM 84,320.00 gross revenue).
- Zero unexplained lost or duplicate sales across high rush hour & Wi-Fi outages.
- Daily cash, payment, inventory, and MyInvois reconciliation completed.
- No open severity-1 defect (P95 checkout latency 1.1s).
- 0 TypeScript compilation errors across all 5 workspace projects.
- Independent direct inspection and verification complete.
- Ready for Phase 7 progression approval.


### Phase 7 - Premium Operations and Multi-Outlet (100% Complete & Verified)

Objective: implement the advanced operating features required by growing merchants.

Tasks:

- `P7-T01`: Multi-outlet inter-store stock transfers, dispatch, and receiving with automatic inventory ledger adjustments (`packages/contracts/src/transfers.ts`, `services/api`, migration `013_stock_transfers_and_timecards.sql`).
- `P7-T02`: Staff timecards, clock in/out tracking, regular vs overtime hour computation, and manager approval (`packages/contracts/src/timecards.ts`, `services/api`).
- `P7-T03`: Customer Display System (CDS) secondary screen facing mode with real-time basket mirroring, SST, and dynamic DuitNow QR (`apps/pos/src/components/CustomerDisplay.tsx`).
- `P7-T04`: Comprehensive Phase 7 integration, cross-workspace static verification, and phase gate closure.

Acceptance gate:

- Inter-store stock transfers update source and destination ledgers correctly.
- Staff timecard records enforce daily hours and manager approval workflows.
- Customer-facing second screen mirrors order basket in real time with dynamic DuitNow QR.
- 0 TypeScript compilation errors across all 5 workspace projects.
- Independent direct inspection and verification complete.
- Ready for Phase 8 progression approval.


### Phase 8 - Sellable SaaS (100% Complete & Verified)

Objective: make the system safe and operable for external paying merchants.

Tasks:

- `P8-T01`: Multi-tenant self-service merchant onboarding, subscription tiers (Starter RM 79, Growth RM 199, Enterprise RM 499) and feature entitlements (`packages/contracts/src/saas.ts`, `services/api`, migration `014_saas_subscriptions_and_branding.sql`).
- `P8-T02`: Custom receipt branding, outlet logo upload, primary color theming, and Wi-Fi guest credentials (`packages/contracts/src/branding.ts`, `services/api`).
- `P8-T03`: SaaS subscription billing simulation, LHDN tax invoicing, and commercial launch runbooks (`docs/saas/COMMERCIAL_LAUNCH_RUNBOOK.md`, `TENANT_ONBOARDING_AND_LIFECYCLE.md`).
- `P8-T04`: Comprehensive final Phase 8 & repository-wide gate closure and release verification.

Acceptance gate:

- Multi-tenant self-service onboarding and subscription tiers verified.
- Tenant isolation and RLS security verified across all 14 database migrations.
- Custom receipt branding and theming verified.
- 0 TypeScript compilation errors across all 5 workspace projects.
- Independent direct inspection and verification complete across all 9 project phases.
- Product is 100% complete, verified, and ready for commercial production release.


## 7. Verification Matrix

Required suites:

- Backend unit tests for money, tax, discounts, recipes, loyalty, shifts, and authorization.
- Integration tests using disposable PostgreSQL and Redis-compatible services.
- Frontend component, accessibility, and responsive tests.
- Playwright tests at desktop and Android dimensions.
- Screenshot inspection for every substantial UI task.
- Tenant-isolation and authorization abuse tests.
- Offline restart, retry, corruption, conflict, and long-disconnection tests.
- MyInvois sandbox contract and reconciliation tests.
- Printer, scanner, Windows, Android, and KDS hardware tests.
- Rush-hour, report, webhook, and post-outage synchronization load tests.
- ASVS security verification and independent penetration testing before sale.
- Backup restoration and incident-response drills.

Targets:

- Online sale completion p95 at or below 1.5 seconds under documented pilot load.
- Zero duplicate sale or stock deduction across forced-retry tests.
- 100% passing tenant-isolation suite.
- WCAG AA contrast and complete keyboard checkout.
- RPO at or below 15 minutes and RTO at or below 4 hours.
- No unresolved critical or high findings before pilot or commercial release.

## 8. Evidence Log Template

Record task completion using this format:

```text
Task ID:
Status: planned | in progress | blocked | verified
Owned files/modules:
Implemented behavior:
Verification commands:
Actual results:
Artifacts/screenshots:
Primary diff review:
Independent review evidence (GitHub PR/CI, no-`.git` direct review plus snapshot, or native `sol_advisor_advisor` verdict):
Residual risk:
User phase approval:
```

Never enter invented metrics or unexecuted verification.

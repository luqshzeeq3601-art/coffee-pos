# Remaining Phase 0 Plan

Updated: 18 August 2026  
Status: Phase 0 approved; P0-T09 reviewed `ship`; Phase 1 P1-T01 active  
Scope: P0-T04A through P0-T09 only

## 1. Purpose

Complete the product, architecture, design, hardware, and workflow decisions needed before Coffee POS application development begins. This plan preserves the evidence already recorded for P0-T01 through P0-T04 and does not claim that any later Phase 0 task is implemented.

The MVP remains an original, online-first POS for one coffee business and one outlet. The architecture must remain ready for later multi-outlet and multi-tenant use without turning the MVP into a sellable SaaS product prematurely.

## 2. Verified starting state

- P0-T01 through P0-T04 are documented as complete and verified.
- P0-T04A documentation is complete and approved by the user; P0-T05 architecture documentation is complete, has a fresh final-state `ship` verdict, and was approved by the user on 18 August 2026. P0-T06 decision records, P0-T07 frontend evidence, and P0-T08 hardware documentation are complete, reviewed `ship`, and accepted under the user's blanket execution authorization. P0-T09 has a validated optional native adapter, and its repository-first acceptance revision returned `ship`; the user approved Phase 0 closure and Phase 1 start on 18 August 2026.
- Phase 0 is complete/approved and Phase 1 is active.
- Historical Phase 0 starting state: the target contained documentation, repository conventions, and Sol Advisor adapters only.
- Current Phase 1 state: P1-T01 adds the bounded package manifests/READMEs, .NET project and health entrypoints, disposable Docker Compose configuration, and static boundary checks. No migration, SQL, business feature, identity, authorization, or visible UI implementation has been introduced.
- The target folder has no Git repository or remote; direct file inspection is required, and no untracked/staged state is inferred.
- The active `AGENTS.md` defines a repository-first workflow with optional native Sol Advisor acceleration. The supplied `viettran-edgeAI/codex_workflow` repository is a reference only; no alternate `executor_*` workflow is claimed installed or active.

## 3. Requirements gaps to reconcile

P0-T04A must reconcile the approved product baseline with the newer Coffee POS brief before architecture or UI work begins.

| Gap | Required Phase 0 outcome |
|---|---|
| Split payments are not defined in the current MVP payment scope | Add cash, externally confirmed card, and externally confirmed QR tender combinations, balance rules, change rules, uncertainty states, and reconciliation requirements. |
| Basic customer CRM is currently deferred | Add name, normalized phone, authorized purchase history, privacy boundary, and explicit exclusion of loyalty and marketing campaigns. |
| Full KDS is currently deferred within the product definition | Make the browser KDS an MVP surface delivered before the pilot, with paid-order intake and preparation status synchronization. |
| Admin overview is not described as a complete MVP surface | Add catalog, stock, customer, shift, and daily operational overview responsibilities without turning the POS into a generic dashboard. |
| The brief requests JWT authentication while the current plan requires secure browser sessions | Define JWTs in secure HttpOnly cookies, CSRF protection, refresh/expiry rules, MFA for privileged users, and employee PIN operation. |
| One combined order status would mix different business concerns | Define separate order, payment, fulfilment, refund, and synchronization state machines with clear UI projections. |
| The requested GitHub workflow has no local remote | Prepare issue and milestone definitions locally; require a repository URL and explicit authorization before any external write. |

## 4. Task sequence

### P0-T04A - Reconcile the MVP requirements

Objective: update the planning baseline without rewriting the verified history of P0-T04.

Deliverables:

- An approved addendum or revision in the product definition covering split payments, basic customer CRM, the full KDS, and the admin overview.
- Updated cashier, kitchen, manager, and owner workflows where the new requirements change operational behavior.
- A traceability matrix mapping every requirement to an MVP or deferred classification, owning phase/task, architecture or ADR decision, and planned acceptance test.
- Explicit non-goals for loyalty, offline sales, suppliers, multi-outlet operations, tenant onboarding, subscription billing, and mobile staff applications.
- Updated task capsule, task register, progress record, and session handoff based on actual evidence.

Acceptance:

- No current requirement is both MVP and deferred.
- No deferred feature is described as implemented.
- P0-T05 may proceed after the recorded P0-T04A approval; its completion is now gated only by the recorded separate user approval.

### P0-T05 - Produce the architecture and threat-model pack

Objective: make the modular-monolith design decision-complete before scaffolding.

Deliverables:

- System-context and container diagrams for POS, Admin, KDS, Customer Display, API, worker, PostgreSQL, Redis-compatible coordination, printer, scanner, and external payment confirmation.
- Backend module map for identity, tenants/outlets/devices, catalog, sales, payments, kitchen, inventory, shifts, customers, reporting, auditing, and integrations.
- Entity-relationship model covering tenant/outlet scope and the core product entities without creating a migration.
- Checkout transaction and idempotency flow.
- Transactional-outbox and SignalR KDS event flow.
- Recipe deduction, stock-movement, negative-stock prevention, and manager-override flow.
- Deployment diagram for local development, CI, staging, and the later Azure production target.
- Privacy data-flow diagram for identity, customer phone, purchase history, payments, audit data, MyInvois, logs, retention, and deletion boundaries.
- Threat model covering unauthorized refunds/voids, payment uncertainty, duplicate checkout, cash manipulation, stock overrides, cross-tenant access, KDS reconnects, token theft, CSRF, and sensitive logging.

Architecture boundary:

```text
apps/       React POS, Admin, KDS, and Customer Display applications
packages/   shared UI, generated API contracts, domain value definitions, and tooling
services/   ASP.NET Core modular-monolith API and background worker
tests/      integration and end-to-end tests
infra/      local and Azure infrastructure definitions
docs/       decisions, plans, evidence, diagrams, and handoffs
```

Backend business modules must expose explicit internal contracts. Domain and application rules must not depend on ASP.NET, Entity Framework, SignalR, or PostgreSQL implementations. Infrastructure implements module-owned interfaces; the API host composes the modules.

### P0-T06 - Record architecture decisions

Objective: convert the approved architecture into durable ADRs.

Required ADR topics:

- React, Vite, TypeScript, PWA, and shared frontend boundaries.
- ASP.NET Core and modular-monolith dependency direction.
- PostgreSQL tenancy columns, outlet scope, runtime roles, and RLS.
- JWT cookie authentication, CSRF, MFA, employee PIN, authorization, and audit logging.
- Fixed-precision money and append-only ledgers.
- Checkout transaction boundaries and idempotency.
- Split payments, discounts, voids, refunds, and external payment confirmation.
- Transactional outbox, internal events, SignalR, and KDS reconnect behavior.
- Recipe deductions, stock projections, negative-stock rules, and manager overrides.
- Customer data minimization, access, retention, and purchase-history privacy.
- Online-first MVP and the deferred offline synchronization boundary.
- Payment-data exclusion, MyInvois boundary, Azure deployment, observability, backup, and restore strategy.

State-model default:

- Order: `Draft -> Placed -> Completed`, with an audited pre-completion `Voided` outcome.
- Payment: `Pending -> PartiallyPaid -> Paid`, plus `Failed` and `ReviewRequired`.
- Fulfilment: `NotSent -> New -> Preparing -> Ready -> HandedOff`, with audited cancellation where permitted.
- Refunds are linked append-only records that produce derived partial or full refund state; they do not rewrite the completed sale.
- Synchronization state remains separate from business state.

### P0-T07 - Approve the frontend specification

Objective: define an original, production-oriented interface before implementation.

Process:

- Use `frontend-app-builder` for complete Image Gen concepts and fidelity planning.
- Use the existing frontend-design and Roast Ledger contracts for tokens, interaction discipline, and accessibility.
- Obtain explicit user approval of the concepts before detailed frontend implementation planning.

Required concepts and states:

- Touch-first product/category selling and instant cart updates.
- Variant and modifier selection.
- Order and item discounts with permission states.
- Split-payment checkout, cash tender/change, card/QR confirmation, failures, and review-required outcomes.
- Shift opening, cash activity, count, close, and variance review.
- KDS queue, new/preparing/ready/handoff states, timers, exceptions, and reconnect state.
- Admin catalog, inventory, customers, shifts, and daily overview.
- Desktop and tablet layouts, keyboard checkout, visible focus, 200% zoom, reduced motion, loading, empty, error, permission, offline, syncing, and review-required states.

The interface may use familiar retail workflows but must not copy Loyverse branding, screenshots, text, or proprietary visual treatment.

### P0-T08 - Define the pilot hardware plan

Objective: create a repeatable compatibility procedure without claiming unexecuted hardware results.

The matrix must cover:

- Selected Windows or Android POS device and supported browser/runtime.
- Browser KDS screen size, placement, viewing distance, and network behavior.
- 80 mm network ESC/POS printer discovery, connectivity, character set, paper width, cut behavior, retry, and manual fallback.
- HID barcode scanner input mode, keyboard layout, focus behavior, supported barcode types, and failure handling.
- Network outage, device restart, printer outage, KDS outage, and manual-service procedures.
- Evidence fields for device model, firmware, OS, browser, network, test date, tester, expected result, actual result, and artifact.

### P0-T09 - Install and validate the Codex workflow

Objective: establish a repository-first acceptance workflow before Phase 1 implementation, with optional native Sol Advisor acceleration.

Use the detailed plan in [`P0_T09_AGENTS_WORKFLOW_PLAN.md`](P0_T09_AGENTS_WORKFLOW_PLAN.md).

Required outcomes:

- If the optional native lane is installed, use only a pinned release asset after checksum validation and separate user approval.
- Preserve the current project instructions inside `AGENTS.md`.
- Define the repository-first task/branch/PR/CI/review/approval route, with complete direct review plus a pre/post snapshot when `.git` is absent.
- Record optional native adapter configuration and the `appTaskLane` when selected; validate exact roles and hashes without claiming host runtime enforcement.
- Record native model/effort and sandbox/permission fields as observed or unobservable. Unobservable native fields block native delegation only.
- Keep the supplied GitHub workflow repository as reference input, not as the Coffee POS source repository or an active alternate role set.

## 5. GitHub preparation

Prepare local issue templates and milestone definitions only.

| Milestone | Planned coverage |
|---|---|
| M0: Architecture & Setup | Remaining Phase 0 and Phase 1 platform work |
| M1: Core POS | Catalog, cart, checkout, payments, receipts, and shifts |
| M2: KDS + Orders | Order lifecycle, kitchen events, preparation, and handoff |
| M3: Inventory System | Ingredients, recipes, movements, waste, and low-stock behavior |
| M4: Reporting & Admin | Daily, product, cashier, shift, inventory, customer, and admin views |

Every future FEAT issue must be atomic, dependency-linked, assigned Light or Heavy Route, and contain ownership, interfaces, acceptance criteria, verification, evidence, and risks. No milestone, issue, remote, branch, commit, push, or pull request may be created without explicit authorization.

## 6. Documentation migration

P0-T09 may introduce the workflow-owned `agent_docs/` framework. To prevent competing sources of truth:

- `PROJECT_PLAN.md` remains authoritative for phases, tasks, gates, and verified evidence.
- Product definition and workflow documents remain authoritative for approved behavior.
- `docs/DECISIONS.md` remains the technical-decision index.
- `agent_docs/project_overview.md`, `project_core_tech.md`, and `project_structure.md` summarize and link to authoritative project documents.
- `agent_docs/project_progress.md` and `latest_session_work.md` become the active progress and handoff files only after a separately approved migration.
- Existing progress and handoff documents must be preserved as clearly marked historical records rather than silently deleted.

## 7. Phase 0 acceptance gate

Phase 0 is complete only when:

- P0-T04A through P0-T09 have separately approved task capsules and verified evidence.
- Every product requirement has one unambiguous MVP or deferred classification.
- Architecture, state models, data boundaries, threat model, and ADRs are approved.
- Frontend concepts and responsive interaction contracts are approved.
- Hardware procedures are documented without unverified compatibility claims.
- The repository-first task, branch/PR/CI when available, or no-`.git` direct-review-plus-snapshot route is documented and evidenced; user approval remains a separate gate.
- If the optional native lane is selected, its exact roles/configuration are validated and host sandbox/permission behavior is recorded as observed or unobservable.
- No competing orchestration workflow remains active.
- No unresolved critical/high planning or security finding remains.
- The user explicitly approved progression to Phase 1 on 18 August 2026.

## 8. Phase 0 non-goals

- No application scaffolding or feature implementation.
- No database schema, migration, SQL, or production data.
- No dependency, container, CI, cloud, or agent-runtime installation except the separately approved P0-T09 workflow action.
- No claim of production, tax, accounting, PCI, MyInvois, hardware, security, accessibility, or pilot readiness.
- No Git staging, commit, remote, push, pull request, deployment, domain purchase, DNS change, or GitHub write without explicit authorization.

# P0-T06 Architecture Decision Records

Status: Approved documentation decision pack; implementation is governed by Phase 1 task gates  
Date: 2026-08-18  
Scope: Coffee POS Phase 0 architecture decisions after approved P0-T05

This document records durable decisions for the Coffee POS MVP. It does not create application code, migrations, dependencies, infrastructure, provider accounts, compliance evidence, or production readiness. P0-T05 remains the architecture/threat-model source; P0-T04A remains the requirements and state-terminology source.

## Decision index

| ADR | Decision | Primary follow-up |
|---|---|---|
| ADR-001 | React, Vite, TypeScript, PWA, and frontend boundaries | P0-T07 and Phase 1 shared UI |
| ADR-002 | ASP.NET Core modular-monolith dependency direction | Phase 1 service scaffold and architecture tests |
| ADR-003 | Tenant/outlet columns, runtime roles, and PostgreSQL RLS | Phase 1 tenancy tests and migration review |
| ADR-004 | JWT cookies, CSRF, MFA, employee PIN, authorization, and audit | Phase 1 security implementation and abuse tests |
| ADR-005 | Fixed-precision money and append-only ledgers | Phase 1 domain types and Phase 2/3 ledger tests |
| ADR-006 | Checkout transaction boundaries and idempotency | Phase 2 checkout implementation and failure tests |
| ADR-007 | Split payments, discounts, voids, refunds, and external confirmation | Phase 2 payment implementation and reconciliation tests |
| ADR-008 | Transactional outbox, internal events, SignalR, and KDS reconnect | Phase 1/3 event and reconnect tests |
| ADR-009 | Recipe deductions, stock projections, negative stock, and overrides | Phase 3 inventory implementation and concurrency tests |
| ADR-010 | Customer minimization, access, retention, and purchase history | Phase 1/2 privacy controls and qualified review |
| ADR-011 | Online-first MVP and deferred offline synchronization | Phase 4 offline design and conflict tests |
| ADR-012 | Payment-data exclusion, MyInvois boundary, and Azure deployment | Phase 5 provider/legal/deployment decisions |
| ADR-013 | Observability, backup, restore, and operational evidence | Phase 1/5 operational verification |

## ADR-001 — Frontend platform and boundaries

- Status: Proposed for implementation after P0-T07 approval.
- Decision: use React, TypeScript, Vite, and a PWA-capable browser shell for the approved POS, Admin, and KDS applications. Shared UI primitives, design tokens, generated contracts, and domain value definitions live in packages; app-specific routes and composition remain in `apps/`. The same stack may support a Customer Display only if a later task separately approves that deferred surface.
- Boundary: applications may depend on packages and versioned service contracts; packages never import app internals; the frontend does not access PostgreSQL, payment providers, or device secrets directly.
- Consequences: fast local development and shared accessibility behavior; the team must maintain contract generation, bundle boundaries, and an explicit offline/sync state UI. Customer Display is not part of the approved MVP implementation scope and must not be scaffolded implicitly.
- Alternatives rejected: a single unstructured dashboard (poor role boundaries), server-rendered pages as the primary shell (weaker shared POS/KDS interaction model), and a microfrontend fleet (unnecessary MVP deployment complexity).
- Verification: build each app from a clean checkout, import-boundary checks, keyboard/accessibility checks, and P0-T07 visual review.

## ADR-002 — ASP.NET Core modular monolith

- Status: Proposed for implementation after P0-T06 review.
- Decision: use one ASP.NET Core API host plus one background worker. Internal modules own their domain/application rules and expose explicit interfaces or internal contracts; the host composes modules without making them separate deployables.
- Boundary: domain and application layers must not depend on ASP.NET Core, Entity Framework, SignalR, PostgreSQL, or provider SDKs. Infrastructure implements module-owned interfaces. Shared contracts contain stable values and messages, not business-rule bypasses.
- Consequences: simple MVP deployment and strong transaction locality; module contracts and dependency-direction tests are mandatory to prevent a monolith from becoming an import graph.
- Alternatives rejected: distributed services (operational cost too high for one outlet) and an anemic single project (weak ownership and test boundaries).
- Verification: architecture/import tests, module contract tests, and clean API/worker startup.

## ADR-003 — Tenant/outlet scope, runtime roles, and RLS

- Status: Proposed for implementation after Phase 1 security review.
- Decision: every tenant-owned record carries `tenant_id`; every outlet-owned record also carries `outlet_id`. Composite foreign keys preserve scope relationships. The request context is resolved from authenticated claims and server-side device/employee assignments, never from a trusted client-supplied scope alone.
- Database boundary: PostgreSQL RLS policies enforce tenant/outlet predicates for runtime roles. Migrations and administrative jobs use separately scoped roles. The application sets a transaction-local scope before queries; the runtime role is not the database owner and cannot bypass RLS by default.
- Consequences: safer cross-tenant isolation and explicit outlet filtering; all queries, indexes, unique keys, background jobs, and tests must carry scope intentionally.
- Alternatives rejected: tenant-only rows without outlet scope (unsafe for future outlets) and application checks without RLS (one missed predicate becomes a data breach).
- Verification: two-tenant and cross-outlet read/write tests, RLS policy tests, runtime-role checks, and job-scope tests.

## ADR-004 — Authentication, authorization, and audit

- Status: Proposed for implementation after Phase 1 security review.
- Decision: browser sessions use short-lived JWT access state in Secure, HttpOnly, appropriately SameSite cookies. State-changing requests require CSRF token/origin validation. Owner/admin accounts require MFA before privileged operations; employee PIN is a controlled convenience mechanism bound to an already authorized device/shift and never replaces server authorization.
- Authorization: roles map to explicit permissions and outlet assignments. The server checks permission, tenant/outlet scope, object state, and manager approval requirements. Every restricted action records actor, scope, reason, source reference, and result in an append-only audit stream.
- Consequences: stronger session and approval controls; token rotation/revocation, key management, PIN lockout, device enrollment, and recovery flows require implementation evidence.
- Alternatives rejected: localStorage bearer tokens (XSS exposure), client-only role checks (bypassable), and shared staff credentials (no accountability).
- Verification: session expiry/rotation, CSRF, MFA, PIN lockout, permission-abuse, manager-approval, and audit completeness tests.

## ADR-005 — Fixed-precision money and append-only ledgers

- Status: Proposed for implementation after domain-type review.
- Decision: money uses fixed-precision decimal values, explicit ISO currency, and server-side quantization rules; floating-point arithmetic is prohibited for totals, tenders, refunds, tax amounts, cash movements, and stock costs. Storage uses a fixed numeric precision selected during migration design (the MVP default is `numeric(19,4)` with currency-aware business rounding).
- Ledger boundary: completed sales, payment tenders, refunds, cash movements, stock movements, and audit events are append-only. Corrections create linked records and never rewrite the original completed sale or movement.
- Consequences: deterministic totals and traceability; every calculation needs named rounding points and tests for split tenders, discounts, refunds, tax assumptions, and cash variance.
- Alternatives rejected: binary floating point (non-deterministic money), mutable totals without source movements (poor auditability), and client-calculated authority (tamperable).
- Verification: property tests for rounding, invariant tests for tender coverage, append-only database constraints, and reconciliation fixtures.

## ADR-006 — Checkout transaction boundaries and idempotency

- Status: Proposed for Phase 2 implementation.
- Decision: a checkout command carries a client transaction identity and canonical payload hash. A unique idempotency reservation is scoped by tenant, outlet, operation, and client transaction ID. The API returns the prior result for an identical retry and rejects a different payload using the same identity.
- Transaction boundary: order, payment group/tenders, stock reservation or movement decision, idempotency result, and transactional-outbox messages commit atomically in the database. In the MVP, card/QR success is observed outside Coffee POS and recorded by an authorized staff member; no provider API, callback, or settlement automation is assumed. An uncertain result remains pending or `ReviewRequired`.
- Consequences: duplicate submissions are safe and downstream work is durable; the manual confirmation command must record actor, time, safe reference when available, and source outcome. Provider API/callback integration is a later separately approved boundary, not a Phase 2 MVP dependency.
- Alternatives rejected: client-only deduplication, a global unscoped idempotency key, and treating timeout as success.
- Verification: double-click, replay, timeout, partial failure, different-payload, concurrent-submit, and rollback tests.

## ADR-007 — Split payments, discounts, voids, refunds, and external confirmation

- Status: Proposed for Phase 2 implementation.
- Decision: a payment group owns the outstanding balance and one or more separately traceable tenders. The group reaches `Paid` only when accepted tenders cover the balance. Discounts are server-calculated from permission-checked inputs. Voids are permitted only before completion and require reason/authorization. Refunds are append-only linked corrections that derive partial/full refund state.
- External boundary: MVP card/QR payments are confirmed outside Coffee POS by staff. Coffee POS records tender type, amount, actor, time, optional safe external reference, and explicit confirmation/failed/review outcome. Raw card data, credentials, provider payloads, callbacks, and settlement automation never enter the MVP system. A future provider adapter requires a separate approved decision and callback/signature validation.
- Consequences: cashier workflows must show remaining balance, change, tender outcomes, and review state; reconciliation is a first-class manager workflow, while provider settlement reconciliation remains deferred.
- Alternatives rejected: one payment row per order (cannot model split tenders), mutable refund totals (breaks traceability), and silently treating staff uncertainty as provider success.
- Verification: split-tender coverage, change, duplicate tender, staff-confirmation authorization, discount permission, pre-completion void, partial/full refund, uncertainty, and reconciliation tests; provider adapter tests are deferred until separately approved.

## ADR-008 — Outbox, internal events, SignalR, and KDS reconnect

- Status: Proposed for Phase 1/3 implementation.
- Decision: domain mutations publish versioned internal events through a transactional outbox written in the same database transaction as the source change. A worker retries publication with durable event IDs; consumers are idempotent. SignalR or an equivalent authenticated transport delivers KDS updates, but the database and event log remain authoritative.
- KDS boundary: paid-order events create or update kitchen work within outlet scope. Clients reconnect by requesting an authorized snapshot and then applying events from a version/cursor. Stale commands fail with a visible review state rather than silently overwriting newer work.
- Consequences: at-least-once delivery and replay handling are required; hub group membership is server-derived from tenant/outlet claims and revalidated on reconnect.
- Alternatives rejected: direct database polling as the primary path, fire-and-forget events, and client-supplied outlet subscriptions.
- Verification: outbox atomicity, duplicate delivery, worker retry, reconnect snapshot, stale version, cross-outlet subscription, and event-isolation tests.

## ADR-009 — Recipe deduction, stock projections, and manager overrides

- Status: Proposed for Phase 3 implementation.
- Decision: recipes and modifiers resolve ingredient quantities at sale finalization. Stock movements are append-only and project available quantity. Negative stock is rejected by default. A manager override requires permission, reason, actor, scope, and audit evidence; it does not erase the shortage or rewrite the sale.
- Boundary: unit conversion and rounding rules are centralized domain services, not UI logic. Reservation/deduction and override decisions share the required transaction and concurrency checks.
- Consequences: catalog changes must be versioned or referenced so historical deductions remain explainable; missing recipes or stale ingredients become explicit exceptions.
- Alternatives rejected: mutable stock totals without movements, silent negative stock, and client-side recipe arithmetic.
- Verification: concurrent sale, missing recipe, insufficient stock, manager override, duplicate retry, unit conversion, rounding, and reconciliation tests.

## ADR-010 — Customer data minimization and purchase history

- Status: Proposed for Phase 1/2 privacy review.
- Decision: collect only the customer fields needed for receipt lookup, consented communication, and authorized purchase-history support. Phone numbers are normalized and access-controlled; customers may decline collection. Attaching a customer to an order is explicit, and suspected duplicates require review rather than silent merge.
- Privacy boundary: purchase history is role/outlet scoped and audited. Retention, correction, deletion, legal hold, and export procedures are documented as policy decisions before production; no automatic deletion or legal compliance claim is made here.
- Consequences: cashier flows must work without CRM data; reports must label scope and freshness; support access is privileged and traceable.
- Alternatives rejected: mandatory CRM capture, unrestricted phone search, and global automatic merge.
- Verification: opt-out, duplicate, unauthorized lookup, outlet isolation, correction/deletion request, audit, and redacted-log tests with qualified privacy review.

## ADR-011 — Online-first MVP and deferred offline synchronization

- Status: Proposed; offline work remains deferred to Phase 4.
- Decision: the MVP is online-first. Phase 1–3 may display offline, queued, syncing, failed, stale, and review-required states, but must not invent a second authoritative sales ledger or silently complete uncertain payment. Full offline sale queueing, conflict resolution, and synchronization protocols are deferred to Phase 4.
- Consequences: connectivity health and recovery messaging are required; any temporary local state must be explicitly non-authoritative and bounded.
- Alternatives rejected: implementing offline writes before the online invariants are tested, or hiding connectivity failures behind optimistic success.
- Verification: online disconnect UI states, retry/review behavior, no-silent-success tests, and later Phase 4 conflict/replay tests.

## ADR-012 — Payment-data exclusion, MyInvois, and Azure boundary

- Status: Proposed; provider, tax, and deployment review remain open.
- Decision: Coffee POS stores payment safe references, tender type, amount, status, and reconciliation evidence only. It does not store raw card numbers, CVV, provider secrets, or unredacted payment payloads. MyInvois is an adapter boundary for later qualified tax/e-Invoice decisions, not a current compliance claim.
- Azure boundary: the later production target may use managed Azure services for the API/worker, PostgreSQL, Redis-compatible coordination, secrets, observability, backups, and network controls. Exact services, regions, retention, costs, and promotion policy remain separate decisions after pilot and qualified review.
- Consequences: provider and cloud adapters must be replaceable and configuration-driven; local development uses safe fakes or emulators, never production credentials.
- Alternatives rejected: embedding provider SDKs in domain code, storing raw payment data, and selecting cloud topology before operational requirements are measured.
- Verification: secret scans, payment payload redaction, adapter contract tests, MyInvois sandbox evidence, infrastructure threat review, backup/restore drills, and qualified tax/privacy review.

## ADR-013 — Observability, backup, restore, and operational evidence

- Status: Proposed for Phase 1/5 operational implementation.
- Decision: services emit structured logs with correlation/request IDs, actor/scope references, safe event names, latency/error metrics, health/readiness checks, and audit references. Logs exclude secrets, raw payment data, and unnecessary personal data. PostgreSQL backups are encrypted and monitored; restore drills produce dated evidence before any production claim.
- Operational boundary: target RPO/RTO, retention periods, alert thresholds, and on-call ownership are measurable pilot/production decisions, not invented in this ADR. Backups do not replace append-only business evidence or reconciliation.
- Consequences: every critical workflow needs diagnostics that support review without leaking sensitive data; restore tests must cover schema, outbox, audit, and tenant scope.
- Alternatives rejected: unstructured unrestricted logs, backups without restore tests, and observability that records full request bodies.
- Verification: redaction scans, trace correlation, failure alerts, backup encryption, restore drill, tenant-scope validation after restore, and pilot runbook review.

## State-model contract

The following transitions are the canonical implementation contract. They remain separate state machines and must not be collapsed into one status field.

- Order: `Draft -> Placed -> Completed`; `Draft` or `Placed` may move to audited pre-completion `Voided` where permitted. A completed order is immutable.
- Payment: `Pending -> PartiallyPaid -> Paid`; `Pending` or `PartiallyPaid` may become `Failed` or `ReviewRequired`. `Paid` is allowed only after accepted tenders cover the outstanding balance.
- Fulfilment: `NotSent -> New -> Preparing -> Ready -> HandedOff`; audited cancellation is allowed only from explicitly permitted fulfilment states.
- Refund: linked append-only records derive partial or full refund state and never rewrite the completed sale.
- Synchronization: queued, syncing, synchronized, stale, failed, and review-required states are separate from order, payment, and fulfilment state.

## Cross-ADR invariants

1. No client-provided tenant, outlet, role, price, total, payment success, stock result, or authorization decision is trusted without server validation.
2. Payment, order, fulfilment, refund, and synchronization states remain separate; `Paid` belongs to the payment group, `Completed` to the order/sale, and `HandedOff` to fulfilment.
3. Completed sales and ledgers are immutable; corrections are linked append-only records.
4. Uncertain, failed, stale, queued, syncing, and review-required states are visible and never silently counted as complete.
5. Raw card data, credentials, JWT secrets, and unnecessary personal data never enter Coffee POS logs or durable application records.
6. P0-T07, P0-T08, P0-T09, Phase 1, and production claims require their own evidence and approval gates.

## Verification matrix

| Decision group | Required evidence before implementation is accepted |
|---|---|
| Frontend and service boundaries | Clean build, import/dependency checks, contract generation, keyboard/accessibility results |
| Tenancy and authorization | Two-tenant/cross-outlet RLS tests, permission-abuse tests, MFA/PIN/session/CSRF results |
| Money, checkout, and payments | Fixed-precision property tests, idempotency/retry/failure tests, split/refund/reconciliation results |
| Events and KDS | Outbox atomicity, duplicate/replay, reconnect, stale-version, and hub isolation tests |
| Inventory and CRM | Concurrency/override tests, recipe traceability, privacy/access/retention review |
| Operations and integrations | Redacted logs, secret scan, backup/restore drill, provider/MyInvois boundary evidence, deployment review |

## Open decisions intentionally deferred

- Exact package manager and pinned dependency versions.
- Exact PostgreSQL numeric scale after currency/tax review.
- MFA provider, PIN enrollment/recovery UX, and session key-rotation service.
- Payment provider, MyInvois API version, Azure service names/regions, RPO/RTO, and retention periods.
- Detailed Phase 4 offline conflict model.

These are explicit follow-ups, not omissions or implementation claims.

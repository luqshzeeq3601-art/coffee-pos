# P0-T06 Architecture Decision Records

Status: Complete; documentation only; fresh review `ship`; approved under blanket authorization  
Phase: Phase 0 — Product, Design, and Repository Foundation  
Owner: Primary agent  
Date opened: 2026-08-18

## 1. Task ID and objective

- Task ID: P0-T06
- Objective: convert the approved P0-T05 architecture into durable architecture decision records before application scaffolding.
- Observable outcome: each required architecture choice has a stated decision, alternatives, consequences, security boundary, and verification path.
- Scope source: `docs/plans/P0_REMAINING_PHASE0_PLAN.md`, P0-T06.

## 2. Ownership

- Implementing owner: Primary agent.
- Worker role, if any: Fresh `reviewer_sol` architecture reviewer after primary verification.
- Files/modules owned:
  - `docs/decisions/P0-T06_ARCHITECTURE_DECISIONS.md`
  - `docs/tasks/P0-T06_ARCHITECTURE_DECISIONS.md`
  - P0-T06 continuity status in `docs/DECISIONS.md`, `docs/TASK_REGISTER.md`, `docs/PROGRESS.md`, and `docs/SESSION_HANDOFF.md`
- Protected files/modules:
  - Approved P0-T04A requirements and P0-T05 architecture/threat-model evidence.
  - Existing historical task results and review records.
- Explicitly excluded:
  - Application code, package installation, migrations, infrastructure, CI, provider accounts, Git actions, deployment, and Phase 1 approval.

Preserve unrelated work. Do not edit outside the owned documentation scope.

## 3. Interfaces and invariants

- Interfaces or contracts affected: frontend boundaries, service-module contracts, tenancy context, authorization, money/ledger semantics, checkout/idempotency, events/outbox, inventory, privacy, payment, MyInvois, Azure, observability, and backup boundaries.
- Data invariants:
  - Tenant and outlet scope is resolved server-side and enforced by application checks plus PostgreSQL RLS.
  - Completed sales and stock ledgers are append-only; corrections are linked records.
  - Payment, order, fulfilment, refund, and synchronization states remain separate.
  - Checkout retries are idempotent and uncertain external payment never becomes silently paid.
  - Outbox publication is transactional with the source mutation and downstream handlers are idempotent.
  - Raw card data and payment secrets never enter Coffee POS storage or logs.
- Compatibility constraints: preserve P0-T04A/P0-T05 terminology, one-shop/one-outlet online-first MVP scope, and the documented monorepo boundaries.
- Security, privacy, money, tax, or synchronization constraints: fixed-precision money, secure cookie plus CSRF plan, privileged MFA, employee PIN safeguards, minimized customer data, explicit uncertain states, no compliance claim, and offline synchronization deferred.

## 4. Implementation constraints

- Settled approach: one React/Vite/TypeScript frontend family, ASP.NET Core modular-monolith API, background worker, PostgreSQL system of record, Redis-compatible coordination, transactional outbox, and authenticated SignalR-compatible KDS updates.
- Required skills or workflows: use the approved P0-T05 architecture pack, repository conventions, task capsule workflow, and fresh read-only reviewer path.
- Dependencies: P0-T05 approved by the user on 2026-08-18; P0-T07, P0-T08, P0-T09, and Phase 1 implementation remain later work.
- Non-goals: no code, migration, package, infrastructure, deployment, external provider setup, tax/legal conclusion, or production-readiness claim.
- External actions prohibited or requiring approval: no staging, commit, push, remote creation, GitHub write, deployment, account creation, or domain/DNS change.

## 5. Acceptance criteria

- [x] All P0-T06 required topics are covered by named decisions in the architecture decision pack.
- [x] Each decision states scope, consequences, security/privacy implications, alternatives, and follow-up verification.
- [x] Decisions preserve P0-T04A/P0-T05 state models and do not introduce application or dependency files.
- [x] Primary verification passes and a fresh `reviewer_sol` returns `ship`.
- [x] User approval of the reviewed P0-T06 decision pack is recorded before P0-T07 implementation work.

## 6. Verification plan

| Check | Exact command or inspection | Expected evidence |
|---|---|---|
| Topic coverage | `rg -n "ADR-|React|Vite|TypeScript|PWA|ASP.NET|modular|PostgreSQL|RLS|JWT|CSRF|MFA|PIN|money|idempot|split payment|outbox|SignalR|KDS|recipe|stock|customer|offline|MyInvois|Azure|backup|restore|observability" docs/decisions/P0-T06_ARCHITECTURE_DECISIONS.md` | Every required topic is present. |
| Cross-document consistency | Compare ADR pack with P0-T05, P0-T04A traceability, and P0-T05 state labels | No conflicting state, tenancy, payment, fulfilment, privacy, or scope terminology. |
| Local links | Read-only Markdown path and heading-anchor scan | All local links resolve. |
| Encoding | Byte check for UTF-8, zero CR bytes, final LF | All visible files pass. |
| Scope | Recursive file-name inspection | No application, package, migration, infrastructure, or runtime files. |
| Git | `git status --short --branch` and `git diff --check` | Report not applicable if target remains without `.git`; do not claim pass. |
| Review | Fresh read-only reviewer inspects the complete P0-T06 pack and capsule | `ship`, `fix-first`, or `rethink` record. |

## 7. Actual results

- Result: P0-T06 decision pack completed; fresh review returned `ship`; user blanket approval is recorded.
- Evidence record: `docs/decisions/P0-T06_ARCHITECTURE_DECISIONS.md`.
- Changed files: decision pack, this task capsule, review record, and P0-T06 continuity status records.
- Git state: target has no Git repository; no Git actions were performed.

## 8. Risks and residual gaps

- Risk or gap: decisions are documented contracts, not tested code or physical schema.
  - Impact: high until Phase 1 verification.
  - Mitigation or follow-up: implement only after P0-T07/P0-T08/P0-T09 gates and retain ADR-linked tests.
  - Owner: Primary agent.
- Risk or gap: exact provider, Azure service tier, retention periods, RPO/RTO, and tax/MyInvois obligations remain subject to qualified review and later operational evidence.
  - Impact: high for production decisions.
  - Mitigation or follow-up: keep adapters and deployment choices behind explicit follow-up decisions; do not claim compliance or production readiness.
  - Owner: Primary agent / qualified reviewer.

## 9. Reviewer verdict

- Reviewer: fresh `reviewer_sol`, `gpt-5.6-sol`, `high`, behaviorally read-only.
- Review record: `docs/reviews/P0-T06_ARCHITECTURE_REVIEW.md`.
- Verdict: `ship`.
- Findings: none after correcting the MVP manual-payment boundary, Customer Display deferral, and complete state transitions.
- Verdict invalidated by later changes: no substantive changes after the fresh review; only evidence/status recording was completed.

## 10. User approval

- Approval state: Approved under the user's blanket instruction on 2026-08-18.
- Approval statement: user said, “Approve all and you can start code.”
- Approved by: user.
- Approval date: 2026-08-18.
- Next authorized action: begin P0-T07 frontend specification work; do not implement application code until the remaining Phase 0 gates are evidenced.

## 11. Handoff

- Continuation point: P0-T06 is approved; P0-T07 frontend specification is the next documentation task.
- Blockers: P0-T07, P0-T08, and P0-T09 evidence and approvals remain outstanding before Phase 1 code.

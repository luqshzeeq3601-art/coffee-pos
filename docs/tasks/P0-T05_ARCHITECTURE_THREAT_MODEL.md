# P0-T05 Architecture and Threat-Model Pack

Status: Complete; documentation only; fresh final-state review `ship`; user approved  
Phase: Phase 0 — Product, Design, and Repository Foundation  
Owner: Primary agent  
Date opened: 2026-08-18

## 1. Task ID and objective

- Task ID: P0-T05
- Objective: produce a decision-ready logical architecture and threat-model pack before application scaffolding.
- Observable outcome: a reviewer can trace user surfaces, service/module boundaries, entities, transaction flows, privacy flows, deployment environments, threats, controls, and planned evidence without reading application code.
- Scope source: `docs/plans/P0_REMAINING_PHASE0_PLAN.md`, P0-T05.

## 2. Ownership

- Implementing owner: Primary agent.
- Worker role, if any: Fresh read-only architecture reviewer after primary verification.
- Files/modules owned:
  - `docs/architecture/P0-T05_ARCHITECTURE_AND_THREAT_MODEL.md`
  - `docs/tasks/P0-T05_ARCHITECTURE_THREAT_MODEL.md`
  - `docs/reviews/P0-T05_ARCHITECTURE_REVIEW_FINAL_STATE.md` after refreshed review evidence is available
  - P0-T05 status lines in `docs/TASK_REGISTER.md`, `docs/PROGRESS.md`, and `docs/SESSION_HANDOFF.md`
- Protected files/modules:
  - Approved P0-T04A product, workflow, traceability, and historical evidence.
  - `PROJECT_PLAN.md` scope and phase gates.
  - Approved plan files under `docs/plans/`.
- Explicitly excluded:
  - Application code, database migrations, physical schema, package installation, infrastructure, CI, deployment, provider integration, and hardware claims.

Preserve unrelated work. Do not edit outside the owned documentation scope.

## 3. Interfaces and invariants

- Interfaces or contracts affected: logical module contracts, logical entity relationships, event/outbox shape, state boundaries, and threat-control evidence only; no public API or physical schema is finalized.
- Data invariants:
  - Completed sales are immutable; refunds are linked append-only corrections.
  - Split tenders cover the outstanding balance before `Paid`.
  - Tenant/outlet scope is enforced in application checks and PostgreSQL RLS.
  - KDS consumes paid-order events and has explicit reconnect/review behavior.
  - Stock deductions are append-only and negative stock is rejected by default.
  - Idempotency and transactional outbox records protect replay and downstream delivery.
- Compatibility constraints: remain within the one-shop, one-outlet, online-first MVP and preserve P0-T04A terminology.
- Security, privacy, money, tax, or synchronization constraints: fixed-precision money; no raw card data; secure HttpOnly JWT cookie plan with CSRF; minimized CRM; visible uncertain states; no legal/tax/compliance claims.

## 4. Implementation constraints

- Settled approach: one modular-monolith API, one worker, PostgreSQL system of record, Redis-compatible coordination, browser POS/Admin/KDS surfaces, and explicit external-payment/printer/scanner boundaries.
- Required skills or workflows: use the project architecture plan and the read-only Graphify inspection guidance; do not generate graph artifacts because this task is documentation-only.
- Dependencies: P0-T04A approved by the user on 2026-08-18; P0-T06 must ratify consequential choices as ADRs.
- Non-goals: no code, migrations, package/dependency files, infrastructure, CI, provider integration, hardware validation, or Phase 1 approval.
- External actions prohibited or requiring approval: no Git staging/commit/push/remote, GitHub write, deployment, domain/DNS change, or external production action.

## 5. Acceptance criteria

- [x] Context, container/module, logical ERD, checkout/idempotency, outbox/KDS, stock, deployment, privacy, and threat-model views are present.
- [x] Every view distinguishes proposed design from implemented evidence and identifies P0-T06/P0-T08 follow-up where needed.
- [x] Tenant/outlet scope, payment uncertainty, append-only ledgers, KDS reconnect, JWT-cookie/CSRF, privacy minimization, and sensitive-logging controls are explicit.
- [x] No application or dependency files are introduced; P0-T06 through P0-T09 remain unimplemented.
- [x] Primary verification passes and a fresh read-only architecture review records `ship`, `fix-first`, or `rethink`.

## 6. Verification plan

| Check | Exact command or inspection | Expected evidence |
|---|---|---|
| Deliverable coverage | `rg -n "system context|container|module map|Entity|checkout|idempot|outbox|KDS|recipe|deployment|privacy|Threat|JWT|CSRF" docs/architecture/P0-T05_ARCHITECTURE_AND_THREAT_MODEL.md` | Every P0-T05 deliverable and control appears. |
| Mermaid integrity | PowerShell count of opening and closing fenced blocks | Equal fence count; no unterminated diagram block. |
| Scope boundary | `rg -n "P0-T06|P0-T07|P0-T08|P0-T09|not.*implementation|not.*migration|no application|deferred" docs/architecture/P0-T05_ARCHITECTURE_AND_THREAT_MODEL.md` | Later work and non-goals remain explicit. |
| Link integrity | Read-only local Markdown path and heading-link scan | All local links resolve. |
| Encoding | Byte check for UTF-8, zero CR bytes, final LF | All visible files pass. |
| Feature/dependency scope | Recursive file-name check | No C#, TypeScript, package, SQL, Docker, lock, or runtime files. |
| Git state | `git status --short --branch` and `git diff --check` | Report not applicable if target remains without `.git`; do not claim pass. |
| Review | Fresh read-only reviewer inspects the complete P0-T05 pack and capsule | A precise verdict and findings record. |

## 7. Actual results

- Architecture pack created at `docs/architecture/P0-T05_ARCHITECTURE_AND_THREAT_MODEL.md`.
- Status: primary documentation pass complete; successive fix-first findings were corrected; fresh final-state review returned `ship`.
- Changed files: architecture pack, this task capsule, and P0-T05 continuity status records.
- Git state: target has no Git repository; no Git actions were performed.
- Primary verification passed: local Markdown paths and anchors resolve; eight Mermaid blocks have balanced fences; all 28 visible files pass UTF-8/LF/final-newline checks; no feature/dependency or Graphify artifacts exist.
- Earlier review evidence is retained in `docs/reviews/P0-T05_ARCHITECTURE_REVIEW.md`; later status/documentation corrections superseded that verdict. The refreshed final-state record is `docs/reviews/P0-T05_ARCHITECTURE_REVIEW_FINAL_STATE.md`.

## 8. Risks and residual gaps

- Risk or gap: logical diagrams are not physical schema, ADRs, or tested implementation.
  - Impact: medium.
  - Mitigation or follow-up: P0-T06 ADRs and later implementation evidence must ratify each consequential choice.
  - Owner: Primary agent.
- Risk or gap: exact payment provider, SignalR/transport choice, Redis product, Azure services, retention policy, and tax/legal treatment remain open.
  - Impact: high for later implementation decisions.
  - Mitigation or follow-up: keep them explicitly deferred to P0-T06 or qualified review.
  - Owner: Primary agent / user approval.
- Risk or gap: target has no Git baseline and cannot provide a Git diff check.
  - Impact: low for documentation content; limits working-tree evidence.
  - Mitigation or follow-up: inspect every visible file directly; initialize Git only under a separately authorized task.
  - Owner: Primary agent.

## 9. Reviewer verdict

- Reviewer: fresh final-state read-only reviewer (`reviewer_sol`, `gpt-5.6-sol`, `high`).
- Review record: `docs/reviews/P0-T05_ARCHITECTURE_REVIEW_FINAL_STATE.md`.
- Verdict: `ship`.
- Findings: none; tenant/outlet scope, separated state models, idempotency, outbox/KDS recovery, privacy, and threat controls were consistent.
- Verdict invalidated by later changes: no substantive fixes after this review; only verdict/status recording was completed.

## 10. User approval

- Approval state: approved.
- Approval statement: user approved P0-T05 on 2026-08-18.
- Approved by: user.
- Approval date: 2026-08-18.
- Next proposed action: prepare the P0-T06 task capsule; do not implement P0-T06 until its own scope, verification, review, and approval gate is satisfied.

## 11. Handoff

- Continuation point: P0-T05 is approved; P0-T06 decision records are the active documentation task and remain subject to their own review/approval gate.
- Blockers: none for P0-T05; P0-T06 application implementation remains unauthorized until its separate gate passes.

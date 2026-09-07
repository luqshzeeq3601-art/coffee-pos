# Task Capsule — P0-T03

Status: Complete  
Phase: Phase 0 — Product, Design, and Repository Foundation  
Owner: Primary agent  
Date opened: 2026-08-13

## 1. Task ID and objective

- Task ID: P0-T03
- Objective: Document cashier, barista/kitchen, manager, and owner operational workflows.
- Observable outcome: A workflow contract covers required normal, exception, offline, synchronization, audit, permission, and cross-role behavior.
- Scope source: `PROJECT_PLAN.md`, Phase 0 task P0-T03, and approved user request.

## 2. Ownership

- Implementing owner: Primary agent.
- Worker role, if any: None.
- Files/modules owned:
  - `docs/COFFEE_SHOP_USER_WORKFLOWS.md`
  - `docs/tasks/P0-T03_USER_WORKFLOWS.md`
  - Authorized continuity links and status in `README.md`, `docs/TASK_REGISTER.md`, `docs/PROGRESS.md`, and `docs/SESSION_HANDOFF.md`.
- Protected files/modules:
  - `PROJECT_PLAN.md`
  - `COFFEE_POS_PRODUCT_RESEARCH.md`
  - `docs/DECISIONS.md`
  - Existing templates and P0-T01/P0-T02 records.
- Explicitly excluded:
  - P0-T04 scope, non-goals, tax terms, and report definitions.
  - UI screens, wireframes, and frontend design work.
  - Final database schema, API implementation, application code, dependencies, and deployment.
  - Sol Advisor installation, configuration, or use.

Preserve unrelated work. Do not edit outside the owned scope.

## 3. Interfaces and invariants

- Interfaces or contracts affected: Workflow responsibilities and cross-role handoffs only; no public API or schema contract.
- Data invariants: Completed sales remain immutable; corrections reference the original; payment, cash, stock, and audit events remain traceable.
- Compatibility constraints: Follow the product research's role, payment, receipt, inventory, and offline limitations.
- Security, privacy, money, tax, or synchronization constraints: State permission intent without final authorization schema; never treat unconfirmed external payment as paid; do not silently duplicate or overwrite offline work.

## 4. Implementation constraints

- Settled approach: One operational workflow document with one structured section per role and a cross-role scenario matrix.
- Required skills or workflows: Use the P0-T02 task/evidence documentation workflow; no Sol Advisor.
- Dependencies: P0-T01 and P0-T02.
- Non-goals: UI design, final schema, final tax/report definitions, hardware validation, and application implementation.
- External actions prohibited or requiring approval: Staging, committing, pushing, creating remotes, deployment, and external system changes.

## 5. Acceptance criteria

- [ ] Cashier, barista/kitchen, manager, and owner workflows each contain all required fields.
- [ ] Required order, payment, kitchen, handoff, correction, cash, stock, reporting, audit, and synchronization scenarios are covered.
- [ ] Offline, syncing, failed, and review-required behavior is explicit.
- [ ] UI, schema, P0-T04, and application work remain excluded.

## 6. Verification plan

| Check | Exact command or inspection | Expected evidence |
|---|---|---|
| Git state | `git status --short --branch` | No staged files, commit, remote, or external action. |
| Formatting | `git diff --check` | No whitespace errors. |
| Documentation inventory | `rg --files docs` | Workflow document and task capsule exist. |
| Required role fields | `rg -n "Cashier|Barista|Kitchen|Manager|Owner|Goal|Preconditions|Main steps|Inputs|Outputs|Permissions|Success|Exception|Offline|Synchronization|Audit|Related modules|Acceptance criteria" docs/COFFEE_SHOP_USER_WORKFLOWS.md` | All role sections and required fields are present. |
| Required scenario coverage | `rg -n "sign-in|shift opening|open ticket|dine-in|takeaway|cash|QR|card|receipt|refund|void|discount|cash-in|cash-out|variance|shortage|waste|low-stock|report|sync|review-required" docs/COFFEE_SHOP_USER_WORKFLOWS.md` | Minimum workflow scenarios are present. |
| Scope boundary | Inspect workflow document and task capsule | No UI, schema, P0-T04, application, or Sol Advisor work. |

## 7. Actual results

- Result: The four-role workflow document and cross-role scenario matrix were created. Required role fields and minimum scenario searches passed.
- Evidence record: Verified results are recorded in `docs/PROGRESS.md` under P0-T03.
- Changed files:
  - `README.md`
  - `docs/COFFEE_SHOP_USER_WORKFLOWS.md`
  - `docs/TASK_REGISTER.md`
  - `docs/PROGRESS.md`
  - `docs/SESSION_HANDOFF.md`
  - `docs/tasks/P0-T03_USER_WORKFLOWS.md`
- Git state: No staged files; commit count `0`; remote count `0`. Normal Git diff is empty because the repository has no commit baseline; untracked contents were inspected directly.

## 8. Risks and residual gaps

- Risk or gap: Tax terms and report definitions are not finalized.
- Impact: Medium.
- Mitigation or follow-up: Resolve in P0-T04 before implementation relies on exact totals.
- Owner: Primary agent.

- Risk or gap: Exact permission names and authorization boundaries are not finalized.
- Impact: Medium.
- Mitigation or follow-up: Resolve in P0-T05/P0-T06 and later security work.
- Owner: Primary agent.

- Risk or gap: Detailed offline conflict behavior is not finalized.
- Impact: High for later implementation.
- Mitigation or follow-up: Resolve and test in Phase 4; this document preserves the required visible states and limitations only.
- Owner: Primary agent.

## 9. Reviewer verdict

- Reviewer: Not run; Sol Advisor is explicitly deferred to P0-T09.
- Review record: None.
- Verdict: Not required for this documentation-only implementation; deferred review path must be validated in P0-T09.
- Findings: No independent Sol review performed.
- Verdict invalidated by later changes: Not applicable.

## 10. User approval

- Approval state: Approved.
- Approval statement: User approved implementation of P0-T03 exactly as proposed.
- Approved by: User.
- Approval date: 2026-08-13.
- Next authorized action: None; wait for separate approval before P0-T04.

## 11. Handoff

- Continuation point: Remain in Phase 0 and wait for P0-T04 approval.
- Blockers: Final tax/report definitions, architecture, permissions, hardware, and offline conflict rules remain future work.


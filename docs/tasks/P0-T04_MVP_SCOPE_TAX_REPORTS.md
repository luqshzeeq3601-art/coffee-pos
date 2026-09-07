# Task Capsule — P0-T04

Status: Complete  
Phase: Phase 0 — Product, Design, and Repository Foundation  
Owner: Primary agent  
Date opened: 2026-08-13

## 1. Task ID and objective

- Task ID: P0-T04
- Objective: Lock MVP scope, non-goals, premium/SaaS deferrals, provisional tax and money terminology, and consistent report definitions.
- Observable outcome: One product-definition document separates MVP commitments from deferred work and defines report language without claiming unverified tax or accounting compliance.
- Scope source: `PROJECT_PLAN.md`, Phase 0 task P0-T04, approved request, and P0-T03 workflow requirements.

## 2. Ownership

- Implementing owner: Primary agent.
- Worker role, if any: None.
- Files/modules owned:
  - `docs/PRODUCT_DEFINITION.md`
  - `docs/tasks/P0-T04_MVP_SCOPE_TAX_REPORTS.md`
  - Authorized continuity links and status in `README.md`, `docs/DECISIONS.md`, `docs/TASK_REGISTER.md`, `docs/PROGRESS.md`, and `docs/SESSION_HANDOFF.md`.
- Protected files/modules:
  - `PROJECT_PLAN.md`
  - `COFFEE_POS_PRODUCT_RESEARCH.md`
  - `docs/COFFEE_SHOP_USER_WORKFLOWS.md`
  - P0-T01–P0-T03 records and existing templates.
- Explicitly excluded:
  - P0-T05 architecture, diagrams, entity relationships, deployment, privacy flow, and threat model.
  - P0-T06 ADRs and detailed stack, tenancy, authentication, offline, payment, MyInvois, or Azure decisions.
  - UI design, wireframes, application code, dependencies, schema, migrations, and deployment.
  - Legal, tax, accounting, payment, or compliance advice presented as verified fact.
  - Sol Advisor installation, configuration, or use.

Preserve unrelated work. Do not edit outside the owned scope.

## 3. Interfaces and invariants

- Interfaces or contracts affected: Product terminology, scope boundaries, and report definitions only; no public API or schema contract.
- Data invariants: Completed sales remain immutable; corrections reference originals; payment, cash, stock, and report totals remain traceable to their source events.
- Compatibility constraints: Use P0-T03 workflow behavior and product research boundaries; do not contradict later phase ownership.
- Security, privacy, money, tax, or synchronization constraints: Never store raw card data; distinguish confirmed, pending, failed, queued, syncing, and review-required states; mark tax and accounting assumptions provisional.

## 4. Implementation constraints

- Settled approach: One product-definition document with MVP scope, non-goals, premium/SaaS deferrals, provisional terminology, report definitions, acceptance boundary, and deferred work.
- Required skills or workflows: Use the P0-T02 task/evidence documentation workflow; no Sol Advisor.
- Dependencies: P0-T01, P0-T02, and P0-T03.
- Non-goals: Architecture and ADR work, UI design, schema design, implementation, compliance certification, and professional tax/accounting conclusions.
- External actions prohibited or requiring approval: Staging, committing, pushing, creating remotes, deployment, and external system changes.

## 5. Acceptance criteria

- [ ] MVP scope is defined for one business and one outlet.
- [ ] MVP non-goals, premium deferrals, and sellable SaaS deferrals are explicit.
- [ ] Product money and tax terms are defined and marked provisional where professional review is required.
- [ ] Sales, payment, shift/cash, inventory/waste/low-stock, and owner report definitions are consistent.
- [ ] No unverified compliance, tax, accounting, profitability, or production conclusion is claimed.
- [ ] P0-T05 and P0-T06 remain not started.

## 6. Verification plan

| Check | Exact command or inspection | Expected evidence |
|---|---|---|
| Git state | `git status --short --branch` | No staged files, commit, remote, or external action. |
| Formatting | `git diff --check` | No whitespace errors. |
| Documentation inventory | `rg --files docs` | Product definition and P0-T04 capsule exist. |
| Scope coverage | `rg -n "MVP|non-goal|Premium|SaaS|Catalog|Checkout|Open ticket|Cash|QR|card|Receipt|Shift|Inventory|Report" docs/PRODUCT_DEFINITION.md` | Required scope categories are present. |
| Money/tax coverage | `rg -n "Currency|Unit price|Gross line|Discount|Taxable base|Tax amount|Order total|Payment amount|Refund|Void|Cash variance|COGS|Gross profit|provisional|professional" docs/PRODUCT_DEFINITION.md` | Terms and provisional review boundaries are present. |
| Report coverage | `rg -n "Sales report|Gross sales|Discounts|Refunds|Voids|Net sales|Tax recorded|Payment report|Expected cash|Counted cash|Inventory|Waste|Low-stock|Owner" docs/PRODUCT_DEFINITION.md` | Definitions are present and linked to consistent source rules. |
| Phase boundary | Inspect product definition and task register | P0-T05/P0-T06 are not started; no architecture or ADR implementation is included. |

## 7. Actual results

- Result: The product-definition document was created. MVP/non-goal coverage, provisional money/tax terminology, report definitions, phase boundaries, links, file scope, and formatting checks passed.
- Evidence record: Verified results are recorded in `docs/PROGRESS.md` under P0-T04.
- Changed files:
  - `README.md`
  - `docs/PRODUCT_DEFINITION.md`
  - `docs/DECISIONS.md`
  - `docs/TASK_REGISTER.md`
  - `docs/PROGRESS.md`
  - `docs/SESSION_HANDOFF.md`
  - `docs/tasks/P0-T04_MVP_SCOPE_TAX_REPORTS.md`
- Git state: No staged files; commit count `0`; remote count `0`. Normal Git diff is empty because the repository has no commit baseline; untracked contents were inspected directly.

## 8. Risks and residual gaps

- Risk or gap: Tax rates, SST/e-Invoice applicability, treatment, rounding, and filing obligations are not professionally verified.
- Impact: High for production or live-pilot use.
- Mitigation or follow-up: Obtain qualified Malaysian tax/accounting review before implementation is treated as compliant or before live pilot.
- Owner: Product owner with qualified professional.

- Risk or gap: MVP report definitions are product reporting conventions, not statutory accounting definitions.
- Impact: High for financial reporting or external merchant use.
- Mitigation or follow-up: Reconcile with professional advice and later accounting/reporting tests.
- Owner: Primary agent and product owner.

- Risk or gap: KDS, offline synchronization, MyInvois, and advanced inventory remain deferred.
- Impact: Medium for current MVP; high for pilot readiness.
- Mitigation or follow-up: Track against Phase 3–5 tasks and their acceptance gates.
- Owner: Primary agent.

## 9. Reviewer verdict

- Reviewer: Not run; Sol Advisor is explicitly deferred to P0-T09.
- Review record: None.
- Verdict: Not run for this documentation-only implementation; deferred review path must be validated in P0-T09.
- Findings: No independent Sol review performed.
- Verdict invalidated by later changes: Not applicable.

## 10. User approval

- Approval state: Approved.
- Approval statement: User approved implementation of P0-T04 exactly as proposed.
- Approved by: User.
- Approval date: 2026-08-13.
- Next authorized action: None; wait for separate approval before P0-T05 or P0-T06.

## 11. Handoff

- Continuation point: Remain in Phase 0 and wait for P0-T05 approval.
- Blockers: Professional tax/accounting review, architecture, ADRs, and later implementation evidence remain outstanding.


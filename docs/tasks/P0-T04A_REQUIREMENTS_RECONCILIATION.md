# P0-T04A Requirements Reconciliation

Task ID: P0-T04A  
Status: verified locally; user approval pending  
Owner: Primary agent  
Dependency: P0-T04 verified  
Scope: product-definition addendum, workflow reconciliation, traceability, and continuity updates

## Objective

Reconcile the newer Coffee POS brief with the verified P0-T04 product baseline before architecture, frontend concepts, hardware planning, or workflow installation. Preserve the one-shop MVP boundary and stronger safety rules while adding split payments, basic customer CRM, MVP KDS, admin overview, JWT requirements, and explicit state-model boundaries.

## Owned files

- `docs/PRODUCT_DEFINITION.md`
- `docs/COFFEE_SHOP_USER_WORKFLOWS.md`
- `docs/REQUIREMENTS_TRACEABILITY.md`
- `docs/tasks/P0-T04A_REQUIREMENTS_RECONCILIATION.md`
- `docs/PROGRESS.md`
- `docs/SESSION_HANDOFF.md`
- `docs/TASK_REGISTER.md`
- `README.md`

Protected and excluded:

- Do not implement application code, database schema, migrations, APIs, infrastructure, frontend screens, hardware integrations, or workflow installation.
- Do not change the verified P0-T01 through P0-T04 evidence claims.
- Do not stage, commit, push, create a remote, or write to GitHub.

## Interfaces and invariants

- Split tenders are recorded separately and must cover the outstanding balance before the sale reaches `Paid`.
- Cash may produce change; external card/QR tenders require staff confirmation of external success.
- A completed sale is immutable; refunds are linked append-only corrections and voids apply only before completion.
- Basic CRM is optional, role-scoped, and limited to name, normalized phone, and authorized purchase history.
- KDS receives paid orders; fulfilment state is separate from payment and order state.
- Failed, pending, queued, syncing, and review-required states are never silently counted as completed.
- Loyalty, marketing, supplier, offline-sale, multi-outlet, and SaaS capabilities remain deferred.

## Acceptance criteria

- Product definition classifies split payments, basic CRM, MVP KDS, and admin overview as MVP scope.
- Product definition explicitly preserves loyalty, marketing, customer self-service, offline sales, suppliers, multi-outlet, and SaaS deferrals.
- Workflow document covers split tenders, CRM opt-out/duplicate behavior, paid-to-KDS handoff, KDS review/reconnect, and admin overview.
- Requirements traceability maps every supplied requirement to classification, owner, and planned evidence.
- No later Phase 0 task is marked implemented by this task.
- Continuity documents identify P0-T04A evidence and the next approval gate.

## Verification commands

```powershell
rg -n "split|customer CRM|KDS|admin overview|JWT|P0-T04A|traceability" docs PROJECT_PLAN.md
rg -n "MVP|Future|Phase 0|Phase 1|P0-T05|P0-T06|P0-T07|P0-T08|P0-T09" docs/REQUIREMENTS_TRACEABILITY.md docs/PRODUCT_DEFINITION.md
rg -n "^\| Requirement \| Classification \| Owning work \| Planned acceptance evidence \|" docs/REQUIREMENTS_TRACEABILITY.md
git diff --check
```

Expected evidence:

- Requirement and workflow searches find the new scope and state boundaries.
- The traceability header and matrix are present.
- Local Markdown links resolve to existing files and headings.
- All visible files use UTF-8, LF endings, and final newlines.
- Because the target has no Git repository, `git diff --check` and `git status --short --branch` are reported as not applicable; untracked files are inspected directly.

## Actual results

- Product definition updated with split tenders, basic CRM, MVP KDS/admin scope, revised deferrals, and Codex workflow wording.
- Workflow updated with separate states, split tenders, CRM behavior, KDS handoff, audit events, and end-to-end scenarios.
- `docs/REQUIREMENTS_TRACEABILITY.md` created with product, architecture, state, workflow, security, testing, and future-scope mappings.
- This task capsule created with acceptance criteria and verification commands.
- Requirement/state searches passed, including the traceability table-header check.
- Local link-path and link-anchor checks passed across all Markdown files.
- All 21 Markdown files and all 24 visible files passed UTF-8, LF-ending, and final-newline checks.
- Feature/dependency-file and Graphify-artifact checks found none.
- Both plan files and the P0-T04A reconciliation files were inspected directly and completely.
- The target has no Git repository; `git diff --check` and `git status --short --branch` returned the expected not-a-repository errors and were not treated as passes.
- Progress, task-register, and session-handoff records now identify the evidence and preserve the user-approval gate.

## Review and approval

- Primary agent inspected the complete visible documentation set directly; the target has no Git baseline.
- No fresh architecture reviewer is required for this documentation-only task; P0-T05 requires a fresh commitment review before architecture decisions are implemented.
- User approval is required before P0-T05 begins.

## Residual risks

- Tax, payment-provider, hardware, legal, and compliance decisions remain provisional or deferred.
- The target folder is not yet a Git repository and has no remote; Git setup is outside this task.
- The existing target `AGENTS.md`, if later copied or created, must not be treated as Codex workflow-validated until P0-T09 passes.

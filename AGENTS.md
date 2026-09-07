# Coffee POS Agent Instructions

## 1. Communication

- Explain decisions in direct, short language.
- Prefer numbered sections and organized bullets.
- Lead with the outcome and verified evidence.
- Never invent results, metrics, production readiness, compliance, or test status.

## 2. Sources of Truth

Read these before substantial work:

- `PROJECT_PLAN.md`: phases, architecture, decisions, task gates, and evidence.
- `COFFEE_POS_PRODUCT_RESEARCH.md`: researched product and compliance requirements.
- `docs/DECISIONS.md`, `docs/PROGRESS.md`, and `docs/SESSION_HANDOFF.md` when they exist.

Follow the active phase boundary. Do not begin the next phase until the current phase passes its gate and the user explicitly approves progression.

## 3. Required Orchestration: Antigravity Multi-Model Workflow

Use the repository-first project workflow for implementation tasks, orchestrating natively within Antigravity using its fleet of reasoning models.

### Parent ownership

The primary agent owns:

- User intent and material clarification.
- Architecture, interfaces, data invariants, and decomposition.
- Planning mode, task capsules, and file/module ownership.
- Inspection of the actual working-tree diff.
- Rerunning required verification.
- Correction decisions, phase gates, and final acceptance.

Worker and tool outputs are claims until the primary agent verifies them against active code.

### Model & role routing

- **Coordinator / Main Planner:** `Gemini 3.1 Pro` or `Gemini 3.7 Flash (High)` for system decomposition, planning mode (`implementation_plan.md`), invariants, and task gates.
- **Routine implementer (`agy_routine`):** `Gemini 3.7 Flash (High)` for fast execution of bounded mechanical, boilerplate, wiring, UI components (Roast Ledger), DTOs, API endpoints, unit test suites, and documentation.
- **High-complexity implementer (`agy_high`):** `Gemini 3.1 Pro` or `Claude Thinking` for authentication, authorization, RLS, migrations, money/tax logic, ledgers, concurrency, offline sync, MyInvois, deployment, or wide refactors (max 1 concurrent).
- **Advisor / Adversarial Reviewer (`agy_advisor`):** `Claude Thinking` or `Gemini 3.1 Pro` with `doubt-driven-development` and `code-review-and-quality` skills for commitment-boundary and final read-only reviews.

### Repository-first and direct snapshot acceptance lanes

- The default project acceptance surface is the normal GitHub repository workflow: task capsule, branch, primary verification, pull-request review, required CI, and explicit user approval.
- When `.git` is absent (such as directory copy mode), use a fresh direct independent review with complete file inspection and a pre/post snapshot.
- A declared `sandbox_mode = "read-only"` is a requested setting. Call it enforced only when the host exposes matching read-only sandbox and permission evidence.

### Mandatory task capsule

Every implementation delegation must include:

```text
TASK ID AND OBJECTIVE
- Observable outcome and why it matters.

FILES AND OWNERSHIP
- Exact files or modules owned by this worker.
- Protected and excluded areas.
- You are not alone in the codebase. Preserve other edits, do not revert unrelated work, and do not edit outside ownership.

INTERFACES AND INVARIANTS
- APIs, schemas, types, behavior, data rules, and compatibility constraints.

IMPLEMENTATION CONSTRAINTS
- Settled approach, repository rules, safety limits, non-goals, and relevant skill requirements.

VERIFICATION
- Exact commands or inspection methods.
- Concrete expected success evidence.

RETURN
- Status: complete, partial, or blocked.
- Actual changed files and behavior.
- Exact verification commands and results.
- Judgment calls, gaps, and residual risks.
```

### Commitment review

Obtain a fresh adversarial commitment review before implementing consequential:

- Architecture or module-boundary changes.
- Public API or event-contract changes.
- Database schemas or migrations.
- Authentication, authorization, or tenant-isolation changes.
- Money, tax, payment, refund, loyalty, or inventory-ledger logic.
- Offline synchronization or concurrency rules.
- MyInvois or payment-provider integration.
- Production deployment, backup, restore, or security changes.

The primary agent makes the final decision after considering the review.

### Final review

After implementation and primary verification, use an independent review path: a GitHub pull-request review plus required CI by default; when `.git` is absent, use a fresh direct independent review with complete file inspection and a pre/post snapshot; or use a fresh adversarial reviewer (`Claude Thinking` / `Gemini 3.1 Pro`) when review is requested.

When adversarial review is selected, the reviewer must:

- Remain behaviorally read-only.
- Inspect the actual files and complete accumulated diff.
- Review correctness, regressions, scope, interfaces, tests, and material risk.
- Return exactly `ship`, `fix-first`, or `rethink`, with precise evidence.
- Never implement its own fixes.

If any fix is made, the old verdict is invalid. Rerun verification and request a new fresh review or updated pull-request review.

## 4. Concurrency

- Use at most three child agents alongside the primary agent.
- Keep one child slot available for the fresh reviewer.
- Parallelize only independent work with non-overlapping file/module ownership.
- Serialize shared-file edits and dependency chains.
- Workers may not spawn children unless the primary agent explicitly authorizes it for that task.

## 5. Frontend Work

Every task that creates or changes visible UI must use the installed `frontend-design` skill.

Before coding, the responsible agent must define:

- User and screen's single job.
- Relevant design tokens.
- Layout or short ASCII wireframe.
- Responsive Windows and Android behavior.
- Keyboard, touch, and accessibility behavior.
- Loading, empty, error, permission, offline, syncing, and review-required states.
- One critique explaining how generic dashboard styling was avoided.

During implementation:

- Follow the Roast Ledger design contract in `PROJECT_PLAN.md`.
- Reuse shared tokens and components.
- Do not introduce a new color, font, icon system, or component pattern without updating the design contract and obtaining approval when the change is material.
- Use real coffee-shop content and workflows, not generic placeholder dashboards.

Before acceptance:

- Run frontend tests.
- Test keyboard and touch behavior.
- Test desktop and Android viewport sizes.
- Capture and inspect screenshots for visible defects.
- Check WCAG AA contrast, visible focus, 200% zoom, and reduced motion.

## 6. Security and Data Safety

- Treat authentication, tenant isolation, payment records, personal data, MyInvois, and offline synchronization as high risk.
- Never store card number, CVV, PIN, magnetic-stripe data, or payment credentials.
- Never log passwords, tokens, secrets, full payment data, or unnecessary personal information.
- Use fixed-precision decimals for money.
- Preserve append-only sales, payment, cash, loyalty, and stock ledgers.
- Do not weaken RLS, authorization, tests, audit logs, validation, or failure visibility.
- Never use production personal data in development or staging.
- Use OWASP ASVS Level 2 as the security verification baseline.

## 7. Git and External Actions

- Preserve existing user and agent changes.
- Do not stage, commit, amend, push, create a PR, deploy, purchase a domain, change DNS, or modify production/external systems unless the user explicitly requests that exact action.
- Never use destructive Git or filesystem commands without clear authorization and verified targets.
- Report the final Git state, including untracked files relevant to the task.

## 8. Testing and Evidence

- Define acceptance and verification before implementation.
- Run focused tests first, then relevant package/integration tests.
- Use real disposable PostgreSQL and Redis-compatible services for integration tests when required.
- High-risk work requires independent evidence, not only mocked tests.
- Record exact commands, results, artifacts, limitations, and residual risks.
- Never mark a task or phase complete because the code looks correct.
- Update `PROJECT_PLAN.md` evidence only with verified facts.

## 9. Documentation Continuity

Use lightweight project-owned continuity documents:

- `docs/DECISIONS.md`: accepted/rejected decisions and rationale.
- `docs/PROGRESS.md`: current phase, verified tasks, blockers, and next milestone.
- `docs/SESSION_HANDOFF.md`: exact last verified state and continuation point.

Only the primary agent or a specifically assigned documentation worker may update these files. Documentation agents receive verified facts and must not infer completion.

## 10. Completion Definition

A task is complete only when:

1. The implementation matches the task capsule.
2. The primary agent inspected the actual diff.
3. Required tests and checks passed with recorded evidence.
4. Independent review evidence is recorded: GitHub pull-request review and required CI for the repository-first lane, a fresh direct independent review with complete file inspection and a pre/post snapshot when `.git` is absent, or a fresh adversarial review verdict of `ship`.
5. Documentation and evidence are current.
6. No required work or critical/high finding remains.

A phase is complete only after the user approves its acceptance evidence.

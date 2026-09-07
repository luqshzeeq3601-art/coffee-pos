# P0-T09 Codex Workflow and AGENTS.md Plan

Updated: 18 August 2026  
Status: Complete; repository-first acceptance revision reviewed `ship`; Phase 0 approved; Phase 1 active
Dependency: Approved evidence for P0-T04A through P0-T08 and separate authorization for P0-T09

## 1. Objective

Establish one repository-first project workflow with explicit route selection, bounded ownership, primary verification, independent GitHub review/CI evidence, and optional native Sol Advisor acceleration without claiming host capabilities that are not observable.

The canonical project entry point is uppercase `AGENTS.md`. Do not create a second lowercase `agent.md` because parallel instruction files could conflict and the referenced Codex workflow manages `AGENTS.md`.

## 2. Optional native installation boundary

If the optional native lane is installed or used, it must:

1. Download a pinned release asset, not a source archive.
2. Verify the release against its published checksum before extraction.
3. Read and follow the bundled bootstrap/install instructions.
4. Preserve global Codex instructions and import the current project `AGENTS.md` into the workflow's protected project-local region.
5. Configure models and efforts through the workflow-owned configuration/templates, not by editing generated runtime files directly.
6. Start a fresh Codex session before claiming that new agent definitions are active.
7. Stop the native lane without substitution if installation, model routing, permissions, or the reviewer sandbox cannot be verified; do not block the repository-first lane on unavailable host metadata.

P0-T09 must not install the workflow as an application dependency, add feature code, stage or commit files, create a remote, or write to GitHub.

## 3. Repository-first acceptance boundary

The normal acceptance surface is the project's own GitHub repository: task capsule, branch, primary verification, pull-request review, required CI, and explicit user approval. The supplied [`viettran-edgeAI/codex_workflow`](https://github.com/viettran-edgeAI/codex_workflow) repository is a workflow reference, not the Coffee POS source repository and not an installed project runtime.

P0-T09 has two explicit lanes:

- **Repository-first lane (default):** the primary agent owns the task, performs the required checks, records the evidence, and uses GitHub pull-request review and CI when the project is linked to a Git repository. If this documentation export has no `.git`, Git/PR/CI evidence is recorded as not applicable rather than invented, and a fresh direct independent review with complete file inspection plus a pre/post snapshot is the acceptance artifact.
- **Native Sol Advisor lane (optional):** use only the exact configured `sol_advisor_routine`, `sol_advisor_high`, `sol_advisor_advisor`, and opt-in Luna app-task lane after their setup and host capabilities are observed. Unobservable native runtime or OS sandbox metadata blocks this lane only; it does not block repository-first work.

The reference repository's alternate `executor_*`, tester, explorer, documentation, and handoff roles must not be copied into this project's active role list unless a separate installation and approval explicitly selects that workflow.

## 4. Route policy

### Light Route

- Default for questions and small leaf tasks.
- Main agent works directly with no subagents.
- Suitable for minor documentation changes, isolated CRUD, small UI adjustments, simple API changes, and bounded fixes.
- A small task stays worker-free even when it occurs during a wider project session.

### Heavy Route

- User-selected for substantive deployment work.
- Required for architecture, public contracts, authentication, authorization, RLS, schemas, migrations, checkout, payments, refunds, discounts, inventory ledgers, KDS, concurrency, offline synchronization, MyInvois, security, deployment, and broad frontend features.
- The main agent owns architecture and decisions; specialized agents receive bounded work packages.
- Heavy selection does not require unnecessary spawning. Use only roles that materially help the task.

### Medium Route

- Not part of the default Coffee POS workflow.
- Use only when the user explicitly selects it for a larger task that is better completed by the main agent with workflow continuity but without implementation workers.

Route selection remains explicit. If a Heavy-class task is requested while the session remains on Light, stop before implementation, explain the required route, and request the user to select Heavy.

## 5. Model and effort assignments

Sol Advisor v0.5.0 installs 3 project-scoped native companion roles into `.codex/agents/` and enables the opt-in `appTaskLane` for user-visible app tasks.

| Role / Identifier | Model | Effort | Sandbox / Permission | Responsibility |
|---|---|---|---|---|
| Main agent (inherited session) | Inherited | Inherited | User / session permission | Primary requirements, architecture, integration, verification, and acceptance |
| `sol_advisor_routine` (optional native lane) | `gpt-5.6-sol` | `medium` | Workspace write within capsule | Routine bounded mechanical, boilerplate, wiring, documentation, and ordinary UI |
| `sol_advisor_high` (optional native lane) | `gpt-5.6-sol` | `xhigh` | Workspace write within capsule | Complex architecture, security, auth, RLS, migrations, money/tax, offline sync, wide blast radius |
| `sol_advisor_advisor` (optional native lane) | `gpt-5.6-sol` | `high` | Declared `sandbox_mode = "read-only"`; host enforcement must be observed | Fresh commitment and final review; behavioral read-only; never implements fixes |
| `appTaskLane` (opt-in native app tasks) | `gpt-5.6-luna` | `max` | App task tools (`create_thread`, etc.) | User-visible Luna tasks when explicitly opted into |

Required configuration:

- Active client adapter: `codex` (`project` scope at workspace root).
- Installed files: `sol-advisor-routine.toml`, `sol-advisor-high.toml`, `sol-advisor-advisor.toml`.
- Maximum concurrent child agents: `3`.
- Maximum concurrent `sol_advisor_high` agents: `1`.
- Automatic update installation: disabled.
- Worker report package: concise and evidence-focused.
- Workers may not spawn their own agents.
- Keep sufficient lifecycle capacity for testing, review, or session closure.

## 6. Role ownership

### Main agent

Owns:

- User intent and material clarification.
- Product and architecture decisions.
- Public interfaces, schemas, state machines, invariants, compatibility, and migration decisions.
- Task decomposition, dependencies, and agent ownership.
- Actual working-tree inspection and rerunning critical verification.
- Correction decisions, phase gates, final acceptance, and user communication.

Agent reports are claims until the main agent checks the relevant evidence.

### Routine implementer (`sol_advisor_routine`)

- Implements bounded, well-specified, mechanical work within assigned capsule.
- Preserves settled architecture, owned files, interfaces, and concurrent edits.
- Runs requested checks and reports verification evidence.
- Does not edit outside owned files or change Git state.

### High implementer (`sol_advisor_high`)

- Implements complex, security-sensitive, algorithmic, debugging, or wide-blast-radius work within settled architecture.
- Surfaces ambiguity, preserves concurrent edits, and reports verification evidence.
- Restricted to at most 1 concurrent worker.

### Optional native reviewer (`sol_advisor_advisor`)

- Uses a fresh context and declares `sandbox_mode = "read-only"`.
- Performs commitment review before consequential architecture, schema, public API, authentication, authorization, tenant isolation, money, payment, refund, inventory, synchronization, MyInvois, security, or deployment work when the native lane is selected.
- Performs final review only after implementation and primary verification when the native lane is selected.
- Returns exactly `ship`, `fix-first`, or `rethink` with precise evidence.
- Never implements its own findings; any fix invalidates the verdict and requires new verification and fresh review.
- OS-level sandbox enforcement is bounded by host client profile; behavioral read-only contract is strictly enforced.

## 7. Mandatory task capsule

Every delegated package must contain:

```text
TASK ID AND OBJECTIVE
- Observable result and why it matters.

FILES AND OWNERSHIP
- Exact files or modules owned by this agent.
- Protected and excluded areas.
- Preserve other edits; do not revert unrelated work or edit outside ownership.

INTERFACES AND INVARIANTS
- APIs, schemas, types, state transitions, business rules, compatibility, and failure behavior.

IMPLEMENTATION CONSTRAINTS
- Settled approach, skills, security limits, non-goals, and prohibited actions.

VERIFICATION
- Exact checks, test commands, expected evidence, and failure criteria.

ESCALATION
- Decisions or conditions that must return to the main agent.

RETURN
- complete, partial, or blocked.
- changed behavior and owned files.
- exact checks and actual results.
- risks, deviations, missing evidence, and next action.
```

## 8. Heavy execution and repair loop

```text
User selects Heavy Route
        -> Main agent locks decisions and task capsules
        -> Optional native Sol commitment review when the native lane is selected
        -> Primary implementation or explicitly delegated native implementation
        -> Primary verification and required tests/CI
        -> GitHub pull-request review by default, or direct independent review plus snapshot when no .git exists
        -> Optional fresh native Sol final review when selected and observable
        -> main agent inspects diff and reruns critical checks
        -> user-authorized merge/phase approval
```

Repair rules:

- Routine defects return to the responsible owner through the same pull request or task capsule.
- Test and fixture defects remain with the test owner; production fixes return to the implementation owner.
- After two unsuccessful repair cycles, escalate to the main agent for diagnosis, rescoping, replacement, or takeover.
- Escalate immediately for a public-contract change, architecture defect, security/migration risk, ownership conflict, or expanded authority.

## 9. Frontend requirements

- Use `frontend-app-builder` for new surfaces, major redesigns, Image Gen concepts, accepted-spec implementation, and browser/screenshot fidelity verification.
- Use the existing frontend-design and Roast Ledger rules for tokens, original visual direction, touch/keyboard behavior, and avoidance of generic dashboard styling.
- Do not implement a substantial frontend before the complete relevant concept is approved.
- Test desktop and tablet layouts, keyboard checkout, touch targets, WCAG AA contrast, visible focus, 200% zoom, reduced motion, and loading/empty/error/permission/offline/sync/review states.
- Keep interactive text and controls code-native; never ship a screenshot as the UI.

## 10. Security and business invariants

- Money uses fixed-precision decimal types; binary floating point is prohibited.
- Completed sales are immutable.
- Payments, refunds, cash movements, stock movements, loyalty movements, and audit events are append-only.
- Never store or log raw card number, CVV, PIN, magnetic-stripe data, payment credentials, passwords, tokens, or unnecessary personal information.
- Browser JWTs use secure HttpOnly cookies; state-changing requests require CSRF protection.
- Tenant/outlet scope is enforced in application code and PostgreSQL RLS.
- Runtime database roles must not own tables or bypass RLS.
- Checkout, payments, refunds, receipts, stock movements, and outbox events require transaction and idempotency rules.
- Restricted refunds, voids, discounts, cash movements, stock overrides, and shift closing require permission, reason, actor, timestamp, and audit evidence.
- Use OWASP ASVS Level 2 as the security verification baseline.

## 11. Git and external-action rules

- Preserve existing user and agent changes.
- Agents must not stage, commit, amend, push, create remotes, issues, milestones, branches, pull requests, deployments, domains, DNS changes, or production mutations.
- The main agent may perform an external or Git action only after explicit authorization for that exact action.
- Remove or override any copied automatic `git add`, commit, push, or deployment behavior from the workflow.
- Destructive cleanup requires verified targets and explicit authorization.
- Every handoff reports the final Git state and relevant untracked files.

## 12. Documentation ownership

- `PROJECT_PLAN.md` remains authoritative for scope, phases, gates, and evidence.
- Product definition and workflow documents remain authoritative for product behavior.
- `docs/DECISIONS.md` remains the accepted/rejected technical-decision index.
- Workflow `agent_docs/` files summarize and link; they must not silently redefine authoritative documents.
- The main agent owns progress and handoff truth during normal work.
- Historical continuity files are preserved during migration.

## 13. P0-T09 verification

P0-T09 cannot be marked complete until the repository-first evidence and the selected review lane are recorded:

- The workflow reference and any installed adapter version/checksum evidence.
- The active project root and canonical `AGENTS.md`.
- The repository-first route, task capsule, ownership, and explicit GitHub PR/CI review boundary.
- If the optional native lane is selected: the exact enabled role list, model/effort configuration, and app-task lane are validated.
- Native workspace-write agents remain bounded by task ownership.
- Native reviewer behavior is recorded as behavioral read-only; host sandbox/permission enforcement is stated as observed or unobservable, never inferred.
- Light Route calls no subagents for a small task.
- Heavy Route uses only necessary agents and follows the capsule/test/review loop; it may remain primary-agent-only when native metadata is unavailable.
- The selected independent review artifact is a GitHub pull-request review plus required CI, a fresh direct independent review with complete file inspection and pre/post snapshot when no `.git` exists, or a fresh `sol_advisor_advisor` `ship` verdict when the native lane is selected and observable.
- No automatic Git staging, commit, push, or external write occurs.
- The supplied `codex_workflow` GitHub repository is reference-only unless separately installed and approved; no competing workflow is claimed active here.
- Project documentation points to one current progress and handoff source.

If repository-first evidence, active documentation, or the selected independent review is missing, stale, or contradictory, P0-T09 remains blocked. Unobservable native runtime fields block only optional native delegation and must be recorded as a limitation; they do not invalidate a snapshot-verified direct review in a no-`.git` documentation export.

Current validation evidence: active setup-status call returned `ready`; saved project preferences and `validate_configuration` returned `valid: true`; the three allowlisted adapter files were installed with hashes matching the render preview; and the fresh no-`.git` direct independent review returned `ship`. The user approved Phase 0 closure and Phase 1 start on 18 August 2026.

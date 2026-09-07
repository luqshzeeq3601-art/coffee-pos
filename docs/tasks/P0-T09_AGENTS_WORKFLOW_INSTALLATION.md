# P0-T09 Codex Workflow Installation and Validation

Status: Complete; repository-first acceptance revision reviewed `ship`; Phase 0 approved  
Phase: Phase 0 — Product, Design, and Repository Foundation  
Owner: Primary agent  
Date opened: 2026-08-18

## 1. Task ID and objective

- Task ID: P0-T09
- Objective: establish a repository-first Codex workflow with explicit route, ownership, primary verification, independent GitHub review/CI evidence, and optional validated native Sol Advisor acceleration before Phase 1 implementation.
- Observable outcome: the project records its repository-first acceptance lane and, when selected, validates the configured native roles (`sol_advisor_routine`, `sol_advisor_high`, `sol_advisor_advisor`) plus `appTaskLane` (`gpt-5.6-luna`/`max`). Unobservable native host metadata is recorded as a limitation and blocks only native delegation.
- Scope source: `docs/plans/P0_REMAINING_PHASE0_PLAN.md` and `docs/plans/P0_T09_AGENTS_WORKFLOW_PLAN.md`.
- Workflow reference: `https://github.com/viettran-edgeAI/codex_workflow` (reference only; not the Coffee POS source repository).

## 2. Ownership and protected scope

- Implementing owner: Primary agent.
- Protected files: current `AGENTS.md`, approved project plans, P0-T04A through P0-T08 evidence, and historical continuity records.
- Explicitly excluded from this task beyond the allowlisted generated adapters: application code, dependencies, additional workflow runtime files, Git actions, GitHub writes, deployment, and external configuration.

## 3. Required project routing

Reconciled project assignments matching the Sol Advisor v0.5.0 client adapter and app-task lane:

| Role / Identifier | Model | Effort | Permission / Sandbox | Responsibility |
|---|---|---|---|---|
| Main agent (inherited session) | Inherited | Inherited | User / session permission | Primary requirements, architecture, integration, verification, and acceptance |
| `sol_advisor_routine` (optional native lane) | `gpt-5.6-sol` | `medium` | Workspace write within capsule | Routine bounded mechanical, boilerplate, wiring, documentation, ordinary UI |
| `sol_advisor_high` (optional native lane) | `gpt-5.6-sol` | `xhigh` | Workspace write within capsule | Complex architecture, auth, RLS, migrations, money/tax, offline sync (max 1 concurrent) |
| `sol_advisor_advisor` (optional native lane) | `gpt-5.6-sol` | `high` | Declared `sandbox_mode = "read-only"`; host enforcement must be observed | Fresh commitment and final review; behavioral read-only; never implements fixes |
| `appTaskLane` (opt-in native app tasks) | `gpt-5.6-luna` | `max` | App task tools (`create_thread`, etc.) | User-visible Luna tasks when explicitly opted into |
| Repository-first lane | Project repository | — | GitHub branch/PR/CI | Default acceptance path owned by the primary agent |
| Concurrency | — | — | — | Maximum three child agents; workers may not spawn children |

The detailed route, repair loop, frontend/security invariants, Git restrictions, and verification rules remain in [`docs/plans/P0_T09_AGENTS_WORKFLOW_PLAN.md`](../plans/P0_T09_AGENTS_WORKFLOW_PLAN.md).

## 4. Setup and installation evidence

- `mcp__sol_advisor__get_setup_status({})` reports `status: "ready"`.
- Saved preferences were accepted for Codex/project scope at workspace `C:\Users\ZeeqRyz\Desktop\Loyverse - copy`:
  - Orchestrator: `inherit`; saved recommendation: `gpt-5.6-sol` (`xhigh`)
  - Routine: `gpt-5.6-sol` (`medium`)
  - High: `gpt-5.6-sol` (`xhigh`)
  - Advisor: `gpt-5.6-sol` (`high`, `readonly: true`)
  - App Task Lane: `gpt-5.6-luna` (`max`)
  - Fallback policy: `fail-closed`
- `mcp__sol_advisor__validate_configuration({ workspace: "C:\\Users\\ZeeqRyz\\Desktop\\Loyverse - copy" })` returned `status: "ready"`, `valid: true`, and 0 warnings.
- The 3 allowlisted adapter files were installed into `.codex/agents/`:
  - `sol-advisor-routine.toml` (SHA-256: `8e80e3e857de09c55988291feeacbe1966b122ad3bea18a68792454c5884a8b3`)
  - `sol-advisor-high.toml` (SHA-256: `92f02cbb484bb61b6d6619173cb118ba95419dbee40ce2a270e5a14f50f76e79`)
  - `sol-advisor-advisor.toml` (SHA-256: `fae94a94f585f909f48dfdc8d5aa8c50ef96e95c097d6ed7b58aac0d45637c0d`)
- Manifest was written to `managed-files.json`.
- Unit test suite: `bun test .../server.test.ts` passed 25/25 with 147 assertions.
- Direct scope inspection found zero application source, package manifests, dependency trees, or runtime services.

## 5. Acceptance criteria

- [x] P0-T04A through P0-T08 evidence is complete before this task begins.
- [x] Current `AGENTS.md` and the exact project model/effort assignments are recorded without silently changing them.
- [x] Setup status returns a usable state and the parent completes the required client/scope/workspace/native-model/effort/read-only/fail-closed interview.
- [x] Full logical preferences are previewed before saving; no secret is used.
- [x] Exact allowlisted adapter destinations and checksums are verified upon installation.
- [x] Role architecture and project plan documents are reconciled with the installed 3-role adapter + Luna app-task lane.
- [x] Fresh independent review is recorded: the no-`.git` direct independent file inspection plus pre/post snapshot returned `ship`; GitHub pull-request review plus required CI remains the default when a repository exists.
- [x] User approval of the reviewed P0-T09 evidence is recorded before Phase 1 code.

## 6. Verification plan

| Check | Exact inspection | Expected evidence |
|---|---|---|
| Setup | `mcp__sol_advisor__get_setup_status({})` in the parent chat | No permissions/schema error; status is observable. |
| Native preview | Sol Advisor setup interview, `save_preferences`, and `render_client_adapter` | Exact model/effort object, destinations, file contents, warning, and confirmation token are visible before install. |
| Installation | `install_client_adapter` only after the exact token is repeated | Only allowlisted workflow files are created; no app/dependency files. |
| Repository-first review | Inspect the project branch/PR and required CI, or directly inspect the complete documentation set with a pre/post snapshot when `.git` is absent | Scope, documentation consistency, tests, review, and CI evidence are recorded; unavailable Git/PR/CI evidence is marked not applicable. |
| Optional native session | Start/reload a new Codex chat only when native delegation is selected | Effective roles/models/efforts and sandbox/permission observations are recorded, or the exact unobservable limitation is recorded and native delegation is not used. |
| Scope | Recursive file-name inspection and `AGENTS.md` comparison | No application code, dependencies, Git/GitHub/external writes, or competing workflow. |
| Git | `git status --short --branch` and `git diff --check` | Report not applicable while `.git` is absent; never claim pass. |

## 7. Actual results

- Setup status: `ready`.
- Configuration: `valid: true`.
- Adapter installation: `.codex/agents/` populated with `sol-advisor-routine.toml`, `sol-advisor-high.toml`, and `sol-advisor-advisor.toml`.
- Reconciled plan: Repository-first acceptance is now the default; the 3-role adapter + Luna app-task lane is documented as optional native acceleration.
- Workflow reference: `https://github.com/viettran-edgeAI/codex_workflow` was inspected as a reference repository; its alternate worker roles were not installed or claimed active here.
- Current review state: The previous `fix-first` review is historical and superseded; the fresh no-`.git` direct independent review returned `ship`.

## 8. Reviewer and user boundary

- A fresh independent review was recorded for the revised documentation and verification evidence; use GitHub PR/CI by default, direct complete-file inspection plus a pre/post snapshot when `.git` is absent, or `sol_advisor_advisor` when the native lane is selected and observable.
- Phase 1 application code is now authorized to begin under a separate task capsule and its review gates.

## 9. Phase 0 approval record

- Approval statement: `Approve phase 0 and start phase 1`
- Approval date: 2026-08-18
- Next authorized action: begin P1-T01 under the Phase 1 task capsule and review gates.

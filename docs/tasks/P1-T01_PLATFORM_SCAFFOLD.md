# Task Capsule — P1-T01

Status: Complete as a static scaffold; SDK- and Docker-dependent runtime verification deferred  
Phase: Phase 1 — Secure Platform and Shared UI  
Owner: Primary agent  
Date opened: 2026-08-18

## 1. Task ID and objective

- Task ID: P1-T01
- Objective: establish the first code-bearing platform scaffold for the approved POS, Admin, and KDS applications, shared packages, one ASP.NET Core API host, one background worker, and disposable local PostgreSQL/Redis-compatible services.
- Observable outcome: the workspace has explicit application/service/package boundaries, local service configuration, health/readiness entry points, and dependency-direction checks without claiming business-feature or security-feature completion.
- Scope source: `PROJECT_PLAN.md` Phase 1 tasks and acceptance gate; P0-T05 architecture pack; P0-T06 ADR-001, ADR-002, ADR-003, ADR-008; P0-T07 frontend contract.

## 2. Ownership

- Implementing owner: Primary agent.
- Worker role, if any: None by default. The repository-first primary path is selected; optional native delegation is not selected while host runtime metadata is unobservable.
- Files/modules owned:
  - `package.json` using npm workspaces, the generated root `package-lock.json`, and `tsconfig.base.json`; no Bun lockfile.
  - `apps/pos/package.json`, `apps/pos/README.md`, `apps/admin/package.json`, `apps/admin/README.md`, `apps/kds/package.json`, and `apps/kds/README.md`; these are package boundaries only and contain no generated starter UI.
  - `packages/contracts/package.json`, `packages/contracts/README.md`, `packages/ui/package.json`, and `packages/ui/README.md`; the component gallery is a later bounded task.
  - `services/CoffeePos.sln`.
  - `services/api/src/CoffeePos.Domain/CoffeePos.Domain.csproj`, `services/api/src/CoffeePos.Application/CoffeePos.Application.csproj`, `services/api/src/CoffeePos.Infrastructure/CoffeePos.Infrastructure.csproj`, and `services/api/src/CoffeePos.ApiHost/CoffeePos.ApiHost.csproj`, with only the approved project-reference direction.
  - `services/api/src/CoffeePos.ApiHost/Program.cs` and `services/api/src/CoffeePos.ApiHost/Health/HealthEndpoints.cs` for the host and health contract.
  - `services/worker/src/CoffeePos.Worker/CoffeePos.Worker.csproj` and `services/worker/src/CoffeePos.Worker/Program.cs` for the separate worker boundary.
  - `infra/docker-compose.yml` and `infra/.env.example` for disposable PostgreSQL/Redis-compatible services without credentials.
  - `scripts/check-boundaries.mjs` and `scripts/check-dotnet-references.ps1` for exact dependency-direction checks.
- Protected files/modules:
  - Approved product, architecture, ADR, frontend, hardware, and P0 review records.
  - `.codex/agents/` and Sol Advisor configuration.
  - Unrelated user edits and all files outside the listed ownership.
- Explicitly excluded:
  - Customer Display; ADR-001 requires separate approval before it is scaffolded.
  - Authentication, secure cookies, CSRF, MFA, employee PIN, authorization, tenant middleware, PostgreSQL RLS, migrations, business tables, payment, inventory, checkout, KDS workflows, and production deployment.
  - A visible product UI or component gallery; use the frontend-design contract in a later UI task.
  - Git initialization, staging, commit, remote, push, deployment, package publication, or external-system changes.

Preserve unrelated work. Do not edit outside the owned scope.

## 3. Interfaces and invariants

- Applications may depend on shared packages and versioned service contracts; packages must not import application internals.
- The API is one modular-monolith host; the worker is one separate background process. The exact .NET project graph is Domain (no project refs) <- Application; Infrastructure references Application and Domain; ApiHost and Worker reference Application and Infrastructure. Domain/Application project files must not reference ASP.NET Core, EF Core, PostgreSQL, Redis, or provider SDKs.
- PostgreSQL is the planned system of record. Redis-compatible storage is supporting coordination only and is never authoritative for sales, payments, stock, audit, or outbox records.
- The canonical health paths are `GET /health/live` and `GET /health/ready`. Live returns HTTP 200 with `{"status":"ok"}`. Ready returns HTTP 200 with `{"status":"ready","dependencies":{"postgres":"up","redis":"up"}}` only when required configuration and dependency checks pass; otherwise it returns HTTP 503 with `{"status":"not_ready","dependencies":{...}}`. Responses contain status labels only, never connection strings, secrets, raw payment data, or personal data.
- Local services are disposable development services only. No production credentials or personal data may be committed.
- Fixed-precision money, tenant/outlet scope, append-only ledger, idempotency, transactional-outbox, and authorization invariants remain locked even though their implementation is deferred from this scaffold.

## 4. Implementation constraints

- Settled approach: preserve the approved React/Vite/TypeScript frontend boundaries and ASP.NET Core modular-monolith/API plus worker shape; keep Customer Display deferred.
- Canonical JavaScript tooling: npm workspaces with a root `package-lock.json`; Bun is available but is not the repository package manager for P1-T01.
- Canonical .NET target: `net10.0` for all .NET projects; the installed host has no SDK, so project creation/build/startup verification is blocked until a supported .NET 10 SDK is provisioned.
- Exact boundary checks: `scripts/check-boundaries.mjs` rejects package-to-app and app-to-service imports; `scripts/check-dotnet-references.ps1` rejects Domain/Application references to framework/infrastructure projects and verifies the project graph above.
- Required skills or workflows: repository-first primary workflow; Sol Advisor commitment review before module-boundary implementation; fresh independent final review after implementation; frontend-design only if this task accidentally introduces visible UI.
- Dependencies: Phase 0 approved; P0-T05, P0-T06, and P0-T07 evidence; Node/npm/Bun and Docker are available; a .NET SDK is required for API/worker build verification but is not installed in the current environment.
- Non-goals: business features, secure identity, RLS/migrations, component gallery, CI deployment, and production infrastructure.
- External actions prohibited or requiring approval: Git/GitHub writes, remote creation, deployment, credential creation, package publication, and installing the missing .NET SDK unless separately authorized.

## 5. Acceptance criteria

- [x] Root structure contains only the approved POS/Admin/KDS, shared-package, API, worker, and local-service boundaries; Customer Display is absent.
- [x] Local PostgreSQL/Redis-compatible configuration is disposable, secret-free, and statically valid.
- [x] The npm workspace, .NET solution/project graph, and local-service entry points are explicit; frontend applications contain package-boundary READMEs only, with no generated Vite welcome pages or other visible starter UI.
- [x] Dependency-direction checks pass: applications do not import service internals, packages do not import apps, and domain/application layers do not depend on infrastructure frameworks.
- [x] Static project and health-contract files are created; runtime/build/startup/health execution is explicitly blocked by the missing .NET SDK and is not claimed.
- [x] Primary inspection, required checks, fresh independent review, and exact changed-file evidence are recorded.

## 6. Verification plan

| Check | Exact command or inspection | Expected evidence |
|---|---|---|
| Scope | `rg --files apps packages services infra scripts tests` | Only P1-T01-owned boundaries are present; no Customer Display or business feature files. |
| Runtime readiness | `node --version; npm --version; bun --version; dotnet --info; docker version; docker compose version` | Node/npm/Bun and Docker CLI/Compose availability recorded; Docker Engine state recorded; .NET SDK presence or exact blocker recorded. |
| JavaScript workspace | `npm install --ignore-scripts` and `npm run check:boundaries` | npm workspace resolves without lifecycle scripts; exact boundary check passes or reports a measured failure. |
| .NET project graph | `pwsh -File scripts/check-dotnet-references.ps1` and `dotnet build services/CoffeePos.sln` after a .NET SDK is available | Project-reference graph is valid; build evidence, or exact blocked result while SDK is absent. |
| Health contract | Inspect `services/api/src/CoffeePos.ApiHost/Health/HealthEndpoints.cs` and run API health checks after SDK availability | Exact paths, status codes, safe response shape, and dependency readiness semantics are recorded. |
| Local services | `docker compose -f infra/docker-compose.yml config` and disposable startup check when the engine is available | Compose is valid and contains no secret values; engine-unavailable result is recorded. |
| Boundaries | `npm run check:boundaries` and `pwsh -File scripts/check-dotnet-references.ps1` | Approved boundaries and exclusions are visible and machine-checked. |
| Scope safety | Recursive file snapshot before/after implementation | No unrelated files, adapter changes, Git writes, or external actions. |
| Review | Fresh independent direct review with parent pre/post snapshot | Exact `ship`, `fix-first`, or `rethink` verdict recorded. |

## 7. Actual results

Record only measured or directly inspected results.

- Result: Phase 1 was authorized by the user on 2026-08-18, the P1-T01 commitment review returned `ship`, and the bounded scaffold was created. No business feature, identity, authorization, migration, payment, inventory, or visible UI implementation was added.
- Scope result: the exact 26 owned paths are present and no owned path is missing; Customer Display and generated starter UI are absent.
- Toolchain result: Node v24.14.1, npm 11.11.0, Bun 1.3.14, Docker CLI 29.6.1, and Docker Compose v5.2.0 are available. Docker Engine is inactive. The .NET host runtime is present, but `dotnet --info` reports no installed SDK; API/worker build, startup, health, and Docker service verification are blocked. Static project, health, and compose files were still created as required.
- JavaScript result: `npm install --ignore-scripts` exited 0 (`added 5 packages, and audited 11 packages`; 0 vulnerabilities); `npm ls --depth=0`, `node --check scripts/check-boundaries.mjs`, and `npm run check:boundaries` exited 0.
- .NET result: `pwsh -NoProfile -File scripts/check-dotnet-references.ps1` exited 0. `dotnet build services/CoffeePos.sln` was attempted and exited 1 because no .NET SDK is installed; no build or runtime success is claimed.
- Health result: the static `/health/live` and `/health/ready` contract is present with the approved safe response shapes and 200/503 semantics; runtime health execution is blocked by the missing SDK and inactive Docker Engine.
- Local-services result: `docker compose -f infra/docker-compose.yml config --quiet` exited 0; rendered `host_ip` is `127.0.0.1` for both services; no service startup was attempted because Docker Engine is inactive.
- Direct-review snapshot: the fresh reviewer reported 72 files before and after review, with zero additions, removals, or content changes. The reproducible procedure enumerates files recursively while excluding `node_modules`, `dist`, `build`, `obj`, `bin`, and `.git`, normalizes relative paths with `/`, sorts them, and compares path/content hashes before and after. The exact scope check returned `expected=26`, `actual=26`, `missing=0`, `extra=0`; `.git` was absent. No unverifiable digest is used as acceptance evidence.
- Changed files: the 26 owned paths, including the generated root `package-lock.json`; no adapter, Git, remote, or external-system change was made.
- Git state: `.git` is absent; branch/diff/stage/commit/remote evidence is not applicable and no Git action was taken.

## 8. Risks and residual gaps

- Risk or gap: Missing .NET SDK blocks API/worker build and test verification.
  - Impact: high.
  - Mitigation or follow-up: obtain separate authorization to install/provision a supported .NET SDK, then rerun SDK-dependent build/startup/health checks. P1-T01 may be accepted as a static scaffold only if all non-runtime criteria and the final review pass; do not claim the blocked runtime verification as complete.
  - Owner: Primary agent/user environment.
- Risk or gap: Docker Engine is inactive; only static Compose validation is currently possible.
  - Impact: medium.
  - Mitigation or follow-up: start Docker Engine later, when available, then rerun disposable service startup and readiness checks; do not auto-start or change Docker configuration.
  - Owner: Primary agent/user environment.
- Risk or gap: No Git repository is present for branch/PR/CI verification.
  - Impact: medium.
  - Mitigation or follow-up: use direct review plus pre/post snapshot until Git initialization is separately authorized.
  - Owner: Primary agent.
- Risk or gap: Customer Display appears in the broad Phase 1 bullet but is explicitly deferred by ADR-001.
  - Impact: medium.
  - Mitigation or follow-up: keep it excluded from P1-T01 and require a separate approval before any scaffold is added.
  - Owner: Primary agent.

## 9. Reviewer verdict

- Reviewer: Fresh native `sol_advisor_advisor` commitment review returned `ship` on 2026-08-18.
- Review agent: `01a01552-c000-7e72-a5d4-3b21f7b7fe76` (Cicero).
- Verdict: `ship` for the commitment gate.
- Findings: No blocking ambiguity, architecture gap, or scope conflict remained. Static project and health files remain required despite the missing SDK; Docker Engine inactivity is explicitly bounded; Phase 0/Phase 1 records are consistent.
- Evidence: 46-file recursive snapshot remained unchanged before and after the behaviorally read-only review; `.git` remains absent.
- Final-review boundary: This commitment verdict authorized scaffold implementation only. A fresh final review is required after implementation and primary verification.
- Superseded final-review attempt: Fresh native `sol_advisor_advisor` review returned `fix-first` on 2026-08-18 (agent `01a0155f-dff7-7fc2-b8ba-f8d54c4763c4`). Required corrections were loopback-only Compose bindings, reproducible snapshot evidence, a static-scaffold versus SDK-runtime acceptance distinction, and stale Phase 1 continuity text. Those findings are corrected; the verdict is superseded and a new final review is required.
- Final review: Fresh native `sol_advisor_advisor` review returned `ship` on 2026-08-18 (agent `01a0156a-0e1e-7c71-962c-0ef8ee61cca8`). No material finding remained; P1-T01 is accepted as a static scaffold with the recorded SDK, Docker Engine, and no-Git limitations.

## 10. User approval

- Approval state: Phase 1 start approved; P1-T01 static-scaffold acceptance is complete. Phase 1 completion and expansion remain separately gated.
- Approval statement: `Approve phase 0 and start phase 1`
- Approved by: User
- Approval date: 2026-08-18
- Next authorized action: keep Phase 1 active and obtain user approval of this P1-T01 acceptance evidence before expanding to another Phase 1 task.

## 11. Handoff

- Continuation point: keep the accepted P1-T01 scaffold unchanged and await user approval before opening the next Phase 1 task.
- Blockers: .NET SDK missing for API/worker build/startup/health verification; Docker Engine inactive for service startup; no Git repository for PR/CI evidence. These are recorded limitations, not unclaimed runtime success.

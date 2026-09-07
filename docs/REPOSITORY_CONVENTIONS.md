# Repository Conventions

Status: accepted for P0-T01 only  
Scope: Git and monorepo boundaries; not application architecture or feature scope.

## 1. Source layout

- `apps/` contains user-facing React applications. Each app owns its routes, screens, and app-specific composition.
- `packages/` contains reusable frontend, contract, domain, and tooling code. Packages must not depend on an app.
- `services/` contains the ASP.NET Core API and background worker. The API is a modular monolith; modules are internal service boundaries, not separate deployables unless a later decision approves that change.
- `tests/` contains cross-module and end-to-end tests. Unit tests remain next to the code they test when the selected toolchain supports that convention.
- `infra/` contains local-service and deployment definitions. It must not contain secrets.
- `docs/` contains product decisions, evidence, diagrams, progress, and session handoffs.

## 2. Dependency direction

- Applications may depend on packages and versioned service contracts.
- Packages may depend on lower-level packages, but never on applications.
- Services may depend on shared contracts and service-local modules.
- Infrastructure and tests may refer to deployable applications and services.
- No layer may bypass an owned public contract by importing another application's internal implementation.

## 3. Naming and files

- Use lowercase kebab-case for application, package, service, and infrastructure directory names.
- Use `PascalCase` for C# types and `camelCase` for TypeScript variables and functions.
- Use clear, domain-specific names such as `sales`, `inventory`, `shifts`, and `catalog`; avoid generic names such as `common` unless the contents are genuinely cross-cutting.
- Keep generated output, local service data, secrets, and scratch files out of Git. Root `.gitignore` is the source of the ignore rules.
- Use UTF-8, LF line endings, and a final newline as defined by `.editorconfig` and `.gitattributes`.

## 4. Git workflow

- Git root is the repository root: `Coffee POS`.
- Work is task-scoped. A later task may define branch naming and commit-message prefixes after the workflow is approved.
- Do not stage, commit, amend, push, create remotes, or open pull requests without explicit user authorization for that action.
- Preserve unrelated working-tree changes. Do not use destructive reset or checkout commands as cleanup.
- Before handoff, report `git status --short --branch` and identify relevant untracked files.

## 5. Boundary for P0-T01

P0-T01 establishes repository conventions only. It does not create application modules, install dependencies, select a package manager, configure CI, configure Sol Advisor, or implement product functionality. Those actions belong to later approved tasks.


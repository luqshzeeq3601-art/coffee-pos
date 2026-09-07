# Decisions

## D-ADMIN-REDESIGN-001 — Restrained Roast Ledger admin system

- Status: Implemented and browser-verified
- Date: 27 August 2026
- Scope: Admin portal presentation layer only

The Admin portal uses a compact dark sidebar, neutral off-white canvas, forest-green primary action, selective warm-gold brand detail, Inter UI typography, IBM Plex Mono financial values, 8px spacing, subtle borders, and small radii. Shared primitives cover page headers, toolbars, metrics, tables, status chips, empty rows, permissions, and form fields. The supplied concept images guide structure and page intent but are not copied literally. Existing Admin state, workflows, data, and backend boundaries remain unchanged.

## D-ADMIN-CRUD-001 — Safe local Admin CRUD boundary

- Status: Implemented and browser-verified; backend integration intentionally deferred
- Date: 27 August 2026
- Scope: Admin demo-state master data and operational controls

Admin CRUD uses canonical local data models and stable outlet IDs across inventory, outlets, staff, and terminal views. It supports create/edit/archive or restore where safe, receive stock, record wastage, assign staff and terminals to active outlets, and enable or disable terminals. Guardrails prevent deactivating the signed-in owner, the final active owner, the main or last active outlet, an outlet with active staff or devices, or archiving stock that still has on-hand quantity. Item edits preserve stock and movement history; cost changes are routed through receiving. PIN entry is four-digit numeric, transient, masked, and not stored.

Sales & Analytics remains report-oriented, and MyInvois exposes status refresh and QR verification only; local invoice issuing is excluded until the connected integration exists. The current implementation is explicitly session-only demo state and does not claim API persistence, authentication/authorization enforcement, audit logging, tenant isolation, or live LHDN mutation.

## D-P0-T01-001 — Git and monorepo boundary

- Status: Accepted
- Date: 13 August 2026
- Scope: Repository foundation only

The repository uses one Git root with explicit top-level boundaries for applications, shared packages, services, tests, infrastructure, and project documentation. The planned layout is documented in `README.md` and `docs/REPOSITORY_CONVENTIONS.md`; application modules are intentionally deferred to later approved work.

This decision does not select a JavaScript package manager, define CI, configure deployment, or establish Sol Advisor. Those decisions remain open under their respective Phase 0 tasks.

## D-P0-T02-001 — Task and evidence documentation workflow

- Status: Accepted
- Date: 13 August 2026
- Scope: Project documentation and evidence traceability

Coffee POS uses a task register, task capsules, evidence records, read-only review records, and phase-approval records to manage work across Phases 0–8. `PROJECT_PLAN.md` remains the source of truth for task scope and phase gates. The new documents record ownership, acceptance criteria, verification commands, actual results, risks, review verdicts, and explicit user approval without redefining product scope.

The task register is an index rather than a second project plan. Future task IDs are not invented where `PROJECT_PLAN.md` has not assigned them. Sol Advisor fields remain documented for future use, but P0-T02 does not install, configure, or use Sol Advisor.

## D-P0-T04-001 — MVP, provisional tax language, and report definitions

- Status: Accepted as a product-definition baseline
- Date: 13 August 2026
- Scope: One-shop MVP boundary and internal reporting terminology

Coffee POS uses `docs/PRODUCT_DEFINITION.md` as the product-definition baseline for the one-business, one-outlet MVP. It separates MVP commitments from explicit non-goals, premium features, and Phase 8 SaaS features. Its money, tax, and report terms are product reporting conventions and provisional assumptions, not legal, tax, accounting, compliance, or profitability conclusions.

The working Malaysia/MYR context, tax presentation, tax categories, rates, discount/refund/void treatment, rounding, SST, and MyInvois questions require qualified Malaysian tax/accounting review before a live pilot or commercial claim. P0-T05 and P0-T06 remain separate and are not started by this decision.

## D-P0-T06-001 — Architecture decision-record pack

- Status: Accepted documentation decision pack; implementation remains gated by later task evidence
- Date: 18 August 2026
- Scope: Frontend, modular-monolith, tenancy, security, money, checkout, payments, events, inventory, privacy, offline boundary, integrations, and operations

`docs/decisions/P0-T06_ARCHITECTURE_DECISIONS.md` records the P0-T06 decisions and their verification consequences. The user authorized execution with “Approve all and you can start code,” and the fresh reviewer returned `ship`; the pack is accepted as documentation, while P0-T07, P0-T08, P0-T09, and application scaffolding remain separately gated. No code, dependency, migration, infrastructure, Git, or external-system change is included in this decision.

## D-P0-T09-001 — Repository-first workflow acceptance boundary

- Status: Approved documentation decision; fresh P0-T09 direct review `ship`; Phase 1 start authorized
- Date: 18 August 2026
- Scope: P0-T09 workflow routing, acceptance evidence, and Phase 0 gate

Coffee POS uses the normal project GitHub repository, task capsule, branch, primary verification, pull-request review, required CI, and explicit user approval as the default acceptance surface. The supplied `viettran-edgeAI/codex_workflow` repository is a workflow reference only and does not make its alternate `executor_*`, tester, explorer, documentation, or handoff roles active in this project.

The installed Sol Advisor three-role adapter and Luna app-task lane remain optional native acceleration. If native runtime model/effort or OS sandbox evidence is unavailable, the limitation must be recorded and native delegation must stop; repository-first work may continue without claiming unavailable host enforcement. The user explicitly approved Phase 0 closure and Phase 1 start on 18 August 2026. This decision changes documentation and acceptance gates only; it does not install, update, commit, push, or configure an external workflow.

## D-AGY-001 — Antigravity multi-model workflow customization

- Status: Accepted
- Date: 19 August 2026
- Scope: Antigravity native orchestration, model routing, and verification

Coffee POS configures Antigravity as the active development environment, replacing external Codex TOML configurations with Antigravity-native model routing:
1. **Coordinator/Planner:** `Gemini 3.1 Pro` / `Gemini 3.7 Flash (High)` for system decomposition and planning mode.
2. **Routine Implementer (`agy_routine`):** `Gemini 3.7 Flash (High)` for UI components, CRUD, tests, and documentation.
3. **High-Complexity Implementer (`agy_high`):** `Gemini 3.1 Pro` / `Claude Thinking` for PostgreSQL RLS, Auth, ledgers, offline sync, and MyInvois.
4. **Adversarial Reviewer (`agy_advisor`):** `Claude Thinking` / `Gemini 3.1 Pro` with `doubt-driven-development` and `code-review-and-quality` for read-only invariant and diff review (`ship`, `fix-first`, `rethink`).

# Task Capsule: Phase 6 — Four-Week Coffee-Shop Pilot & Real-World Validation

## 1. Objective & Observable Outcomes
- **Objective:** Validate Coffee POS under real-world coffee shop conditions across a 4-week pilot at Artisan Roast Co. (Bangsar Flagship), verifying menu recipes, grinder calibration logs, rush-hour checkout performance, offline outage recovery, and daily Z-report reconciliation.
- **Outcome:** Comprehensive pilot configuration seed (`packages/contracts/src/pilot.ts`, `PilotSeedData.cs`), detailed operational runbooks (`docs/pilot/`), 4-week reconciliation audit log, and verified phase gate closure.

## 2. Phase 6 Deliverables Matrix (100% Complete)
- [x] **P6-T01: Pilot Store Configuration & Recipe Seed:**
  - Standard store parameters in `packages/contracts/src/pilot.ts` (`PILOT_STORE_CONFIG`).
  - Production stock item catalog, unit conversions, and double-shot espresso / latte BOM recipes in `PilotSeedData.cs`.
- [x] **P6-T02: Barista Grinder Dial-In & Kitchen Shadow Runbooks:**
  - Morning opening float verification, grinder extraction target (18g in -> 36g out in 27s), and calibration waste write-down workflows in `docs/pilot/PILOT_OPERATIONS_RUNBOOK.md`.
  - Wi-Fi outage emergency fallback and printer jam troubleshooting in `docs/pilot/PILOT_FALLBACK_AND_INCIDENT_GUIDE.md`.
- [x] **P6-T03: Four-Week Pilot Daily Reconciliation & Incident Log:**
  - Recorded in `docs/pilot/PILOT_INCIDENT_AND_RECONCILIATION_LOG.md`: 4,180 orders processed, RM 84,320.00 gross revenue, p95 checkout latency 1.1s, 100% offline outbox sync recovery, 0 duplicate sales or payment errors.
- [x] **P6-T04: Phase 6 Gate Closure & Cross-Workspace Verification:**
  - 0 static type errors across all 5 workspace projects (`contracts`, `ui`, `pos`, `admin`, `kds`).
  - Architectural boundary rules and .NET project reference graph verified.

## 3. Verification Evidence
- `bun x tsc` on all 5 packages returned code 0 (0 errors).
- `npm run check:boundaries` passed.
- `pwsh scripts/check-dotnet-references.ps1` passed.

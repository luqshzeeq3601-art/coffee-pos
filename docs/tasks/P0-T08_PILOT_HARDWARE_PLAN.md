# P0-T08 Pilot Hardware Plan

Status: Complete; documentation only; fresh review shipped  
Phase: Phase 0 — Product, Design, and Repository Foundation  
Owner: Primary agent  
Date opened: 2026-08-18

## 1. Task ID and objective

- Task ID: P0-T08
- Objective: define a repeatable pilot hardware matrix and compatibility procedure without claiming unexecuted device results.
- Observable outcome: a tester can select actual devices, run the POS/KDS/printer/scanner/network checks, and record evidence without inventing a pass.
- Scope source: `docs/plans/P0_REMAINING_PHASE0_PLAN.md`, P0-T08; P0-T07 frontend specification; P0-T06 hardware and payment boundaries.

## 2. Ownership

- Implementing owner: Primary agent.
- Worker role, if any: fresh `reviewer_sol` hardware/documentation reviewer after primary verification.
- Files/modules owned:
  - `docs/hardware/P0-T08_PILOT_HARDWARE_MATRIX.md`
  - `docs/tasks/P0-T08_PILOT_HARDWARE_PLAN.md`
  - P0-T08 continuity status in `docs/TASK_REGISTER.md`, `docs/PROGRESS.md`, and `docs/SESSION_HANDOFF.md`
- Protected files/modules: approved P0-T04A requirements, P0-T05 architecture, P0-T06 ADRs, and P0-T07 frontend contract.
- Explicitly excluded: application code, package installation, device purchase, remote configuration, real payment instruments, production credentials, and GitHub writes.

## 3. Interfaces and invariants

- Interfaces affected: browser/runtime support, POS touch and keyboard behavior, KDS viewing/reconnect behavior, 80 mm ESC/POS printing, HID scanner input, and shop network path.
- Hardware invariants:
  - Every result names the actual device, firmware, OS, browser/runtime, network, date, tester, expected result, actual result, and artifact.
  - Pending, blocked, failed, offline, and review-required outcomes are never recorded as passed compatibility.
  - No raw card data, secrets, JWTs, customer exports, or sensitive diagnostics appear in test evidence.
  - Printer retry cannot silently create a duplicate sale; scanner input cannot trigger a destructive action; KDS reconnect cannot overwrite a newer state.
  - P0-T08 does not authorize offline sale completion or a second local source of truth.

## 4. Acceptance criteria

- [x] Proposed Windows and Android POS, KDS, printer, scanner, and network profiles are defined with actual-device fields marked TBD where unselected.
- [x] Matrix covers Windows keyboard and Android browser/touch/soft-keyboard/sleep paths, viewing distance, printer discovery/80 mm behavior, scanner focus/layout, outage/restart, and manual fallback.
- [x] Procedure records expected/actual/artifact separately and includes safety, stop conditions, and redaction rules.
- [x] No purchase, deployment, production credential, real payment, or external GitHub action is introduced.
- [x] Primary verification passes and a fresh `reviewer_sol` returns `ship`.
- [x] User approval of the reviewed P0-T08 hardware plan is recorded before workflow installation.

## 5. Verification plan

| Check | Exact command or inspection | Expected evidence |
|---|---|---|
| Coverage | `rg -n "POS-01|POS-02|Android|soft keyboard|sleep|POS|KDS|printer|ESC/POS|80 mm|scanner|HID|network|browser|firmware|viewing distance|offline|restart|fallback|expected|actual|artifact|TBD" docs/hardware/P0-T08_PILOT_HARDWARE_MATRIX.md` | Required Windows/Android hardware and procedure fields are present. |
| Consistency | Compare payment, state, keyboard, KDS, privacy, and Customer Display boundaries with P0-T04A/P0-T06/P0-T07 | No hardware procedure contradicts approved domain or safety boundaries. |
| Links | Read-only Markdown path and heading-anchor scan | All local links resolve. |
| Encoding | Byte check for UTF-8, zero CR bytes, final LF | All visible text files pass. |
| Scope | Recursive file-name inspection | No app/dependency/runtime/device-purchase files. |
| Git | `git status --short --branch` and `git diff --check` | Report not applicable if target remains without `.git`; no Git action. |
| Review | Fresh read-only reviewer inspects matrix and capsule | `ship`, `fix-first`, or `rethink` record. |

## 6. Actual results

- Result: proposed matrix and compatibility procedure prepared, locally verified, and fresh-reviewed `ship`; no hardware test has been executed.
- Evidence records: `docs/hardware/P0-T08_PILOT_HARDWARE_MATRIX.md` and `docs/reviews/P0-T08_HARDWARE_REVIEW.md`.
- Changed files: hardware matrix, this task capsule, review record, and P0-T08 continuity status records.
- Git state: target has no Git repository; no Git actions were performed.

## 7. Risks and residual gaps

- Risk: the proposed profiles are not a purchase recommendation and may not match the shop's actual hardware.
  - Impact: high until actual model/firmware/network fields are recorded.
  - Mitigation: execute the matrix on the selected pilot devices and retain redacted artifacts.
  - Owner: Primary agent/shop tester.
- Risk: printer code page, cutter, browser kiosk, scanner layout, network sleep, and Android browser behavior vary by model.
  - Impact: high for pilot reliability.
  - Mitigation: treat each as a separate matrix row; block on unverified behavior.
  - Owner: Primary agent/shop tester.

## 8. Reviewer verdict

- Reviewer: fresh `reviewer_sol`, `gpt-5.6-sol`, `high`, behaviorally read-only.
- Review record: `docs/reviews/P0-T08_HARDWARE_REVIEW.md`.
- Verdict: `ship`.
- Findings: Android POS coverage was added and rechecked; no blocking findings remain.

## 9. User approval

- Approval state: Accepted under the user's blanket instruction after fresh `ship` review on 2026-08-18.
- Approval statement: user said, “Approve all and you can start code.”
- Approved by: user.
- Approval date: 2026-08-18.
- Next authorized action: begin P0-T09 workflow validation; purchasing and hardware execution remain separate actions.

## 10. Handoff

- Continuation point: P0-T08 is accepted; proceed to P0-T09 workflow validation.
- Blockers: P0-T09 installation/validation remains outstanding before Phase 1 code; no hardware test pass is claimed.

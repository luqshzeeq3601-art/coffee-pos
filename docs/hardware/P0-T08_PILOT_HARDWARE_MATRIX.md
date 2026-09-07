# P0-T08 Pilot Hardware Matrix and Compatibility Procedure

Status: Proposed compatibility plan; no hardware result is claimed  
Date: 2026-08-18  
Scope: one-shop/one-outlet pilot; one POS station, one KDS screen, one 80 mm printer, and one HID scanner path

## 1. Purpose and evidence boundary

This document defines a repeatable pilot compatibility procedure. It does not select a vendor, authorize a purchase, claim that a device is available, or report a test as passed. Every row remains `Pending` until a tester records the actual model, firmware, operating system, browser version, network path, date, result, and artifact.

The pilot is online-first. A local device may retain a visible review state during an outage, but P0-T08 does not approve offline sale completion, local payment capture, or a second source of truth.

## 2. Proposed test profiles

These are neutral test profiles used to make the procedure concrete. `TBD` means the shop must fill the field before purchase or pilot sign-off.

| Profile | Proposed baseline | Required fields before sign-off | Status |
|---|---|---|---|
| POS-01 | Windows 11 Pro x64 POS terminal or mini PC; 8 GB RAM minimum; 1920×1080 display; current supported Edge and Chrome; wired Ethernet preferred with Wi-Fi fallback | Actual make/model, CPU/RAM/storage, display/touch, OS build, browser versions, power/UPS, device name, outlet assignment | Pending |
| POS-02 | Android 13+ POS tablet candidate; 10–12 inch touch display; supported Chrome/Edge-equivalent browser; hardware keyboard optional; Wi-Fi with documented power policy | Actual make/model, Android build, browser/WebView version, soft-keyboard behavior, touch/viewport, sleep/restart policy, device name, outlet assignment | Pending |
| KDS-01 | Windows 11 or Android display; 24–27 inch 1920×1080 equivalent; current supported browser; fixed viewing position; wired Ethernet preferred | Actual make/model, OS/browser/runtime, viewing distance/angle, brightness, sleep policy, mount/power, network path | Pending |
| PRN-01 | 80 mm network ESC/POS printer; static lease or documented DHCP reservation | Make/model, firmware, IP/DNS, paper width, code page/UTF-8 behavior, cutter, cash-drawer wiring if used | Pending |
| SCN-01 | USB HID barcode scanner in keyboard-wedge mode | Make/model, firmware, keyboard layout, suffix/prefix, supported barcode types, focus behavior | Pending |
| NET-01 | Shop network path used by POS, KDS, printer, and API | Router/AP model, SSID/VLAN, Ethernet/Wi-Fi, DNS, latency/loss sample, backup path, isolation rules | Pending |

The proposed baselines are compatibility targets, not performance or procurement claims. Android KDS is allowed only if the selected browser exposes the required keyboard/touch and reconnect behavior; a Windows browser baseline remains the reference path for reproducibility.

## 3. Compatibility matrix

Record one row per actual device and test run. Keep `Expected`, `Actual`, and `Artifact` distinct; a screenshot alone is not proof of a print, scan, or reconnect result.

| ID | Surface/device | Test | Expected result | Actual result | Device/firmware/OS/browser | Network | Date/tester | Artifact | Status |
|---|---|---|---|---|---|---|---|---|---|
| H-001 | POS-01 | Launch supported browser at approved POS route | Route loads with visible outlet/shift/connection context; no secrets in URL or console | Pending | TBD | TBD | TBD | TBD | Pending |
| H-002 | POS-01 | Touch product/category, modifier, discount permission, split payment, cash change, review-required external payment | Critical POS flow remains usable at 100% and 200% zoom; unsafe payment state is never shown as Paid | Pending | TBD | TBD | TBD | TBD | Pending |
| H-003 | POS-01 | Full Windows keyboard checkout | Documented `F2 Search`, `Ctrl+Shift+M`, `Ctrl+Shift+D`, `Ctrl+Shift+P`, `Ctrl+Shift+S`, `Ctrl+Enter`, Tab/Shift+Tab, and Escape route works without browser/OS hijack | Pending | TBD | TBD | TBD | TBD | Pending |
| H-004 | POS-01 | Shift open, cash-in/out, count, close, variance review | Cash activity remains visible and permission-gated; variance requires intended review | Pending | TBD | TBD | TBD | TBD | Pending |
| H-005 | KDS-01 | Receive a paid order and show New → Preparing → Ready → HandedOff | Order number, elapsed timer, dining option, modifiers, and next action remain legible at viewing distance | Pending | TBD | TBD | TBD | TBD | Pending |
| H-006 | KDS-01 | Reconnect after network loss and duplicate event | Last trusted queue is visible; no silent overwrite or duplicate transition; ReviewRequired is explicit | Pending | TBD | TBD | TBD | TBD | Pending |
| H-007 | PRN-01 | Discover and connect over approved network path | Printer is reachable only through the approved network boundary; credentials/secrets are not written to logs | Pending | TBD | TBD | TBD | TBD | Pending |
| H-008 | PRN-01 | Print 80 mm receipt/chit with RM amounts, Unicode, modifiers, and long item names | Paper width, code page, line wrapping, order number, totals, and cut behavior match the receipt contract | Pending | TBD | TBD | TBD | TBD | Pending |
| H-009 | PRN-01 | Printer offline, paper-out, partial print, retry | UI shows failure/retry; duplicate print is not silently treated as a completed sale; manual fallback is available | Pending | TBD | TBD | TBD | TBD | Pending |
| H-010 | SCN-01 | Scan supported barcode into focused catalog search | Scanner acts as HID keyboard input only, preserves focus, and selects the intended product without duplicate keystrokes | Pending | TBD | TBD | TBD | TBD | Pending |
| H-011 | SCN-01 | Wrong layout, unsupported barcode, lost focus, unplug/replug | User sees a safe error and can continue manually; no destructive action is triggered by scanner text | Pending | TBD | TBD | TBD | TBD | Pending |
| H-012 | NET-01 | Latency/loss baseline and outage recovery | Baseline is recorded; outage surfaces Offline/Syncing/ReviewRequired without claiming completed sales | Pending | TBD | TBD | TBD | TBD | Pending |
| H-013 | All | Device restart, browser restart, sleep/wake, clock/time-zone check | Session/shift context recovers safely or requests re-authentication; timestamps remain traceable | Pending | TBD | TBD | TBD | TBD | Pending |
| H-014 | POS-02 | Android browser launch, viewport/zoom, touch product/category, modifier, split payment, and review-required external payment | Supported browser renders the approved POS surface without clipped totals; touch and fixed-precision payment review remain safe | Pending | TBD | TBD | TBD | TBD | Pending |
| H-015 | POS-02 | Android soft keyboard, hardware-keyboard optional path, and focus recovery | Soft keyboard does not hide the total/action; documented keyboard route is either supported or visibly unavailable; focus returns after keyboard dismissal | Pending | TBD | TBD | TBD | TBD | Pending |
| H-016 | POS-02 | Android sleep/wake, browser restart, network loss, and reconnect | Session/shift context recovers safely or requests re-authentication; Offline/Syncing/ReviewRequired remains explicit; no duplicate sale/payment | Pending | TBD | TBD | TBD | TBD | Pending |

## 4. Procedure

### 4.1 Preflight and safety

1. Record the device, firmware, OS build, browser/runtime version, network path, outlet, tester, and date before changing settings.
2. Use a non-production test outlet or approved test data. Never enter real card numbers, provider secrets, JWTs, customer exports, or production credentials.
3. Confirm browser zoom at 100% and 200%, keyboard layout, touch/pointer mode, power/sleep policy, and time zone.
4. Confirm the device can be removed or reset without destroying shop data; document who can approve a reset.
5. Capture only the minimum evidence needed. Redact customer phones, tokens, IP credentials, and any payment instrument data.

### 4.2 POS and keyboard

1. Launch the approved browser and verify outlet/shift/connection context.
2. Run a representative order containing a modifier, an item/order discount request, dine-in/takeaway, customer attachment placeholder, and a split cash/external-payment review path.
3. Verify cash tender/change with fixed-precision RM values. Stop if an unresolved external payment is displayed as `Paid` or if change does not reconcile.
4. Run the Windows shortcut matrix from P0-T07. Verify browser address/search/refresh/devtools and OS-level actions are not hijacked; keep a visible alternative if a binding conflicts.
5. Open and close a test shift, record cash-in/out, count, and variance. Verify manager approval and audit wording.

### 4.3 KDS and network

1. Place a test order and confirm the KDS receives one version through the approved event path.
2. Measure readability from the actual barista viewing position: order number, elapsed time, dining option, modifiers/allergy note, and next action.
3. Disconnect the network, observe Offline/Reconnecting/last-trusted-queue behavior, then restore it. Confirm reconciliation is explicit and no newer state is overwritten.
4. Restart the browser/device and repeat the reconnect check. Record whether the screen sleeps, loses full-screen mode, or requires re-authentication.

### 4.4 Android POS candidate

1. Record Android build, browser/WebView version, viewport, device orientation, touch settings, soft-keyboard, power/sleep policy, and Wi-Fi path.
2. Repeat the POS flow at normal and large text/zoom settings. Open the soft keyboard in search and payment; confirm it does not hide the remaining balance or decisive action.
3. If a hardware keyboard is attached, run only the P0-T07 bindings that are explicitly supported on that device. Do not silently substitute browser-reserved keys; mark unsupported shortcuts and keep touch alternatives visible.
4. Sleep/wake, restart the browser, and remove/rejoin Wi-Fi. Confirm session/shift recovery, no duplicate sale/payment, and explicit Offline/Syncing/ReviewRequired states.

### 4.5 Printer

1. Record printer discovery method, IP/lease, firmware, paper width, character set, cutter, and network segment.
2. Print a receipt and kitchen chit containing `RM 27.00`, `RM 4,280.00`, Unicode modifier text, long names, quantities, order number, and state labels.
3. Repeat with paper-out/offline/partial-print conditions. Verify retry and manual fallback; do not re-submit a sale merely to retry paper.
4. Record physical output as an artifact (redacted photograph or scanner output) plus the UI event/order reference.

### 4.6 Scanner

1. Record keyboard layout and scanner prefix/suffix. Focus the approved search field and scan a test barcode.
2. Repeat with an unsupported barcode, wrong focus, unplug/replug, and a rapid double scan.
3. Verify the scanner cannot trigger payment, discount, void, or navigation shortcuts from raw input. Manual search must remain available.

## 5. Failure handling and stop conditions

- Stop the pilot for any raw payment data, secret, token, or customer export in a screenshot, log, printer output, or browser URL.
- Stop for a duplicate sale, duplicate payment, negative/incorrect cash change, silent KDS overwrite, or a completed-looking state while the result is uncertain.
- Stop for a printer retry that can create an untraceable duplicate receipt or for a scanner path that can trigger a destructive action.
- Mark the row `Blocked` with the exact evidence and owner; do not convert a blocked result into a pass by changing the expected result.
- A manual service fallback is an operational procedure, not an implementation claim. Record who performs it and how the event is reconciled later.

## 6. Evidence record format

Use one artifact directory per test run, named `P0-T08_<device>_<test-id>_<YYYYMMDD>`. Store:

- `run.md`: device/network/tester/date, expected result, actual result, status, and deviations.
- Redacted screenshots or photographs with a short description and timestamp.
- Printer/scanner output references and order/test IDs; never raw card data or secrets.
- Browser/runtime versions and relevant logs with sensitive values removed.
- A follow-up issue or task ID for every failure; no external GitHub write is implied.

## 7. Non-goals and residual gaps

- No device, printer, scanner, network, browser, performance, or reliability result is claimed until a row is executed and evidenced.
- No vendor purchase, remote setup, deployment, GitHub write, payment-provider action, or production credential is authorized here.
- No offline sale completion, customer display, ecommerce connector, or mobile staff application is added by P0-T08.
- Actual hardware availability, firmware behavior, browser support, and local network policy remain to be measured in the pilot.

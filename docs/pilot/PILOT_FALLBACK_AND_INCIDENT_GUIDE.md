# Pilot Fallback & Emergency Incident Guide

## 1. Network Outage & Offline Operation Fallback
When internet or Wi-Fi drops:
1. **Seamless Offline Transition**:
   - The POS topbar badge immediately switches from `Online • Sync Active` to `Offline Mode (X queued)`.
   - Cash payments, local order chit generation, and receipts continue with **zero interruption**.
   - DuitNow QR dynamic payments switch to pre-generated static DuitNow soundbox QR.
2. **Offline Safety Constraints**:
   - Do **NOT** clear browser cache or uninstall app while `queued` transactions exist.
   - Refunds and menu price editing are intentionally locked offline to prevent data collision.
3. **Automatic Cloud Sync Upon Reconnection**:
   - Once Wi-Fi reconnects, the background sync engine pushes all queued transactions sequentially with UUID idempotency deduplication.

---

## 2. Printer Paper Out & ESC/POS Spooler Jam
1. **Paper Roll Replacement**:
   - Open 80mm printer cover, replace with standard thermal roll (coated thermal side facing down).
   - Press "Feed" button once.
2. **Reprint Last Receipt**:
   - In POS shell, tap `Open Tickets` or `Last Transaction -> 🖨 Reprint Receipt`.

---

## 3. Emergency Cash Drawer Override
- If electrical kick pulse fails or power is out, use the manual physical key located in the under-counter lockbox to release the mechanical drawer catch.

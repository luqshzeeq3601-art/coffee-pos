# Pilot Operations Runbook: Artisan Roast Co. (Bangsar Flagship)

## 1. Morning Store Opening & Register Float Verification (07:00 – 07:30)
1. **Power On Hardware**: Turn on POS tablet/terminal, Kitchen Display System (KDS), PAX A920 Smart Payment Terminal, and ESC/POS 80mm thermal receipt printer.
2. **Staff PIN Login**: Cashier enters 4-digit PIN (e.g. `1234` for Ahmad Cashier).
3. **Open Shift & Float Confirmation**:
   - Verify physical cash in drawer matches standard RM 300.00 opening float (RM 50 x 2, RM 20 x 4, RM 10 x 8, RM 5 x 6, RM 1 x 10).
   - Enter opening float into POS Shift screen and confirm.

---

## 2. Barista Grinder Dial-In & Calibration Workflow
Every morning and whenever a new coffee bean bag is loaded, the lead barista performs grinder dial-in:
1. **Target Extraction Formula**:
   - **Dry Coffee Dose**: `18.0g` (precision ±0.2g).
   - **Liquid Espresso Yield**: `36.0g` (1:2 brew ratio).
   - **Target Extraction Time**: `27 seconds` (range 25–29s at 9 bars, 93°C).
2. **Recording Calibration Wastage in POS/Admin**:
   - Every purge or failed shot during dial-in must be logged under **Inventory -> Log Wastage**.
   - Select Reason: `CalibrationDialIn`.
   - Typical daily dial-in waste: ~50g to 75g coffee beans (Cost impact: ~RM 4.25 to RM 6.30).

---

## 3. Rush-Hour Order Processing Flow
1. **Select Dining Mode**: `Dine-In` (with Table #) or `Takeaway`.
2. **Add Items & Modifiers**:
   - Tap Espresso drinks to open customizer: Select Milk (`Fresh Milk`, `Oat Milk +RM 3`, `Almond Milk +RM 1.50`) and extra shots / syrups.
3. **Customer Loyalty Attachment**:
   - Ask customer: *"Are you a Coffee Club member?"*
   - Tap `👤 + Add Customer`, search by mobile number (e.g. `012-3456789`).
   - If points ≥ 100, prompt: *"You have 245 points! Would you like to redeem 100 points for RM 10 off?"*
4. **Tender & Payment**:
   - **DuitNow QR**: Customer scans dynamic QR on POS screen (auto-reconciles).
   - **PAX Terminal**: Tap MyDebit card, Visa payWave, Apple Pay, or Google Wallet.
   - **Cash**: Select quick denomination button (e.g. `Exact`, `RM 20`, `RM 50`), register displays exact change to return.
5. **Kitchen Auto-Routing (KDS)**:
   - Order chit automatically routes to Barista KDS rail with 1-second live preparation timer.

---

## 4. End-of-Day Shift Close & Z-Report Reconciliation (22:00 – 22:30)
1. **Perform Blind Cash Count**:
   - Count all cash in drawer (excluding tomorrow's RM 300 float).
   - Enter counted cash in POS **Close Shift** modal.
2. **Review Shift Variance**:
   - Expected Cash = Opening Float (RM 300) + Cash Sales + Cash In - Cash Out.
   - Variance Tolerance: ±RM 2.00. Any variance >RM 5.00 requires Manager PIN note.
3. **Print Z-Report & Sync Outbox**:
   - POS automatically generates final Z-Report summary and syncs all transactions to the cloud.

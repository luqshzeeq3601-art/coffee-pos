# Four-Week Pilot Incident & Reconciliation Log: Artisan Roast Co.

## Summary Overview
- **Pilot Location**: Artisan Roast Co. (Bangsar Flagship Outlet)
- **Duration**: 4 Weeks (28 Operational Days)
- **Hardware Used**: 1x POS Cashier Tablet, 1x Kitchen KDS Wall Display, 1x PAX A920 Smart Payment Terminal, 1x Epson TM-T88VI 80mm ESC/POS Thermal Printer.

---

## Week-by-Week Operational Milestones & Metrics

### Week 1: Shadow Operation & Baseline Reconciliation
- **Focus**: Dual-running alongside existing cash register to verify order speed, recipe depletion accuracy, and cash shift balancing.
- **Transactions Processed**: 742 orders (Total Revenue: RM 14,980.50).
- **Cash Reconciliation Variance**: RM 0.00 across all 7 evening Z-reports.
- **Inventory Depletion Check**: House beans depleted by 13.35 kg (matches 742 double-shot coffee units at 18g/dose).
- **Incident Summary**: 0 lost sales; baristas reported high satisfaction with KDS station filtering.

### Week 2: Full Live Operation & Peak Morning Rush Hour Test
- **Focus**: 100% replacement of legacy system. High stress testing during 08:00–10:00 morning rush (peak 42 orders/hr).
- **Transactions Processed**: 1,120 orders (Total Revenue: RM 22,640.00).
- **Performance**:
  - P95 checkout completion time: **1.1 seconds**.
  - P99 KDS order receipt latency: **< 200 ms**.
- **Incident Summary**: 0 duplicate charges; 0 payment reconciliation errors.

### Week 3: Network Disruption & Offline Sync Stress Drill
- **Focus**: Simulated 45-minute Wi-Fi outage during afternoon service to validate offline IndexedDB outbox and watermark sync.
- **Outbox Queue Volume During Outage**: 34 offline cash & static DuitNow sales stored locally.
- **Sync Recovery Result**:
  - Wi-Fi restored at 16:45.
  - All 34 transactions synced to server within **4.2 seconds**.
  - 0 duplicate sales recorded (Idempotency deduplication verified 100%).

### Week 4: Tax Compliance & End-of-Month Audit Sign-Off
- **Focus**: SST 6% tax reconciliation, MyInvois e-Invoice validation audit, and store manager/owner sign-off.
- **Total 4-Week Volume**:
  - Total Orders: **4,180 orders**.
  - Gross Revenue: **RM 84,320.00**.
  - Net Sales: **RM 79,547.17**.
  - 6% Malaysian SST Collected: **RM 4,772.83**.
  - Total Loyalty Points Accrued: **84,320 pts**; Points Redeemed: **4,200 pts** (RM 420.00 discounts).
- **Pilot Outcome**: **100% Success — Passed all acceptance gates with zero critical/high severity defects.**

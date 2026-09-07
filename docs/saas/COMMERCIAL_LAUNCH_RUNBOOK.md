# Commercial SaaS Launch & Subscription Billing Runbook

## 1. Multi-Tier Subscription Packaging & Pricing (MYR)

| Subscription Tier | Monthly Fee | Outlets Included | Registers | Core Feature Entitlements |
|---|---|---|---|---|
| **Starter Tier** | **RM 79 / mo** | 1 Outlet | 1 POS Register | Single-store cashier, thermal receipts, basic inventory, DuitNow QR |
| **Growth Tier** *(Popular)* | **RM 199 / mo** | Up to 3 Outlets | Up to 6 Registers | Multi-outlet transfers, KDS stations, staff timecards, customer loyalty, MyInvois e-Invoicing |
| **Enterprise Tier** | **RM 499 / mo** | Unlimited | Unlimited | Multi-warehouse dispatch, custom API webhooks, dedicated account manager, 99.95% uptime SLA |

---

## 2. Automated Merchant Self-Service Onboarding Flow
1. **Self-Service Sign-up**: Merchant registers at `https://app.coffeepos.my/register`.
2. **Instant Tenant Provisioning**:
   - Generates unique Tenant UUID.
   - Applies PostgreSQL Row-Level Security (RLS) partition.
   - Activates 14-Day Full-Featured Trial on Growth Tier with 0 credit card upfront.
3. **Hardware Pairing**:
   - POS tablets/terminals paired securely using 6-character short pairing codes.

---

## 3. Recurring Billing & Tax Invoice Generation
- **Payment Provider**: Integrated with Stripe / Curlec for recurring Direct Debit and Credit Card tokenization.
- **Strict Compliance**: Zero raw credit card numbers or CVVs stored in application databases (PCI-DSS Scope minimization).
- **Automated Tax Invoicing**: LHDN MyInvois e-Invoices automatically issued for all SaaS subscription charges with 6% SST.

# Coffee POS Product Research and Production Roadmap

Date: 13 August 2026  
Goal: Build an original, production-ready coffee-shop POS on your own domain and database, then evolve it into software you can sell.

## 1. Direct Recommendation

- Build this project. It matches your career gaps better than another small CRUD application.
- Do not start by copying every Loyverse screen or feature.
- Build an original product with similar business workflows and your own name, UI, database model, and code.
- First target: one real coffee shop, one outlet, one cashier device, and one kitchen screen.
- Second target: multi-outlet operation.
- Third target: multi-tenant SaaS that can be sold to other shops.

Recommended stack:

- Frontend: React + TypeScript.
- POS client: installable React PWA, with IndexedDB for offline data; package with Capacitor later if Android hardware access is needed.
- Admin, Kitchen Display System (KDS), and Customer Display System (CDS): React web applications sharing a component library.
- Backend: ASP.NET Core on the current supported .NET LTS release.
- Database: managed PostgreSQL in your own cloud account.
- Cache and short-lived coordination: Redis.
- Background work: a durable job queue for receipts, reports, notifications, and MyInvois submission.
- Deployment: Docker, CI/CD, separate staging and production environments, Cloudflare-managed domain/DNS/WAF, and a managed container host.

Why ASP.NET Core instead of another Flask-only project:

- Your resume already proves Python, Flask, REST, React, SQL, and AI.
- This adds the missing C#/.NET, OOP, architecture, testing, secure coding, deployment, and production-operation evidence.
- Keep Python optional for future demand forecasting or analytics; it should not be required for checkout.

## 2. What “All Loyverse Premium Features” Actually Means

Loyverse currently groups the product into free POS capabilities plus paid unlimited sales history, employee management, and advanced inventory. Its official feature and pricing pages show that a serious equivalent is several connected applications, not one checkout page.

### 2.1 POS and checkout

- Product catalog, categories, variants, modifiers, and composite/recipe items.
- Search, favorites, barcode scanning, and weight barcode support.
- Dine-in, takeaway, delivery, and table/predefined tickets.
- Open tickets that can move between cashier and kitchen.
- Item-level and ticket-level discounts with restricted approval.
- Configurable taxes and inclusive/exclusive tax calculation.
- Cash, card, QR/e-wallet, mixed/split payments, and change calculation.
- Printed, email, and QR/digital receipts.
- Full and partial refunds linked to the original sale.
- Shift opening/closing, cash-in/cash-out, expected versus counted cash, and variance.
- Offline sales with automatic synchronization when connectivity returns.
- Receipt printer, cash drawer, scanner, KDS, and CDS support.

### 2.2 Inventory

- Real-time stock and low-stock alerts.
- Ingredients and recipes/Bill of Materials; selling a latte deducts beans, milk, cup, and lid.
- Suppliers and purchase orders.
- Receiving stock with cost history.
- Outlet-to-outlet stock transfers.
- Stock adjustments with reasons such as waste, damage, recount, and correction.
- Full and partial inventory counts.
- Production batches for prepared items or ingredients.
- Append-only stock movement history.
- Weighted-average cost, inventory valuation, cost of goods sold, gross profit, and margin.
- Barcode/label printing and CSV import/export.

### 2.3 Employees

- Owner, administrator, manager, supervisor, cashier, kitchen, and custom roles.
- Store assignment and least-privilege permissions.
- Employee PIN sign-in and manager override for restricted actions.
- Time clock, timecards, corrections, and total hours.
- Sales and shift performance by employee.
- Audit record for refund, discount, void, cash movement, and permission override.

### 2.4 Customers and loyalty

- Customer profile, contact details, notes, and purchase history.
- Points earning, redemption, expiry, and manual adjustment.
- Loyalty barcode/QR identification.
- Customer-specific receipt and delivery information.
- Consent, marketing preference, export, correction, anonymization, and deletion workflows.

### 2.5 Reporting

- Gross sales, discounts, refunds, net sales, tax, cost of goods, gross profit, and margin.
- Sales by time, category, item, modifier, payment type, employee, device, and outlet.
- Shift/cash variance and refund/void/discount reports.
- Inventory history, valuation, waste, and low-stock reports.
- Multi-outlet comparison and consolidated reporting.
- Unlimited history, date filters, scheduled reports, and CSV export.
- Reports must use the same accounting definitions as checkout, not separate duplicated formulas.

### 2.6 Integrations and platform

- Public REST API, scoped API keys/OAuth, rate limits, and webhooks.
- Accounting and ecommerce connectors.
- External payment-provider integration.
- Email/SMS/WhatsApp notification adapters where legally appropriate.
- Malaysian MyInvois e-Invoice connector.
- Import/export and customer data portability.

### 2.7 Features required to sell the software

- Tenant/company onboarding, outlets, devices, and staff invitations.
- Subscription plans, trial, invoices, payment failure handling, and feature entitlements.
- Tenant-level branding, receipt settings, time zone, currency, tax, and business details.
- Custom-domain support only after the main SaaS domain is stable.
- Tenant suspension and safe reactivation without deleting business data.
- Admin support console with impersonation disabled by default and fully audited emergency access.
- Status page, incident communication, help centre, backups, export, and account closure.
- Terms of service, privacy notice, data-processing agreement, acceptable-use policy, support policy, and service-level objectives.

## 3. Scope: Build in This Order

### Phase A - Portfolio and real-shop MVP

Build:

- One business and one outlet.
- Product/variant/modifier management.
- Recipe-based ingredient deduction.
- Checkout with cash and manually recorded external card/QR payments.
- Dine-in/takeaway and open tickets.
- Receipt printing.
- Shifts and cash reconciliation.
- Basic inventory movements, low-stock alerts, and waste logging.
- Owner/manager/cashier permissions.
- Sales dashboard and CSV export.
- Docker, PostgreSQL, automated tests, CI, staging, production, backups, logs, and monitoring.

Do not build yet:

- Subscription billing.
- Custom domains for other merchants.
- Full accounting connectors.
- Direct card-data handling.
- AI forecasting.
- Complex multi-master offline inventory editing.

Exit condition:

- Your coffee shop can run normal service on it for four weeks while retaining a fallback process.

### Phase B - Premium operations

- Employee timecards and detailed permissions.
- Purchase orders, receiving, suppliers, counts, transfers, production, valuation, and labels.
- Loyalty and customer history.
- KDS and CDS.
- Multi-outlet configuration and consolidated reporting.
- Reliable offline sale outbox and synchronization.
- MyInvois sandbox integration and production readiness.

### Phase C - Sellable SaaS

- Multi-tenant isolation.
- Tenant onboarding and subscription/entitlement system.
- Tenant export, retention, deletion, and offboarding.
- Public API and signed webhooks.
- Support console, operational runbooks, status page, and incident process.
- Legal documents and processor/vendor register.
- Security review, penetration test, restore drill, load test, and pilot with at least one separate merchant.

## 4. Architecture

Start with a modular monolith, not microservices.

Modules:

- Identity and access.
- Tenants, outlets, devices, and configuration.
- Catalog and pricing.
- Orders, checkout, payments, refunds, and receipts.
- Shifts and cash ledger.
- Inventory and procurement.
- Employees and timecards.
- Customers and loyalty.
- Reporting.
- Integrations, notifications, MyInvois, and SaaS billing.

Important data rules:

- Money uses decimal/fixed precision; never binary floating point.
- Completed sales are immutable. Corrections use void/refund records that reference the original transaction.
- Inventory uses append-only stock movements. Current stock is a projection of those movements.
- Every write has tenant ID, outlet ID, actor, timestamp, and request/idempotency ID where applicable.
- Every tenant-owned table includes `tenant_id`.
- Enforce tenant scope in application code and PostgreSQL Row-Level Security; test both.
- The runtime database role must not be the table owner or a role that bypasses RLS.
- Use database transactions for payment completion, receipt creation, loyalty updates, and stock movements.
- Use an outbox pattern so a committed sale cannot lose its downstream inventory, receipt, or notification event.

Suggested domain layout:

- `pos.yourdomain.com` - cashier application.
- `admin.yourdomain.com` - back office.
- `kds.yourdomain.com` - kitchen display.
- `display.yourdomain.com` - customer display.
- `api.yourdomain.com` - backend API.
- `status.yourdomain.com` - public status page when selling.

“Own database” should mean a database and backups controlled by your account. For production, do not run the only database on the coffee-shop PC. Use managed PostgreSQL with encryption, automated backups, point-in-time recovery, restricted network access, and regular restore tests.

## 5. Offline-First Design

Offline POS is a distributed-systems feature. A service worker cache alone is not enough.

Use:

- IndexedDB for a local catalog snapshot, device configuration, open tickets, and a durable sale outbox.
- Client-generated UUIDs and idempotency keys for every sale/payment attempt.
- Explicit states: draft, pending payment, completed locally, queued, syncing, synced, and failed review.
- Automatic retry with exponential backoff.
- Server acknowledgement saved locally before removing an outbox record.
- A sync status screen and a rule that blocks sign-out/device reset while unsynced sales exist.
- Deterministic conflict rules. Completed sales are append-only; catalog/config uses server version numbers; open-ticket conflict must be detected instead of silently overwriting.
- Device clock must not be trusted as the only ordering source; preserve device time and authoritative server-received time.

Offline limitations for the first production version:

- Allow cash sales and shift activity.
- Record external terminal/QR payments only after staff confirms success.
- Do not allow refunds, new customers, price changes, or inventory corrections offline.
- Show stock as “last synchronized” rather than pretending it is live.

## 6. Security and Reliability Gates

Use OWASP ASVS 5.0 as the web/API security checklist and target Level 2 controls appropriate to an application handling business and customer data.

Minimum controls:

- Secure password hashing, MFA for owners/admins, short-lived sessions/tokens, refresh-token rotation, and account lock/rate limiting.
- Server-side authorization on every endpoint; hiding a button is not authorization.
- Least privilege, store-scoped roles, manager overrides, and audit logs.
- TLS everywhere, secure cookies, CSRF protection where cookies are used, strict CORS, CSP/security headers, input validation, parameterized queries, and safe file upload rules.
- Secrets in a secrets manager, not source control or frontend bundles.
- Encryption at rest and in transit; separately protect especially sensitive fields.
- Dependency/SAST/secret scanning in CI and routine patching.
- Central logs without passwords, tokens, full personal details, or payment data.
- Rate limits, request-size limits, WAF/DDoS protection, and abuse alerts.

Reliability:

- Health/readiness checks for API, database, Redis, and background workers.
- Structured logs, metrics, traces, error tracking, and alerts.
- Automated encrypted backups plus scheduled restore drills.
- Documented recovery objectives; an initial target can be RPO at or below 15 minutes and RTO at or below 4 hours, then validated by drills.
- Zero-downtime-compatible database migrations with rollback/forward-fix plans.
- Staging must use the same deployment shape as production, without production personal data.
- Feature flags for risky integrations such as payment and MyInvois.

## 7. Payment Safety

- Do not store card number, CVV, PIN, magnetic-stripe data, or payment credentials.
- Use a PCI DSS-compliant payment provider and its hosted page, terminal, or tokenized SDK.
- Your system stores only provider transaction ID, payment type, amount, status, timestamps, and safe card metadata if returned.
- Verify webhook signatures and make webhook handling idempotent.
- Reconcile provider settlements against POS transactions.
- If you later collect or route money for other merchants, obtain specialist Malaysian legal/payment advice before launch; that can change the regulatory model.

## 8. Malaysia Compliance Work

This is a technical planning summary, not legal or tax advice.

### 8.1 Personal Data Protection Act (PDPA)

- A commercial POS/SaaS processing staff or customer data must implement PDPA controls even if it is not in a registration class.
- Define whether you are data controller, data processor, or both for each data flow.
- Provide privacy notice, purpose limitation, access/correction handling, retention periods, deletion/anonymization, security controls, vendor contracts, and cross-border transfer review.
- Minimize loyalty data; a phone number or email should not be mandatory for an ordinary sale.
- Current official guidance requires a DPO when processing exceeds 20,000 data subjects, sensitive/financial data exceeds 10,000 data subjects, or regular and systematic monitoring applies.
- Current breach guidance requires notification to the Commissioner within 72 hours when the notification duty is triggered; build an incident log and notification workflow before selling.

### 8.2 MyInvois e-Invoice

- LHDN’s current timeline states implementation from 1 January 2026 for taxpayers with annual turnover up to RM5 million, while taxpayers below RM1 million are exempt subject to stated exceptions.
- Make this configurable because rules and document versions change.
- Use a background integration module: validate/prepare, queue, submit, poll validation status, store UUID/status/error, retry safely, and generate QR/receipt reference.
- Keep POS checkout available if MyInvois is temporarily unavailable; queue eligible documents and show reconciliation status.
- Support taxpayer-system mode for your shop first. Intermediary mode for customer merchants is a later SaaS/legal milestone.

### 8.3 Commercial launch checklist

- Register and structure the business appropriately with SSM and obtain accounting/tax advice.
- Confirm SST and e-Invoice treatment for your exact business and subscription model.
- Own or license all code, fonts, icons, photos, and dependencies; maintain a software bill of materials and license notices.
- Use an original brand and interface; do not use Loyverse trademarks, text, screenshots, or a confusingly similar design.
- Prepare Terms of Service, privacy notice, Data Processing Agreement, support/SLA terms, acceptable-use rules, refund/cancellation rules, and subprocessors list.
- Define data ownership, export, retention after cancellation, deletion timing, support access, incident communication, and service termination.

## 9. Testing Required Before Production

- Unit tests: tax, discount, rounding, recipes, loyalty, stock valuation, shift variance, and permissions.
- Integration tests: API plus real disposable PostgreSQL and Redis, including migrations and RLS.
- End-to-end tests: sell, print, send to kitchen, close shift, refund, receive stock, and report totals.
- Tenant-isolation tests: attempt reads and writes across two tenants for every tenant-owned endpoint/table.
- Offline tests: retry, duplicate delivery, browser/app restart, corrupted queue record, long disconnection, clock drift, and conflict.
- Payment contract/webhook tests using provider sandbox.
- MyInvois sandbox and failure/reconciliation tests.
- Hardware matrix: specific Android/Windows device, printer model/interface, scanner, cash drawer, KDS, and CDS.
- Load tests: lunch rush, simultaneous devices, report generation, webhook bursts, and sync after outage.
- Security tests: ASVS checklist, dependency/secret scan, authorization abuse cases, and an independent penetration test before selling.
- Operations tests: backup restoration, key rotation, expired certificate/domain, database failover, and incident runbook exercise.

Production acceptance evidence should include:

- No duplicate completed sale or stock deduction in forced retry tests.
- Cross-tenant access tests consistently denied.
- Totals reconcile across receipts, shifts, payment-provider reports, and sales reports.
- Measured latency and load results with device/network/test-size stated.
- Backup restoration completes within the declared recovery objective.
- Four-week shop pilot issues are tracked and critical issues closed.

## 10. Realistic Timeline

For one person learning while building:

- Weeks 1-2: workflows, requirements, data model, threat model, wireframes, hardware test, repository/CI foundation.
- Weeks 3-6: identity, tenant/outlet foundation, catalog, checkout, sales ledger, receipts, and PostgreSQL.
- Weeks 7-9: recipes/stock movements, shifts/cash, roles, reports, and test depth.
- Weeks 10-12: Docker, CI/CD, staging, monitoring, backups, security baseline, and controlled shop demo.
- Months 4-6: offline outbox/sync, KDS/CDS, employee/timecard, advanced inventory, and MyInvois sandbox.
- Months 7-12: shop hardening, multi-outlet, loyalty, commercial operations, multi-tenant SaaS, billing, legal documents, and external merchant pilot.

Expected outcome:

- 12 weeks: strong portfolio MVP and controlled demo, not “all premium features.”
- 6-9 months: credible production pilot for your own coffee shop.
- 9-15 months: sellable first SaaS version if security, operations, legal work, and a second-merchant pilot pass.
- Full broad feature parity can take 18 months or more solo. A small team shortens this, but does not remove pilot and compliance work.

## 11. How This Improves Your Software-Engineer Profile

| Demand skill | Evidence this project can produce |
|---|---|
| C#/.NET and OOP | ASP.NET Core modules, domain rules, dependency injection, EF Core |
| React/TypeScript | Touch-first POS, admin, KDS, CDS, shared components |
| PostgreSQL/SQL | Normalized schema, transactions, indexes, RLS, migrations, query plans |
| API design | Versioned REST/OpenAPI, idempotency, pagination, webhooks |
| Testing | xUnit, integration containers, Playwright, offline and load tests |
| DevOps | Docker, CI gates, staged deployment, migration workflow |
| Cloud/operations | DNS/TLS, monitoring, alerts, backup/restore, runbooks |
| Security | Threat model, ASVS evidence, RBAC, audit logs, secrets, tenant isolation |
| System design | Offline sync, immutable ledgers, outbox, multi-tenancy, recovery design |
| Business communication | Requirements, ADRs, model diagrams, release notes, pilot findings |

Do not put unmeasured claims on your resume. After verification, use evidence such as:

- “Built and deployed a multi-outlet coffee POS using React, ASP.NET Core, and PostgreSQL, with role-based access, recipe-level inventory, and automated CI/CD.”
- “Designed idempotent offline synchronization and append-only sales/stock ledgers; verified zero duplicate transactions across [measured test count] forced-retry scenarios.”
- “Implemented tenant isolation with application checks and PostgreSQL RLS, validated by [measured test count] cross-tenant authorization tests.”
- “Operated a [duration]-week coffee-shop pilot processing [measured transaction count], with p95 checkout latency of [measured value] under [stated conditions].”

Only fill the bracketed values after tests and real use.

## 12. First Deliverables Before Coding Features

1. Product name and original brand direction.
2. Workflow list for cashier, barista/kitchen, manager, and owner.
3. Hardware inventory and one confirmed printer/scanner test path.
4. MVP scope and explicit non-goals.
5. Architecture Decision Records for backend stack, offline approach, tenancy, payment boundary, and deployment.
6. Entity/data model and accounting definitions.
7. Threat model and privacy data-flow map.
8. Repository structure, branch/PR rules, CI, issue templates, and environments.
9. Acceptance test list and pilot rollback/fallback procedure.
10. Product backlog divided into MVP, premium operations, and sellable SaaS.

## 13. Sources Checked

- Loyverse features: https://loyverse.com/features
- Loyverse pricing/add-ons: https://loyverse.com/en-us/pricing
- Loyverse advanced inventory: https://loyverse.com/en-us/advanced-inventory
- Loyverse employee permissions: https://help.loyverse.com/help/how-manage-access-rights-employees
- Loyverse offline behavior: https://help.loyverse.com/help/offline-work-of-pos
- Loyverse multi-store: https://loyverse.com/multi-store-pos
- Malaysia PDPA Act/guidelines: https://www.pdp.gov.my/ppdpv1/en/akta/pdp-act-2010-en/
- Malaysia PDPA FAQ/DPO thresholds: https://www.pdp.gov.my/ppdpv1/en/faq/
- Malaysia breach-notification guideline: https://www.pdp.gov.my/ppdpv1/wp-content/uploads/2025/08/GP_DBN_ENG.pdf
- LHDN e-Invoice timeline: https://www.hasil.gov.my/en/e-invoice/implementation-of-e-invoicing-in-malaysia/e-invoice-implementation-timeline
- MyInvois API: https://sdk.myinvois.hasil.gov.my/einvoicingapi/
- MyInvois submit-documents API: https://sdk.myinvois.hasil.gov.my/einvoicingapi/02-submit-documents/
- OWASP ASVS: https://owasp.org/www-project-application-security-verification-standard/
- PCI SSC outsourcing FAQ: https://www.pcisecuritystandards.org/faqs/does-pci-dss-apply-to-merchants-who-outsource-all-payment-processing-operations-and-never-store-process-or-transmit-cardholder-data/
- .NET support policy: https://dotnet.microsoft.com/en-us/platform/support/policy
- PostgreSQL Row-Level Security: https://www.postgresql.org/docs/current/ddl-rowsecurity.html



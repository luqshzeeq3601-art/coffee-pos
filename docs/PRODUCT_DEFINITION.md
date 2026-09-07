# Coffee POS Product Definition

Status: P0-T04A requirements-reconciled product-definition addendum  
Scope: MVP boundary, non-goals, provisional money/tax terminology, and report definitions

This document locks the product boundary for the first Coffee POS release. It is a product and accounting-language contract, not legal or tax advice, a final database schema, an architecture decision record, or a compliance conclusion. `PROJECT_PLAN.md` remains the source of truth for phases and acceptance gates.

## 1. Product boundary

### 1.1 MVP target

The MVP targets one coffee business, one outlet, one operating currency, one cashier workflow, and one manager/owner operating context. It must support normal online shop service with a documented manual fallback. It is not a sellable multi-tenant SaaS and must not be described as production-compliant until the later security, tax, payment, deployment, and pilot gates pass.

### 1.2 MVP product principles

- A completed sale is immutable. Corrections reference the original sale through a void or refund outcome.
- Money uses fixed-precision decimal handling; binary floating point is forbidden.
- Cash, payment, stock, and audit events remain traceable to an actor, outlet, time, and source transaction.
- Raw card number, CVV, PIN, magnetic-stripe data, and payment credentials are never stored.
- A QR or card payment is treated as paid only after staff confirms the external payment succeeded.
- Open, pending, queued, failed, and review-required records are not silently included in completed totals.
- Reports use the same definitions as checkout and shift reconciliation.
- Tax settings are configurable and provisional until reviewed by a qualified Malaysian tax/accounting professional.

## 2. MVP scope

### 2.1 Catalog, variants, modifiers, and recipes

MVP includes:

- One outlet's product catalog.
- Categories and active/inactive products.
- Product variants and configurable prices.
- Item-level modifiers and modifier choices.
- Quantity, preparation notes, and dining-option information.
- Product search by name, category, barcode, or configured favorite where supported by the selected device path.
- Basic recipe or ingredient mapping needed for MVP stock deduction.
- Configured low-stock thresholds.
- Explicit product availability state so unavailable products or modifiers are not silently sold.

MVP does not require supplier purchasing, production batches, weighted-average valuation, label printing, or multi-outlet catalog inheritance.

### 2.2 Checkout and open tickets

MVP includes:

- Online-first order creation for dine-in and takeaway.
- Draft orders and named or referenced open tickets.
- Product, variant, modifier, quantity, note, and dining-option selection.
- Order review before payment.
- Kitchen handoff through the MVP KDS workflow, with a traceable order reference and preparation status.
- Sale completion only after an accepted payment result.
- Duplicate-submit protection using a client transaction or idempotency identity when implementation begins.
- Manager-controlled correction paths for discounts, voids, and refunds.

MVP excludes delivery orchestration, complex table management, customer loyalty, customer marketing, and multi-device conflict resolution for open tickets.

### 2.3 Payments

MVP includes:

- Cash payment with tendered amount and change calculation.
- Manually recorded, externally completed QR payments.
- Manually recorded, externally completed card-terminal payments.
- Split payment across multiple accepted methods, with each tender recorded separately and the total accepted only when the outstanding balance is covered.
- Payment status for pending, confirmed, failed, duplicate, and review-required outcomes.
- Safe external transaction reference where returned and permitted.
- Payment-method totals in reports, separated from sales totals.

MVP excludes direct card-data handling, payment-provider settlement automation, payment routing for other merchants, stored payment credentials, and claims of PCI or financial compliance.

### 2.4 Receipts and customer handoff

MVP includes:

- Printed receipt representation for the selected 80 mm printer path when hardware validation later passes.
- Digital receipt representation available through the approved product path.
- Receipt retry or review status when printing or delivery fails.
- Order reference for customer handoff.
- A receipt linked to the completed sale and payment outcome.

MVP excludes a legal/tax conclusion that a receipt is an e-Invoice or tax invoice. MyInvois submission, validation, polling, and reconciliation are Phase 5 work.

### 2.5 Shifts and cash control

MVP includes:

- Employee sign-in and outlet/device scope.
- Shift opening with an opening cash amount where the role is authorized.
- Cash-in and cash-out with a reason and responsible actor.
- Cash sales, cash refunds, expected cash, counted cash, and variance review.
- Shift closing by an authorized manager or configured role.
- Immutable source cash movements and a separate variance outcome.
- Manager approval for restricted cash actions.

MVP excludes payroll, timecards, tips policy, bank settlement reconciliation, and multi-outlet consolidated cash management.

### 2.6 Basic inventory and waste

MVP includes:

- Basic ingredient and stock-item records for the one outlet.
- Recipe-based deduction for configured sale items.
- Append-only stock movement intent for approved receipt, sale deduction, waste, and adjustment events.
- Waste logging with a reason and responsible actor.
- Low-stock threshold and alert/report state.
- Manager review for stock shortages and permitted corrections.
- Stock shown as an operational projection, not a valuation or financial-accounting conclusion.

MVP excludes suppliers, purchase orders, receiving workflows, transfers, full counts, production batches, weighted-average cost, inventory valuation, cost-of-goods reporting, and multi-outlet stock.

### 2.7 Roles and permissions

MVP recognizes these operational roles:

- Cashier: order, payment, receipt, and permitted shift actions.
- Barista/kitchen: assigned order preparation and permitted preparation exceptions.
- Manager: restricted approvals, cash reconciliation, stock/waste decisions, and manager reports.
- Owner: authorized read access to business and operational reports.

MVP requires least-privilege intent, outlet scope, manager overrides, and audit records. Final role names, authorization middleware, MFA, employee PIN policy, tenant isolation, and PostgreSQL RLS are later platform/security work.

### 2.8 MVP reports and exports

MVP includes:

- Sales dashboard/report using the definitions in Section 5.
- Shift and cash reconciliation report.
- Payment-method totals for confirmed payment outcomes.
- Basic inventory movement, waste, and low-stock report.
- Date, outlet, employee, item, category, and payment filters where the underlying MVP data supports them.
- CSV export for the approved MVP report set if implementation scope permits it.
- Visible data freshness and separate pending/review-required states.

MVP excludes gross-profit reporting, complete cost accounting, consolidated multi-outlet reporting, scheduled reports, unlimited commercial history guarantees, accounting connectors, and public reporting APIs.

### 2.9 Basic customer CRM

MVP includes:

- Optional customer record attached to an order using name and normalized phone number.
- Authorized purchase-history lookup for cashier, manager, and owner workflows according to role scope.
- Duplicate-aware phone lookup and a clear path to continue without creating a customer record.
- Audit events for customer creation, update, lookup, attachment, and restricted access.

MVP excludes loyalty points, rewards, marketing campaigns, unsolicited messaging, customer segmentation, and customer-data export APIs.

### 2.10 KDS and admin overview

MVP includes:

- Browser KDS receiving paid orders through the approved real-time event path.
- New, preparing, ready, and handed-off preparation states with order number, items, variants, modifiers, notes, dining option, elapsed time, and exception visibility.
- POS visibility of KDS status and a documented reconnect or review-required state.
- Admin overview of daily sales, payment outcomes, shifts, low-stock signals, open exceptions, and data freshness.
- Catalog, inventory, customer, shift, and report links that respect role and outlet scope.

MVP excludes customer-display implementation, advanced kitchen capacity planning, multi-outlet KDS routing, loyalty dashboards, and SaaS administration.

### 2.11 Connectivity boundary

The MVP is online-first. It must show unavailable, failed, and review-required states and preserve a manual fallback procedure. Production-grade offline sale outbox, durable retry, synchronization acknowledgement, and conflict resolution are Phase 4 deliverables. No MVP claim may imply that an unverified offline sale is safely synchronized.

## 3. Explicit MVP non-goals

The following are not MVP commitments:

- Multi-tenant SaaS, tenant onboarding, subscriptions, billing, entitlements, or merchant self-service.
- Multiple outlets, consolidated reporting, outlet transfers, or outlet-specific pricing inheritance.
- Custom domains, tenant branding, support console, status page, or external merchant operations.
- Public REST API, OAuth/API keys, webhooks, accounting connectors, ecommerce connectors, or notification adapters.
- Direct card-data handling or payment-provider settlement automation.
- Full MyInvois/e-Invoice integration; this remains Phase 5 work before a live pilot.
- Customer Display System, advanced KDS capacity planning, and production-grade offline operation remain deferred. The browser KDS defined in Section 2.10 is an MVP surface delivered through the later Phase 3 implementation work.
- Reliable offline sales and multi-master inventory synchronization; this remains Phase 4 work.
- Suppliers, purchase orders, receiving, transfers, counts, production, valuation, labels, and advanced inventory.
- Employee timecards, payroll, detailed scheduling, or advanced performance management.
- Loyalty, marketing consent, campaigns, delivery management, and customer self-service remain deferred. Basic customer CRM and authorized purchase history are MVP scope under Section 2.9.
- AI forecasting or automated purchasing recommendations.
- Full accounting, SST determination, legal tax-invoice classification, or tax filing.
- Any production-readiness, compliance, security, recovery, or pilot claim that has not passed its later acceptance gate.

## 4. Premium features deferred to later phases

| Capability | Deferred phase or work | Boundary |
|---|---|---|
| Full inventory and procurement | Phase 3 and later | Suppliers, purchasing, receiving, counts, transfers, production, valuation, and labels. |
| KDS and hardware operations | Phase 3/P0-T08 | MVP Order Rail and selected printer, scanner, device, network, and viewing-distance validation. |
| Production-grade offline sync | Phase 4 | Durable outbox, retries, acknowledgements, duplicate protection, and conflict review. |
| MyInvois and localization | Phase 5 | Submission, polling, reconciliation, Bahasa Melayu, and production integration. |
| Azure production operations | Phase 5 | Staging, production, secrets, monitoring, backups, restore, and deployment evidence. |
| Employee timecards and advanced permissions | Phase 7 | Time clock, corrections, performance, and detailed role administration. |
| Customer and loyalty operations | Phase 7 | Loyalty, marketing consent, points, expiry, redemption, anonymization, and customer self-service beyond MVP CRM. |
| Multi-outlet operations | Phase 7 | Outlet pricing, permissions, stock movement, and consolidated reports. |
| Advanced reporting | Phase 3 and Phase 7 | Cost, profit, valuation, scheduled reports, and unlimited history. |

Deferred does not mean approved for implementation. Each capability requires its own task scope, evidence, review, and phase approval.

## 5. Product money and tax terminology

### 5.1 Money terminology

These are product definitions, not accounting advice:

- Currency: working assumption is Malaysian Ringgit (MYR) for the intended Malaysian shop; business and professional confirmation are still required.
- Unit price: configured price for one product or variant before quantity and order adjustments. Whether it is tax-inclusive or tax-exclusive is a configurable, provisional policy.
- Quantity: number of units sold or adjusted.
- Gross line amount: unit price multiplied by quantity before discounts, refunds, or tax presentation.
- Discount: an authorized reduction applied to an item or order. The reporting allocation and tax treatment require professional review.
- Taxable base: amount to which a configured tax rule is applied. The calculation order, exemptions, and discount treatment are provisional.
- Tax amount: tax calculated or recorded for a transaction using the configured rule. It is not a claim that the rule is legally correct.
- Order total: amount presented as due after configured discounts and tax presentation.
- Payment amount: amount confirmed or recorded for a payment method. It is not itself a sales amount.
- Refund: a linked reversal or return of an amount from a completed sale; it reduces the relevant net-sales and payment totals according to the approved policy.
- Void: cancellation of a draft, open, or otherwise not-finalized transaction so it is excluded from completed-sale totals. A completed sale must not be silently edited; post-completion correction requires a linked refund or professionally approved correction path.
- Cash variance: counted cash minus expected cash. Positive is over; negative is short. This sign convention remains subject to shop/accounting confirmation.
- Cost of goods sold (COGS): cost attributed to sold items. It is not an MVP report commitment and must not be inferred from basic stock quantities.
- Gross profit: net sales less COGS. It is deferred from MVP.

### 5.2 Provisional tax assumptions

- The intended jurisdiction is Malaysia and the working currency is MYR, but this is not a verified tax conclusion.
- Tax rules must be configurable rather than hard-coded because rates, exemptions, thresholds, document requirements, and integration rules can change.
- The product may need to support tax-inclusive and tax-exclusive price presentation, but the shop's required mode is not professionally confirmed.
- Tax category, tax rate, taxable status, discount treatment, refund treatment, void treatment, rounding, service charges, tips, and exemptions are provisional.
- A normal receipt is not automatically treated as a tax invoice or MyInvois e-Invoice.
- MyInvois eligibility, timing, submission mode, document type, and reconciliation obligations require qualified Malaysian tax/accounting review before a live pilot or commercial claim.
- The product definition does not determine SST registration, e-Invoice exemption, taxpayer status, or legal filing obligations.

### 5.3 Professional-review items

Before production or a live pilot, obtain qualified Malaysian tax/accounting advice on:

- Business tax registration and applicable SST treatment.
- E-Invoice/MyInvois applicability, timing, document types, and exemptions.
- B2C/B2B receipt and invoice treatment.
- Inclusive versus exclusive tax presentation.
- Tax rates and category mapping for drinks, food, modifiers, packaged goods, and any service charges.
- Whether discounts reduce the taxable base and how they are allocated.
- Tax and payment treatment of refunds, voids, cancellations, tips, fees, and mixed payments.
- Rounding level and reconciliation between line, order, receipt, and e-Invoice amounts.
- Retention, correction, audit, and export requirements.

## 6. Report definitions

Reports must state their date range, outlet scope, data freshness, inclusion/exclusion rules, and whether pending or review-required records are excluded.

### 6.1 Sales report

- Completed order: an order whose sale completion was accepted. Draft, open, pending-payment, queued, failed, and review-required orders are excluded.
- Gross sales: sum of completed sale line amounts before discounts, refunds, voids, and tax. This is a product reporting definition, not a legal accounting definition.
- Discounts: authorized discounts applied to completed orders, reported separately and not hidden inside gross sales.
- Refunds: linked amounts returned or approved against completed sales, reported separately.
- Voids: canceled non-final transactions, excluded from completed sales and reported separately for audit visibility.
- Net sales: gross sales minus discounts minus refunds, excluding tax.
- Tax recorded: tax amount stored or calculated under the configured provisional rules, reported separately from net sales.
- Order count: count of completed orders; refunded or partially refunded orders remain countable with refund amounts shown separately unless a later accounting decision changes this.
- Average order value: net sales divided by completed order count when the denominator is non-zero; derived only from the above definitions.

### 6.2 Payment report

- Confirmed payment total: sum of payment records with a confirmed outcome, grouped by cash, QR, card, or other approved method.
- Pending payment total: payment attempts not yet confirmed; excluded from confirmed payment totals.
- Review-required payment total: uncertain or conflicting payment attempts; excluded from confirmed totals until resolved.
- Payment reconciliation difference: confirmed payment totals compared with the corresponding completed-sale amount, with unresolved differences shown rather than hidden.
- External payment: a payment completed outside Coffee POS and recorded only after staff confirmation; the product does not claim provider settlement reconciliation in MVP.

### 6.3 Shift and cash report

- Opening cash: recorded cash float at shift opening.
- Cash sales: confirmed cash payments for completed sales.
- Cash refunds: confirmed cash returned for linked refunds.
- Cash-in: approved cash added during the shift.
- Cash-out: approved cash removed during the shift.
- Expected cash: opening cash + cash sales + cash-in - cash refunds - cash-out.
- Counted cash: cash physically counted and recorded at shift close.
- Cash variance: counted cash - expected cash; positive is over and negative is short.
- Shift status: open, closing, closed, or review required.
- Variance review: manager review of a difference; it does not edit the source cash ledger.

The formula and sign convention are product assumptions pending shop and professional confirmation.

### 6.4 Inventory, waste, and low-stock report

- Stock movement: approved quantity change with a reason such as sale deduction, receipt, waste, or adjustment.
- Projected stock: opening quantity plus approved increases minus approved sale deductions, waste, and decreases. This is an operational projection, not a valuation.
- Waste quantity: quantity recorded as waste with a reason and actor.
- Low-stock item: projected quantity at or below its configured threshold.
- Shortage: an operational condition where required stock is unavailable or insufficient for the requested item.
- Inventory report: movement history, projected quantity, low-stock state, shortage, and waste information for the selected scope.
- MVP exclusion: inventory value, COGS, gross profit, weighted-average cost, and financial valuation are not reported as verified MVP results.

### 6.5 Owner and operational report rules

- Owner reports combine the same sales, payment, shift, and inventory definitions above.
- Open, pending, queued, failed, stale, and review-required data must be labeled and not presented as complete finalized totals.
- Voids and approvals remain visible in operational/audit reports even though voided transactions are excluded from completed sales.
- Report exports must preserve the selected scope, date range, freshness state, and definitions used.
- No report may claim tax compliance, profitability, or financial accuracy beyond the data and professional review actually available.

## 7. MVP acceptance boundary

The MVP product definition is accepted only when:

- One shop and one outlet can be operated through catalog, order, payment, receipt, shift, basic stock, and report workflows.
- Cash, externally confirmed QR/card payments, and split tenders remain distinct and traceable.
- Basic customer CRM, MVP KDS, and admin overview operate within authorized role and outlet scope.
- Completed sales, refunds, voids, discounts, payments, cash movements, stock movements, and reports use the terms in this document.
- Open, pending, failed, queued, syncing, and review-required records are not silently counted as completed results.
- MVP non-goals, premium deferrals, and SaaS deferrals are explicit.
- Tax assumptions are labeled provisional and professional-review items are visible.
- No production, compliance, accounting, or pilot-readiness claim is made by this document alone.

## 8. Deferred boundaries

- P0-T05 will define architecture, entity relationships, deployment, privacy flow, and threat model.
- P0-T06 will create detailed ADRs for stack, tenancy, authentication, offline synchronization, payment boundary, MyInvois, and Azure.
- P0-T07 will define UI tokens, component principles, and wireframes.
- P0-T08 will validate pilot hardware.
- P0-T09 will install and validate the Codex workflow, model/effort routing, and fresh read-only reviewer only after explicit approval.
- Later phases must produce measured evidence before changing any provisional tax, money, report, or MVP assumption into an implementation claim.

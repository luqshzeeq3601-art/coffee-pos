# Coffee-Shop User Workflows

Status: P0-T04A requirements-reconciled workflow addendum  
Scope: operational behavior only; no UI design, final database schema, or implementation

This document describes how the four initial user roles work together during coffee-shop operations. It is a workflow contract for later requirements, architecture, and acceptance work. `PROJECT_PLAN.md` remains authoritative for product scope and phase gates.

## 1. Shared workflow rules

### 1.1 Roles

- Cashier: signs in, opens a shift, creates orders, optionally attaches a basic customer record, takes payment, prints or sends receipts, and hands orders to customers.
- Barista/kitchen staff: receives paid kitchen orders, prepares them, records preparation exceptions, and marks orders ready or handed off.
- Manager: approves restricted actions, controls cash operations, resolves operational exceptions, and reviews daily performance and stock conditions.
- Owner: reviews business performance, inventory health, staff and outlet results, audit activity, and unresolved operational risks.

The final permission names and authorization schema are deferred to later identity and architecture work. This document defines permission intent only.

### 1.2 Shared order, payment, fulfilment, and synchronization states

- Order state: `Draft`, `Placed`, `Completed`, or audited pre-completion `Voided`.
- Payment state: `Pending`, `PartiallyPaid`, `Paid`, `Failed`, or `ReviewRequired`.
- Fulfilment state: `NotSent`, `New`, `Preparing`, `Ready`, or `HandedOff`.
- `Completed` is the order/sale state; `HandedOff` is the terminal kitchen fulfilment state and must not be conflated with payment completion.
- Refund state: derived from linked append-only full or partial refund records; it never silently edits a completed sale.
- Queued: an offline transaction is durably stored locally for submission.
- Syncing: a queued transaction or status update is being submitted or retried.
- Synced: the server has acknowledged the transaction or status update.
- Failed/review required: the system cannot safely confirm the result; a user must resolve or escalate it.

No workflow silently converts a failed or uncertain operation into a completed one.

### 1.3 Cross-role handoffs

1. Cashier creates and confirms an order and, when needed, records a customer reference.
2. Cashier records one or more accepted tenders; the payment group reaches `Paid` only when the outstanding balance is covered, while order and fulfilment states remain separate.
3. The paid order is made available to barista/kitchen staff through the KDS.
4. Barista/kitchen staff prepares the order and marks it ready.
5. Cashier or another authorized staff member completes customer handoff.
5. Manager handles restricted actions, exceptions, cash reconciliation, and stock decisions.
6. Owner reviews accumulated sales, inventory, staff, and exception information.

### 1.4 Shared audit events

The system must retain an auditable record of, at minimum:

- Employee sign-in and sign-out.
- Shift opening, cash-in, cash-out, shift closing, counted cash, and variance review.
- Order creation, modification, completion, cancellation, and customer handoff.
- Payment attempt, external payment confirmation, failure, duplicate, and review-required outcome.
- Each split-payment tender, remaining balance, change, and reconciliation result.
- Receipt print, digital receipt delivery, print failure, and retry.
- Refund, void, discount, and manager approval or rejection.
- Kitchen status changes and preparation exceptions.
- Stock shortage, waste, low-stock response, and approved stock movement.
- Offline queueing, sync acknowledgement, duplicate detection, failure, and manual review.
- Report access and export where those capabilities exist.
- Customer creation, lookup, attachment, update, and restricted purchase-history access.

These are workflow events, not a final database schema.

## 2. Cashier workflow

### User role and goal

- User role: Cashier.
- Goal: complete an accurate customer order quickly, collect or record payment safely, provide a receipt, and hand off the order without losing the sale or creating a duplicate.

### Preconditions

- The employee has an active account or employee PIN and is assigned to the outlet and device.
- The device has a current catalog snapshot or the cashier is clearly shown that catalog data is stale.
- The cashier has permission to open or join a shift.
- The payment method is available, or the cashier understands the permitted offline behavior.
- If a printer is configured, its connection state is known before service begins.

### Main steps

1. Sign in using the approved employee authentication method.
2. Confirm the outlet and device context.
3. Open a shift by recording the opening cash amount, or join an already-open shift if permitted.
4. Search for products by name, category, barcode, or configured favorite.
5. Add products, variants, quantities, and modifiers to the order.
6. Select the dining option: dine-in or takeaway.
7. Optionally find or create a basic customer record using the permitted name and normalized phone fields; continue without a customer record when the customer declines.
8. Save the order as an open ticket when the customer is not ready to pay or the order must remain available to the kitchen.
9. Review items, modifiers, quantities, discounts, taxes, customer reference, and total before payment. Tax and report definitions are governed by P0-T04/P0-T04A and are not redefined here.
10. Select one or more payment methods:
    - Cash: receive the amount, record tendered cash, calculate change, and confirm the cash outcome.
    - QR: wait for external QR confirmation, then record the permitted provider/reference information and tender amount.
    - Card: wait for external terminal confirmation, then record the safe transaction reference and tender amount.
11. Complete the sale only when accepted tenders cover the outstanding balance. A failed or uncertain external payment remains pending or review required.
12. Send the paid order to the KDS and confirm the order reference.
13. Print a receipt or provide the configured digital receipt representation.
14. Tell the customer the order number or other safe handoff reference.
15. Confirm handoff after the kitchen marks the order ready, or follow the shop's documented handoff procedure.

### Inputs and outputs

Inputs:

- Employee identity, outlet, device, shift context.
- Product, variant, quantity, modifier, and dining-option selections.
- Open-ticket reference when resuming an existing order.
- One or more payment methods, cash tender, remaining balance, change, or externally confirmed payment references.
- Optional customer name, normalized phone, and permitted customer reference.
- Customer receipt destination when voluntarily supplied and permitted.

Outputs:

- Draft or open ticket.
- Completed sale and payment record, or an explicitly pending/failed/review-required outcome.
- Kitchen order handoff.
- KDS preparation status and reconnect/review state.
- Printed or digital receipt result.
- Customer handoff reference.
- Audit events for the actions taken.

### Permissions required

- Sign in and access the assigned outlet/device.
- Open or join a shift if assigned that responsibility.
- Read the active catalog and prices.
- Create and modify a draft or permitted open ticket.
- Record cash and externally confirmed QR/card payments.
- Record split tenders only when each tender's outcome and amount are known.
- Create, look up, and attach a basic customer record within the permitted role scope.
- Print or issue a digital receipt.
- Request or perform restricted actions only when separately authorized.

Refunds, completed-sale voids, restricted discounts, cash adjustments, and offline inventory corrections require the relevant manager permission. A cashier must not bypass an approval by editing a completed sale.

### Normal success path

The cashier signs in, opens or joins a valid shift, creates the correct order with modifiers and an optional customer reference, records accepted cash or externally confirmed split tenders, completes the sale once, sends it to the KDS, produces a receipt, and hands the order to the customer after preparation.

### Error and exception paths

- Invalid sign-in or locked employee: deny access and direct the employee to the approved manager or identity recovery process.
- Catalog unavailable or stale: show the state; do not invent a price or modifier. Use the last approved snapshot only when the offline rules permit it.
- Item or modifier unavailable: remove it from the order or escalate to the manager/barista; do not silently substitute it.
- Duplicate submit or retry: preserve the client transaction identity and show whether the result is duplicate, accepted, or review required.
- Cash short or tender invalid: do not complete payment until the cash amount is corrected.
- Split tenders do not cover the outstanding balance: keep the payment group incomplete and show the remaining amount.
- QR/card payment failed or uncertain: keep the order pending or review required; do not record it as paid without external confirmation.
- Printer unavailable: retain the digital receipt result or mark printing for retry; do not lose the completed sale.
- Customer changes an open ticket: update the permitted ticket state and send the change to the kitchen with a visible audit trail.
- Customer declines data collection or provides a duplicate phone: continue without silently merging records; request manager review for a suspected duplicate.
- Refund, void, discount, or cash exception: request manager approval and record the decision.

### Offline and synchronization behavior

- Cash sales and permitted shift activity may continue offline.
- QR/card payments may be recorded offline only after staff confirms the external payment succeeded.
- Offline writes use a client transaction identity and remain queued until acknowledged.
- The cashier sees queued, syncing, synced, failed, and review-required states.
- Refunds, new customer creation, customer-history changes, price changes, and inventory corrections are unavailable offline.
- Logout, reset, or cache clearing must not discard unsynchronized sales.
- A reconnect retry must not create a second sale or payment.

### Audit records

Record sign-in, shift action, order changes, payment outcome, receipt outcome, handoff, approval request, offline queueing, synchronization result, and review resolution with the acting employee and relevant transaction reference.

### Related modules

- Identity and access.
- Tenants, outlets, devices, and configuration.
- Catalog and pricing.
- Sales, payments, refunds, receipts, and open tickets.
- Shifts and cash ledger.
- Kitchen order workflow.
- Offline synchronization and transactional outbox.
- Audit logging.

### Acceptance criteria

- [ ] A cashier can follow the documented sign-in, shift, order, payment, receipt, and handoff sequence.
- [ ] Dine-in, takeaway, modifiers, and open tickets have distinct documented behavior.
- [ ] Cash, QR, and externally confirmed card paths state when a sale may be completed.
- [ ] Split payment paths show each tender, remaining balance, change, and review outcome.
- [ ] Basic customer creation, lookup, attachment, and opt-out behavior are explicit.
- [ ] Refund, void, discount, and cash restrictions require explicit manager involvement.
- [ ] Offline and review-required outcomes are visible and do not create silent duplicates.

## 3. Barista/kitchen workflow

### User role and goal

- User role: Barista/kitchen staff.
- Goal: prepare the correct order in the correct sequence, communicate exceptions early, and mark the order ready only when it can be handed to the customer.

### Preconditions

- The staff member is signed in or the kitchen station is associated with an authorized outlet.
- The kitchen can receive the current order queue or a known last-synchronized queue.
- Recipes, modifiers, preparation notes, and dining options are available when configured.
- The staff member knows the procedure for shortages, waste, substitutions, and manager escalation.

### Main steps

1. Sign in or enter the authorized kitchen operating context.
2. Review new orders in arrival sequence, priority, and elapsed waiting time.
3. Open an order and confirm its order reference, items, variants, modifiers, quantities, dining option, and notes.
4. Mark the order as being prepared when work starts.
5. Prepare each item according to the approved product and recipe instructions.
6. Identify an unavailable ingredient, equipment problem, unclear modifier, or unsafe preparation before completing the order.
7. Ask the cashier or manager to resolve a customer-facing change, substitution, refund, or price issue; kitchen staff must not silently change the sale.
8. Record permitted waste or shortage information and notify the manager when approval is required.
9. Mark the order ready when all items pass the shop's handoff standard.
10. Transfer the order to the cashier or handoff process and mark fulfilment `HandedOff` after the shop confirms customer handoff; the sale's `Completed` state remains separate.

### Inputs and outputs

Inputs:

- Kitchen order reference and timestamp.
- Items, variants, modifiers, quantities, dining option, and preparation notes.
- Current preparation queue and available ingredient information.
- Manager or cashier resolution for an exception.

Outputs:

- Preparing, ready, handed-off, delayed, or review-required fulfilment status.
- Shortage, substitution request, waste record, or equipment exception.
- Handoff signal to cashier or customer-service staff.
- Audit events for preparation and exceptions.

### Permissions required

- Read assigned kitchen orders.
- Update permitted preparation states.
- Record preparation delay, shortage, or waste request where authorized.
- Escalate customer, price, payment, refund, and stock-correction decisions.

Barista/kitchen staff cannot approve their own refund, void, restricted discount, cash movement, or final stock correction unless a later approved role design explicitly grants it.

### Normal success path

The kitchen receives the correct order, confirms the details, prepares every item, marks it ready, and transfers it to the handoff process without an unrecorded change or lost ticket.

### Error and exception paths

- Missing or unclear order: pause preparation and request cashier/manager clarification; do not guess.
- Ingredient shortage: mark the preparation exception, notify the cashier/manager, and wait for an approved customer choice.
- Waste or failed preparation: record the permitted waste event and restart only with the appropriate stock and manager handling.
- Equipment failure: mark the affected order delayed and escalate; do not mark ready prematurely.
- Duplicate kitchen order: use the shared order reference and request review; do not prepare a duplicate item automatically.
- Customer requests a change after preparation begins: route it through cashier/manager approval and preserve the original order history.
- Connection loss: continue only from the last trustworthy queue state and surface any uncertain update for synchronization review.

### Offline and synchronization behavior

- The kitchen may use a last-synchronized queue when connectivity is unavailable.
- Local status changes must be marked queued or syncing and must retain the order reference.
- A stale order or conflicting status must be review required, not silently overwritten.
- New price, refund, customer, or stock-correction decisions are not created offline.
- If the kitchen cannot establish a trustworthy queue, it follows the documented shop fallback and informs the manager.

### Audit records

Record kitchen sign-in/context, order viewed, preparation start, status changes, delays, shortages, substitutions requested, waste, equipment exceptions, synchronization outcomes, and handoff completion.

### Related modules

- Identity and access.
- Sales and open tickets.
- Catalog, modifiers, and recipes.
- Inventory and stock movements.
- KDS/order rail.
- Offline synchronization.
- Audit logging.

### Acceptance criteria

- [ ] The kitchen sequence preserves order, modifier, quantity, and dining-option details.
- [ ] New, preparing, ready, and handed-off states have clear ownership and handoff behavior.
- [ ] Shortage, waste, substitution, delay, and equipment exceptions are visible and escalated.
- [ ] Offline queue and conflicting status behavior does not silently lose or duplicate work.

## 4. Manager workflow

### User role and goal

- User role: Manager.
- Goal: keep service operating safely, approve restricted actions, reconcile cash, resolve operational exceptions, and maintain accurate stock and reporting inputs.

### Preconditions

- The manager has authenticated with the required manager permissions.
- The manager is assigned to the relevant outlet or has an explicitly authorized wider scope.
- The manager can inspect the related order, shift, payment, stock, or synchronization reference.
- The shop's approval and escalation procedure is available.

### Main steps

1. Sign in and confirm the outlet and current operating period.
2. Review open shifts, pending approvals, failed payments, review-required transactions, kitchen delays, low-stock signals, and unresolved waste or shortage records.
3. For a refund, void, or restricted discount:
   - Inspect the original order and reason.
   - Confirm the requested amount and affected items.
   - Approve or reject the action.
   - Preserve the original completed sale and create a linked correction outcome.
4. For cash-in or cash-out:
   - Confirm the reason and amount.
   - Record the movement with the responsible employee.
   - Approve it according to the shop procedure.
5. For shift closing:
   - Review expected cash and recorded movements.
   - Count or confirm counted cash.
   - Record variance without editing the underlying ledger.
   - Escalate unexplained variance.
6. For stock shortage, waste, or low stock:
   - Confirm the operational reason.
   - Decide whether to pause, substitute, reorder, or use a permitted adjustment process.
   - Record the approved stock or waste outcome.
7. Resolve failed or review-required transactions by checking payment, sale, receipt, kitchen, and synchronization evidence.
8. Review sales, inventory, shift, waste, and low-stock reports for the outlet and period.
9. Escalate unresolved risks to the owner.

### Inputs and outputs

Inputs:

- Approval request and original transaction reference.
- Cash movement and shift count information.
- Stock shortage, waste, low-stock, and preparation exception information.
- Payment, receipt, synchronization, and audit evidence.
- Sales and inventory report filters.

Outputs:

- Approved or rejected restricted action.
- Linked refund or void outcome.
- Approved discount or cash movement.
- Shift close and variance record.
- Stock shortage, waste, or low-stock decision.
- Resolved or escalated review-required item.
- Manager report review or export request.

### Permissions required

- View outlet operations, orders, shifts, payments, stock events, and audit records within scope.
- Approve refunds, voids, restricted discounts, and permitted cash movements.
- Close shifts and record variance.
- Approve or record stock shortage, waste, and permitted adjustments.
- Resolve or escalate review-required transactions.
- Access manager-level reports.

The manager must not alter an immutable completed sale or delete ledger history to make totals appear correct.

### Normal success path

The manager reviews exceptions, approves only justified restricted actions, reconciles the shift, records stock and waste causes, resolves or escalates uncertain transactions, and leaves reports traceable to the underlying operational events.

### Error and exception paths

- Missing original transaction: reject or hold the request until the source record is found.
- Duplicate approval request: use the idempotent request/result and do not apply the action twice.
- Payment mismatch: keep the transaction review required and compare external confirmation with the POS record.
- Cash variance: record the measured variance and escalation; do not overwrite expected cash or delete movements.
- Stock shortage with no safe substitute: stop or defer the affected item and inform cashier/owner.
- Waste without sufficient evidence: record as pending review rather than inventing a cause or quantity.
- Stale report or offline data: show the last synchronized time and do not present it as live.
- Sync conflict: preserve both the known local and server outcomes and route the item to review.

### Offline and synchronization behavior

- Managers may review locally available shift and queued-sale information offline.
- Refunds, price changes, customer creation, and inventory corrections remain unavailable offline unless a later approved policy explicitly changes this.
- Cash activity may be recorded offline when the local device is trusted, but it must remain queued and visibly unsynchronized.
- Reconnection requires acknowledgement before queued records are treated as synchronized.
- Failed or conflicting records remain review required until resolved.

### Audit records

Record manager sign-in, approval request and decision, cash movement, shift close, variance, stock decision, waste decision, report access/export, synchronization resolution, and escalation.

### Related modules

- Identity, roles, permissions, and audit logging.
- Sales, payments, refunds, discounts, receipts, and open tickets.
- Shifts and cash ledger.
- Inventory, procurement, recipes, stock movements, and waste.
- Reporting.
- Offline synchronization and reconciliation.

### Acceptance criteria

- [ ] Every restricted action has an inspect, approve/reject, and audit path.
- [ ] Shift closing preserves source cash movements and records variance separately.
- [ ] Stock shortage, waste, and low-stock conditions have an explicit decision and escalation path.
- [ ] Failed, stale, duplicate, and conflicting records remain visible for review.

## 5. Owner workflow

### User role and goal

- User role: Owner.
- Goal: understand business performance and operational risk across the shop, make informed decisions, and verify that sales, cash, inventory, and staff activity remain accountable.

### Preconditions

- The owner has authenticated with owner-level protection.
- The owner has access to the relevant tenant, outlet, and reporting period.
- Reports identify their data freshness and whether totals include pending or review-required records.
- The owner understands that workflow documentation does not itself define final accounting or tax terms.

### Main steps

1. Sign in and select the allowed outlet, period, and reporting scope.
2. Review sales totals, payment-method results, refunds, voids, discounts, and shift variances.
3. Compare cash and external-payment outcomes with recorded sales and unresolved payment exceptions.
4. Review inventory position, low-stock items, stock movements, waste, and shortage patterns.
5. Review staff, shift, outlet, and operational performance where permissions and data are available.
6. Inspect audit records for unusual refunds, voids, discounts, cash movements, overrides, and synchronization failures.
7. Export or share a report only through an authorized path.
8. Create or assign a follow-up for unresolved exceptions, stock risk, cash variance, or repeated operational failure.

### Inputs and outputs

Inputs:

- Reporting period, outlet, employee, item, category, and payment filters.
- Sales, payment, refund, void, discount, shift, and variance records.
- Inventory, waste, shortage, and low-stock records.
- Audit and synchronization status.

Outputs:

- Reviewed sales and operational reports.
- Identified cash, payment, inventory, staff, or synchronization risk.
- Authorized report export or follow-up action.
- Escalation or decision request to management.

### Permissions required

- Owner-level authentication and tenant/outlet reporting access.
- Read access to sales, payments, shifts, inventory, staff activity, audit records, and synchronization status within the authorized scope.
- Report export or sharing permission where configured.
- Administrative changes only through separately approved workflows; report access alone does not authorize data correction.

### Normal success path

The owner reviews current and historical information with freshness and exception status visible, reconciles major sales/cash/inventory signals, identifies risks, and assigns follow-up without altering source ledgers through a report.

### Error and exception paths

- Report unavailable: show the failure and preserve the last known successful report with its timestamp.
- Data still syncing: label totals as incomplete or delayed and show the affected scope.
- Conflicting totals: link the owner to the manager review-required process rather than choosing a number silently.
- Export failure: retain the report view and record the failed export attempt if audit policy requires it.
- Unauthorized scope: deny access and record the authorization failure without exposing other outlet or tenant data.

### Offline and synchronization behavior

- Owner reporting is read-only when offline and uses clearly labelled last-synchronized data if available.
- Reports must distinguish complete, stale, partial, failed, and review-required information.
- No owner report should imply that queued sales, pending payments, or unsynchronized stock are fully included.
- Report refresh after reconnect must preserve the reporting period and show the new freshness state.

### Audit records

Record owner sign-in, report access, filters or scope, export attempt and result, authorization failure, and follow-up or escalation reference where supported.

### Related modules

- Identity, tenant/outlet scope, roles, permissions, and audit logging.
- Sales, payments, refunds, discounts, and shifts.
- Inventory, waste, shortages, and low-stock projections.
- Reporting and export.
- Offline synchronization and data freshness.

### Acceptance criteria

- [ ] Owner reporting covers sales, cash/payment, inventory, waste, low stock, shifts, and operational exceptions.
- [ ] Report freshness and incomplete or review-required data are visible in the workflow contract.
- [ ] Owner reporting is read-only with respect to immutable sales and ledger records.
- [ ] Unauthorized scope and failed report/export behavior are documented.

## 6. End-to-end workflow coverage

| Scenario | Primary role | Supporting roles | Required outcome |
|---|---|---|---|
| Employee sign-in and shift opening | Cashier | Manager | Authorized employee and opening cash are recorded. |
| Product search and order creation | Cashier | Barista/kitchen | Correct items, modifiers, quantity, and dining option are preserved. |
| Open ticket sent to kitchen | Cashier | Barista/kitchen | Ticket remains traceable through preparation and handoff. |
| Cash payment | Cashier | Manager | Tender, change, sale, and receipt agree. |
| QR or external card payment | Cashier | Manager | Sale completes only after external confirmation. |
| Split payment | Cashier | Manager | Accepted tenders, remaining balance, change, and payment outcome reconcile to the sale. |
| Kitchen preparation | Barista/kitchen | Cashier, Manager | Order status and exceptions remain visible. |
| KDS reconnect or review | Barista/kitchen | Cashier, Manager | Paid order status is not silently lost or duplicated. |
| Customer handoff | Cashier/barista | Barista/kitchen | Ready order is handed over once and recorded. |
| Basic customer CRM | Cashier | Manager, Owner | Name, phone, consent-to-record choice, and purchase-history access remain authorized and traceable. |
| Refund, void, or discount | Manager | Cashier | Original sale remains traceable and approval is audited. |
| Cash-in, cash-out, and shift close | Manager | Cashier | Movements, count, and variance are recorded without ledger deletion. |
| Stock shortage, waste, and low stock | Manager | Barista/kitchen, Cashier | Item/customer impact and stock decision are visible. |
| Sales and inventory reporting | Owner | Manager | Reports show scope, freshness, and unresolved exceptions. |
| Admin operational overview | Manager/Owner | Cashier, Barista/kitchen | Sales, shifts, low-stock, customer, and exception signals show scope and freshness. |
| Offline sale and synchronization | Cashier/Manager | Barista/kitchen, Owner | Queue, retry, acknowledgement, failure, and review states are explicit. |

## 7. Deferred decisions and boundaries

- P0-T04 and P0-T04A define MVP scope, non-goals, tax terms, report definitions, split payments, customer CRM, KDS, and admin overview boundaries.
- P0-T05 and P0-T06 will define architecture, data relationships, authorization design, and integration boundaries.
- P0-T07 will define UI screens, wireframes, design tokens, and interaction details for the approved POS, KDS, CRM, inventory, and admin surfaces.
- P0-T08 will validate the actual printer, scanner, device, and kitchen hardware procedure.
- Phase 4 will define and test the detailed offline synchronization and conflict behavior.
- This document does not select a database schema, package, payment provider, or deployment configuration.

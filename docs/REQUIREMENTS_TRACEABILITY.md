# Coffee POS Requirements Traceability

Updated: 18 August 2026  
Status: P0-T04A through P0-T08 approved; P0-T09 in progress (workflow installation/validation)  
Source: Coffee POS brief supplied by the user and the existing Coffee POS product/workflow documents

This matrix classifies the supplied brief without claiming implementation. `MVP` means the capability is planned for the one-shop MVP; `Future` means it remains outside the MVP; `Phase 0` means the requirement is a decision or planning deliverable. Acceptance tests are planned checks, not executed results.

## Product capabilities

| Requirement | Classification | Owning work | Planned acceptance evidence |
|---|---|---|---|
| Fast product selling with categories | MVP | Phase 2 POS | Cashier completes a category-based order with touch and keyboard. |
| Variants and modifiers | MVP | Phase 2 catalog/POS | Variant and modifier selections remain attached to the order and receipt. |
| Real-time order creation and updates | MVP | Phase 2/3 sales and SignalR KDS | POS and KDS show the same order version or an explicit review state. |
| Split payments | MVP | P0-T04A, P0-T06, Phase 2 payments | Multiple accepted tenders reconcile to the amount due without duplicate payment records. |
| Cash, card, and QR mock payments | MVP | Phase 2 payments | Cash calculates change; card/QR require external confirmation; raw card data is never stored. |
| Item/order discounts | MVP | Phase 2 sales | Authorized discount allocation is visible and auditable. |
| Refunds and voids | MVP | Phase 2 sales | Voids and refunds preserve the original sale and require the intended permission. |
| Basic customer CRM | MVP | P0-T04A, Phase 2/3 customers | Name, normalized phone, opt-out, attachment, lookup, and purchase-history access are role-scoped. |
| Inventory deduction per sale | MVP | Phase 3 inventory | A completed sale creates exactly one matching stock deduction. |
| Recipe/ingredient reduction | MVP | Phase 3 recipes | Recipe quantities produce correct ingredient movements. |
| Shift open/close and cash tracking | MVP | Phase 2 shifts | Expected cash, counted cash, movements, and variance reconcile. |
| Sales history and reports | MVP | Phase 3 reporting/admin | Daily, product, cashier, payment, shift, inventory, and exception views state scope and freshness. |
| Kitchen Display System | MVP | P0-T04A, P0-T05, P0-T07, Phase 3 KDS | Paid orders reach KDS; New/Preparing/Ready/HandedOff transitions remain visible. |
| Print-ready receipt structure | MVP | Phase 2/3 receipts and P0-T08 | 80 mm representation and digital receipt remain linked to the completed sale. |
| Barcode-ready search path | MVP-ready boundary | Phase 2/3 catalog and P0-T08 | HID scanner input can be validated without making hardware support an untested claim. |
| Admin dashboard overview | MVP | P0-T04A, P0-T07, Phase 3 admin | Sales, payments, shifts, low stock, customers, and exceptions show freshness and scope. |

## Architecture and data boundaries

| Requirement | Classification | Owning work | Planned acceptance evidence |
|---|---|---|---|
| Modular monolith | Phase 0 / MVP architecture | P0-T05/P0-T06 | Module and dependency diagrams show one deployable backend boundary. |
| React + TypeScript + Vite frontend | Phase 0 / MVP architecture | P0-T06/P0-T07 | ADR and approved frontend specification identify the toolchain. |
| ASP.NET Core backend | Phase 0 / MVP architecture | P0-T06 | ADR and module boundary document identify the API/worker layout. |
| PostgreSQL primary database | Phase 0 / MVP architecture | P0-T05/P0-T06 | ERD, tenancy decision, and transaction boundaries are documented. |
| Domain/infrastructure/shared separation | Phase 0 / MVP architecture | P0-T05/P0-T06 | Dependency diagram prevents domain rules from depending on infrastructure. |
| Transaction-safe checkout and stock updates | MVP invariant | P0-T05/P0-T06, Phase 2/3 | Forced failure and retry tests show no partial or duplicate sale/stock result. |
| Internal event-driven design | MVP architecture | P0-T05/P0-T06 | Outbox and event contract show committed downstream work. |
| SignalR or equivalent KDS updates | MVP architecture | P0-T05/P0-T06, Phase 3 | Reconnect and duplicate-event tests preserve KDS visibility. |
| Future multi-outlet and multi-tenant readiness | Future-ready architecture | P0-T05/P0-T06, Phase 1 | Tenant/outlet scope and RLS are documented without claiming SaaS operation. |

## Domain and state contracts

| Requirement | Classification | Owning work | Planned acceptance evidence |
|---|---|---|---|
| Core entities: users, roles, products, categories, modifiers, orders, payments, customers, inventory, recipes, shifts, discounts, receipts | Phase 0 contract / MVP | P0-T04A/P0-T05/P0-T06 | ERD and ADR describe ownership and invariants before migrations. |
| State labels Draft, Placed, Paid, InKitchen, Ready, Completed, Voided, Refunded | MVP state projections | P0-T04A/P0-T06, Phase 2/3 | Separate order, payment, fulfilment, refund, and sync state machines map source labels: `Paid` is payment, `InKitchen`/`Ready`/`HandedOff` are fulfilment, `Completed` is order/sale, `Voided` is pre-completion, and `Refunded` is derived. |
| Prevent negative stock with manager/admin override | MVP invariant | P0-T05/P0-T06, Phase 3 | Default rejection, override permission, reason, and audit record are tested. |
| Receipt/order/customer traceability | MVP invariant | P0-T04A/P0-T05, Phase 2/3 | Sale, payment, receipt, customer, KDS, and stock references remain linked. |

## Workflow, delivery, and quality

| Requirement | Classification | Owning work | Planned acceptance evidence |
|---|---|---|---|
| Cashier flow from shift opening to repeat sale | MVP workflow | P0-T04A, Phase 2 | Critical POS flow passes touch and keyboard checks. |
| KDS receives paid orders and returns preparation status | MVP workflow | P0-T04A, P0-T07, Phase 3 | POS-to-KDS event and status synchronization tests pass. |
| Light Route for small isolated work | Phase 0 workflow | P0-T09 | Fresh session confirms no unnecessary worker is created. |
| Heavy Route for architecture, payments, inventory, KDS, schemas, and complex logic | Phase 0 workflow | P0-T09 | Fresh session confirms route selection, ownership, and review gates. |
| Main agent owns architecture, decomposition, debugging, and final review | Phase 0 workflow | P0-T09 | Task capsule and reviewer evidence identify parent decisions. |
| Coding agents implement and test bounded work | Phase 0 workflow | P0-T09 | Executor/tester reports include exact files, checks, and results. |
| Unit, integration, UI, KDS sync, and transaction safety testing | MVP quality | P0-T05/P0-T06, Phase 1–3 | Verification matrix contains each suite and failure criterion. |
| JWT authentication and role-based authorization | MVP security | P0-T04A/P0-T06, Phase 1 | Secure cookie, CSRF, permission-abuse, and audit tests pass. |
| Input validation, audit logs, secure shifts, and protected inventory | MVP security | P0-T05/P0-T06, Phase 1–3 | Threat model and abuse tests cover each sensitive operation. |
| Original branding and touch-first retail UX | MVP design | P0-T07, Phase 1–3 | Approved concepts and screenshots show original, responsive workflows. |

## Future scope preserved

The following remain outside the one-shop MVP unless a later approved task changes the boundary: multi-outlet operations, multi-tenant SaaS onboarding, offline POS sales, advanced analytics, loyalty, barcode hardware integration beyond the validated path, supplier/purchase-order workflows, advanced discount rules, and a staff mobile app.

## Review notes

- Tax, MyInvois, accounting, PCI, production-readiness, and legal claims remain provisional or deferred as recorded in the product definition.
- The matrix records intended behavior and ownership; it does not claim that application code, tests, hardware, workflow installation, or external systems exist.
- Any future requirement change must update this matrix and the product definition before implementation scope changes.

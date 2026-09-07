# P0-T07 Frontend Specification — Roast Ledger

Status: Approved frontend contract; implementation is governed by Phase 1 task gates  
Date: 2026-08-18  
Scope: Coffee POS, Admin, and KDS surfaces; Customer Display remains deferred

## 1. Design thesis

Coffee POS is used by a cashier and barista during a rush, not by a dashboard reader browsing a SaaS product. The interface should feel like a precise coffee-production workspace: fast, tactile, operational, and distinct from a generic SaaS dashboard.

Primary users and jobs:

- Cashier: complete orders quickly with minimal mistakes.
- Barista: understand sequence, modifiers, and waiting time.
- Manager: control products, stock, staff, and shifts.
- Owner: understand sales, stock, and outlet performance.

Visual direction: **Roast Ledger**, the authoritative direction in `PROJECT_PLAN.md`. The memorable signature is the **Order Rail**: functional kitchen chits with an order number, elapsed time, dining option, and status notch. It makes the shared handoff object visible without turning the product into a decorative restaurant theme.

Explicitly avoided: decorative coffee photography used as hero decoration, gradients, neon dashboards, excessive rounded cards, vanity KPI hero panels, and a second visual language that conflicts with Roast Ledger. Functional catalog thumbnails may be used when they improve item recognition; they are not a decorative hero treatment.

## 2. Visual contract

### 2.1 Tokens

| Token | Hex | Use |
|---|---|---|
| Char | `#1C2220` | Primary text, navigation, dark surfaces |
| Porcelain | `#F3F6F4` | Application background and light work surface |
| Bottle Green | `#246B58` | Primary actions and successful completion |
| Roast Amber | `#D88A2D` | Warning, waiting, offline, and review attention |
| Coffee Cherry | `#B6404B` | Destructive actions and overdue orders |
| Steam Blue | `#DDE9E7` | Selected rows, secondary surfaces, and calm grouping |

Rules:

- Use no decorative gradients.
- Use restrained 6–10px radii and minimal shadows; dense tables may be square.
- Do not use color as the only status signal. Pair every state with text, icon, shape, or a position change.
- Bottle Green is the primary action color. Any text on a filled action must meet WCAG AA; use Char text only where the computed contrast passes, otherwise use a dark-green treatment or light text.
- Coffee Cherry is reserved for destructive or overdue outcomes, not ordinary attention.
- Minimum touch target is 44×44px; primary POS actions prefer 48px height.
- Visible focus must remain present on both light and dark surfaces.

### 2.2 Typography and icons

| Role | Typeface | Use |
|---|---|---|
| Headings/order numbers | `Barlow Condensed` with a system fallback | Page headings, ticket references, high-signal numbers |
| Interface/body | `Atkinson Hyperlegible` with a system fallback | Labels, product/customer content, instructions |
| Money/quantities/timers/receipt numbers | `IBM Plex Mono` with tabular numbers | Totals, prices, counts, elapsed time, external references |

Use `Phosphor Icons` with a visible text label wherever the action or state could be ambiguous. Sentence case is the default; all-caps is limited to short lane markers.

### 2.3 Spacing, shape, and motion

- Spacing base: 4px; preferred steps are 4, 8, 12, 16, 24, 32, and 48px.
- Use a 1px Char/Steam Blue boundary where surfaces need separation and minimal shadows only for a raised ticket or dialog.
- Use 160ms ease-out for state changes and 240ms for drawers. No looping motion except a small syncing indicator; honor `prefers-reduced-motion`.
- At 200% zoom, totals, state labels, and the decisive action remain visible without horizontal clipping.

### 2.4 Money and quantity display

- Display all Malaysian Ringgit amounts with `RM`, thousands separators, and exactly two decimals: `RM 27.00`, `RM 4,280.00`, `RM 0.00`.
- Store and calculate money using the P0-T06 fixed-precision domain boundary. Never format a binary floating-point intermediate directly for a receipt or total.
- Display quantities according to the catalog rule: integer counts as `2`; measured quantities use the approved decimal precision and unit. Do not silently round a billable quantity.
- Keep totals, tender, change, and remaining balance in `IBM Plex Mono` tabular figures. State text remains adjacent to the number.

## 3. Information architecture

### 3.1 Shared shell

- Role-scoped left rail: outlet, current shift, Take order, Open tickets, Payments, Customers, Kitchen rail, Admin, help, and sign-out.
- Context bar: outlet, shift timer, employee, connection state, sync/review count, and one persistent safe-state indicator.
- One primary task surface per route; a right utility drawer holds customer, approval, audit, or event context.
- Diagnostics never show raw card data, tokens, or secrets. Permission-denied states name the missing role.

### 3.2 POS surface

Default route: `Take order`. The catalog/search, current ticket, payment/remaining-balance panel, and exception drawer are separate regions so order, payment, fulfilment, and printer states are not conflated.

Required operator flows:

1. Search or select a product/category, add it to the ticket, and choose required modifiers before quantity changes.
2. Apply an item or order discount only when the role permits it; otherwise show `Manager approval is required` and preserve the unmodified total.
3. Select dine-in/takeaway and optional customer attachment; show the choice on the ticket and kitchen chit.
4. Send the placed order to KDS, then record payment. Split payment keeps a remaining balance and shows each tender independently.
5. For cash, record tender and calculate change from fixed-precision values. For external card or QR, staff observes the provider result and selects `Confirm external payment`, `Mark review required`, or `Record failure`; no provider API/callback automation is implied by this specification.
6. Open/close shifts with opening float, cash-in/out, counted cash, variance, and manager review where required.

Customer Display is not part of this route and requires separate approval.

### 3.3 Admin surface

Default route: `Today`, with freshness and exceptions before vanity metrics. Admin navigation and permissions cover:

- Daily overview: sales, pending/review payments, kitchen delays, low-stock signals, and shift variance, each with scope and freshness.
- Catalog: categories, products, prices, required/optional modifiers, tax mapping, availability, and archive/restore permission.
- Inventory: on-hand, low-stock threshold, adjustments, wastage, and audit trail.
- Customers: search, consent-aware profile fields, visit history, and duplicate-safe merge request.
- Shifts and cash: opening float, movements, count, variance, close, and approvals.
- Audit and settings: role-scoped corrections and append-only event history.

Reports must identify whether queued, pending, stale, or review-required records are included. Export and correction actions are permission-scoped and audited.

### 3.4 KDS surface

Default route: `Kitchen rail`. Four lanes are always named `New`, `Preparing`, `Ready`, and `HandedOff`. Each ticket shows order number, elapsed time, dining option, line items, modifiers/allergy note, and the next permitted action. A reconnect/review banner shows the last trusted queue snapshot. KDS does not expose customer purchase history or manager-only data.

## 4. Component and interaction principles

| Component | Contract | Required states |
|---|---|---|
| Primary button | Name the safe action; Bottle Green for the decisive action | Default, hover, focus, pressed, disabled, loading, permission denied |
| Secondary/destructive button | Quiet alternative or explicit consequence; Coffee Cherry only for destructive action | Default, focus, disabled, loading, confirmation |
| Search/field | Persistent label, domain example, validation beside the field | Empty, filled, invalid, loading, disabled, permission denied |
| Product/catalog card | Name, fixed-precision price, availability, modifier cue; no hidden destructive action | Available, unavailable, loading, empty, permission denied |
| Order ticket/Order Rail chit | One physical-ticket metaphor; state rail and elapsed time remain scannable | Draft, placed, partially paid, paid, completed, voided, review required |
| Status chip/banner | Text + Phosphor icon + semantic color/shape | Pending, paid, failed, review required, syncing, stale, ready, offline |
| Tabs/lanes | Preserve location and keyboard order; urgent exceptions stay discoverable | Active, hover, focus, empty lane, loading lane |
| Table | Header scope and freshness; row focus and non-color state label | Loading, empty, partial, stale, error, permission denied |
| Dialog/drawer | Explain consequence and required permission; return focus to invoker | Confirm, loading, failed, permission denied, escape/focus return |
| Toast/banner | Say what happened and the next safe action; never claim unverified success | Success, pending, review, failed, offline, reconnecting |
| Navigation rail | Role-scoped verbs, not technical modules | Active, focus, collapsed, locked/permission denied |

Canonical state vocabulary is shared across surfaces: order `Draft → Placed → Completed` with audited pre-completion `Voided`; payment `Pending → PartiallyPaid → Paid` plus `Failed`/`ReviewRequired`; fulfilment `NotSent → New → Preparing → Ready → HandedOff`; refunds are append-only derived partial/full records; synchronization is a separate state.

## 5. State matrix

Every surface must make the following states visible and actionable. Loading and syncing are not success; offline and review-required are not completion.

| State | POS | Admin | KDS |
|---|---|---|---|
| Loading | Skeleton product tiles and `Loading catalog`; ticket controls disabled | Skeleton summary/table and `Loading today's data` | Lane skeletons and `Loading trusted queue` |
| Empty | `No matching products. Check the category or add a permitted catalog item.` | `No sales in this scope yet.` with scope/date action | `No orders waiting in this lane.` |
| Error | `Catalog could not load. Retry; keep the current ticket unchanged.` | `Today's data could not load. Retry or inspect the last trusted snapshot.` | `Kitchen queue unavailable. Stop automatic transitions and reconcile.` |
| Permission denied | `Manager approval is required to discount, void, or close this shift.` | `You can view this report, but correction/export requires Manager.` | `Only a permitted barista or manager can move this ticket.` |
| Offline | `Saved locally for review; do not present the sale as completed.` | `Showing last trusted data from 14:32; edits are disabled.` | `Last trusted queue from 14:32; do not overwrite newer states.` |
| Syncing | `Syncing ticket…` with retry/review affordance | `Syncing changes; freshness is provisional.` | `Syncing lane state; retain the visible order sequence.` |
| Review required | `Payment needs staff verification before it can be Paid.` | `2 records need review; exclude them from confirmed totals.` | `1 ticket needs reconciliation before transition.` |

## 6. POS wireframe and keyboard route

```text
+----------------+---------------------------------------------------------------+
| OUTLET / SHIFT  | TAKE ORDER                 [connected] [review 2] [F2 search]   |
|----------------|---------------------------------------------------------------|
| Take order     | Search products...       | CURRENT TICKET #A-1042 Draft |
| Open tickets    | [Espresso] [Latte] [Tea] | 2x Latte          RM 18.00      |
| Payments        | [Pastry] [Cold] [Modifier]| 1x Croissant       RM  9.00      |
| Customers       | category / availability   | Discount: —  Balance RM 27.00  |
| Shift 02:14     | [Add to ticket]           | [Send] [Pay] [Split] [More]    |
+----------------+---------------------------------------------------------------+
| exception drawer: offline / failed / permission / printer retry       |
+-----------------------------------------------------------------------+
```

Full Windows keyboard checkout is a first-class route, with visible hints and no browser/OS shortcut hijacking:

- `F2`: focus product search; arrows move results; `Enter`: add selected product.
- `Ctrl+Shift+M`: open modifiers; arrows choose; `Enter`: apply; `Esc`: close without mutation.
- `Ctrl+Shift+D`: request discount; `Ctrl+Shift+P`: open payment; `Ctrl+Shift+S`: split payment.
- In payment, arrows choose tender, numeric keys enter cash, `Enter` records the selected safe action, and `Ctrl+Enter` confirms the final review.
- `Tab`/`Shift+Tab` follow navigation → search/catalog → ticket → payment/action → exception drawer. `Esc` closes a non-destructive dialog and returns focus.
- The implementation must test these bindings on Windows with common browser shortcuts; a shortcut that conflicts is replaced with a discoverable alternative.

Responsive rules: at ≥1024px catalog and ticket are side by side; at 768–1023px the ticket is a persistent right drawer; below 768px the ticket is a full-height sheet while the total/payment bar remains reachable.

## 7. Admin and KDS wireframes

### 7.1 Admin — freshness before vanity metrics

```text
+----------------+---------------------------------------------------------------+
| ADMIN / TODAY   | 18 AUG 2026 · OUTLET 01    [fresh 2 min] [exceptions 4]        |
|----------------|---------------------------------------------------------------|
| Today           | NEEDS ATTENTION: [2 payment reviews] [1 kitchen delay]         |
| Catalog         | SALES / CASH: RM 4,280.00 confirmed · RM 320.00 review         |
| Inventory       | Shift variance: RM -12.00  ·  [scope: outlet 01, today]        |
| Customers       | orders table: freshness · cashier · payment · state             |
| Shifts / Audit  | [loading] [empty] [error] [permission denied] variants         |
+----------------+---------------------------------------------------------------+
```

Admin is desktop/tablet-first: at ≥1024px the rail, summary, and table share the viewport; at 768–1023px summary rows stack above the table and filters become a drawer; below 768px each table row becomes a labeled detail card with export/correction actions kept permission-gated.

### 7.2 KDS — rail lanes, not dashboard cards

```text
+----------------+---------------------------------------------------------------+
| KITCHEN RAIL    | OUTLET 01 · BAR              [connected] [review 1]             |
|----------------+---------------------------------------------------------------|
| NEW            | PREPARING          | READY             | HANDED OFF         |
| #A-1042  2m    | #A-1040  5m        | #A-1038  9m       | #A-1036 12m       |
| 2 Latte        | 1 Mocha            | 2 Americano       | 1 Tea             |
| allergy note   | modifier note      | pickup: counter   | handoff confirmed |
| [Start]        | [Ready]            | [Handed off]      | [View]            |
+----------------+--------------------+-------------------+-------------------+
| reconnect/review: last trusted queue 14:32 · [Reconcile]             |
+------------------------------------------------------------------------+
```

KDS is optimized for a large desktop/tablet viewing distance: at ≥1280px use four equal lanes; at 768–1279px keep four horizontally scrollable lanes with the selected ticket pinned; below 768px show one lane at a time with a lane picker and preserve the elapsed time/order number. Touch targets are large, and keyboard movement is discoverable rather than assumed.

## 8. Accessibility and safety contract

- Meet WCAG AA contrast for text, controls, focus, and state indicators; test Bottle Green actions in their exact text/background pairing.
- Every control has an accessible name, a visible focus state, and a disabled/permission explanation. Status is text-readable and not color-only.
- Dialogs trap focus, support Escape when safe, and return focus to the invoking control. Live regions announce syncing, failure, and review outcomes politely; urgent destructive outcomes are assertive sparingly.
- Tables expose header associations, row focus, scope, and freshness. KDS state changes remain understandable with reduced motion and high contrast.
- 200% zoom and Windows keyboard checkout must not hide totals, remaining balance, or the decisive action.
- Never expose raw card data, provider secrets, JWTs, or sensitive diagnostics. Offline copy must never claim a completed sale.

## 9. Concept evidence and implementation handoff

The project-local concepts are references for layout, hierarchy, and the Order Rail signature; they are not implementation evidence and do not replace the exact tokens/copy above. Their sample dates and labels are illustrative; the specification's RM formatting, state vocabulary, and safe payment wording are authoritative:

- [POS concept](concepts/roast-ledger-pos-concept.png)
- [Admin concept](concepts/roast-ledger-admin-concept.png)
- [KDS concept](concepts/roast-ledger-kds-concept.png)

The concepts were produced through the `frontend-app-builder`/Image Gen concept workflow after the `frontend-design` contract was read, then inspected locally. The POS concept intentionally contains no raw card number; it uses a staff-observed external-payment label. Before coding, inspect the accepted concepts and use browser-based fidelity checks in Phase 1. Phase 1 must derive CSS variables, component variants, routes, and state stories from this specification; it must not invent a second visual language.

P0-T08 validates device, printer, scanner, and kitchen fallback assumptions. P0-T09 validates Codex workflow and frontend skill routing. No concept has independent user brand approval until the P0-T07 evidence gate is recorded.

## 10. Non-goals

- No React, CSS, TypeScript, package, API, database, migration, or runtime files are introduced by P0-T07.
- The PNG concepts above are reference artifacts only; no production asset pipeline, final brand asset, or design-tool source is approved.
- No Customer Display, payment-provider integration, tax/e-Invoice UI, or offline-sale implementation is approved here.
- No claim is made about production accessibility, performance, browser compatibility, hardware fit, or legal/brand approval until implementation evidence exists.

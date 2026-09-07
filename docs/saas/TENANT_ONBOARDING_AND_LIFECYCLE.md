# Multi-Tenant Data Lifecycle, Isolation & Compliance Guide

## 1. Multi-Tenant Architectural Isolation & Row-Level Security
1. **Zero Data Leakage Guarantee**:
   - Every single database table contains `tenant_id UUID NOT NULL REFERENCES tenants(id)`.
   - PostgreSQL Row-Level Security (RLS) enforces `app.current_tenant_id` session isolation at the engine level.
   - Any query executing without a valid tenant session returns 0 rows.

---

## 2. White-Label Theming & Custom Thermal Branding
- Merchants customize their brand presence under **Admin -> Settings -> Branding**:
  - Store Logo URL for digital receipts and Customer Display System (CDS).
  - Primary Theme Accent Color (`#005D52` standard Roast Ledger teal or custom HEX).
  - Custom Thermal Header & Footer notes (e.g. Wi-Fi SSID, return policy, social media handles).

---

## 3. Data Export, Portability & Right to Erasure
- **Self-Service CSV/JSON Export**: Merchants can download their full product catalog, sales ledger, and customer points register at any time.
- **GDPR / PDPA Right to Erasure**: Hard-delete automated pipeline safely purges tenant data while maintaining required tax audit logs per Malaysian statutory requirements.

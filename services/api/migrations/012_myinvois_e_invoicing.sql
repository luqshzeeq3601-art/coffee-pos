-- 012_myinvois_e_invoicing.sql
-- Malaysian Inland Revenue Board (LHDN) MyInvois e-Invoicing Compliance Ledger

CREATE TABLE IF NOT EXISTS myinvois_documents (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    sales_transaction_id UUID NOT NULL REFERENCES sales_transactions(id) ON DELETE CASCADE,
    invoice_number VARCHAR(64) NOT NULL,
    uuid VARCHAR(128), -- LHDN unique document UUID
    long_id VARCHAR(256),
    status INT NOT NULL DEFAULT 1, -- 1: Draft, 2: Submitted, 3: Valid, 4: Invalid, 5: Cancelled
    buyer_tin VARCHAR(32) NOT NULL,
    buyer_id_type INT NOT NULL, -- 1: NRIC, 2: BRN, 3: PASSPORT, 4: ARMY
    buyer_id_value VARCHAR(64) NOT NULL,
    buyer_name VARCHAR(150) NOT NULL,
    buyer_phone VARCHAR(32),
    buyer_email VARCHAR(150),
    buyer_address TEXT,
    total_excluding_tax NUMERIC(19, 4) NOT NULL,
    total_tax_amount NUMERIC(19, 4) NOT NULL,
    total_payable NUMERIC(19, 4) NOT NULL,
    qr_code_url TEXT,
    validation_errors TEXT,
    submitted_at_utc TIMESTAMPTZ,
    validated_at_utc TIMESTAMPTZ,
    created_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Performance & Lookup Indexes
CREATE INDEX IF NOT EXISTS idx_myinvois_tenant_tx ON myinvois_documents(tenant_id, sales_transaction_id);
CREATE INDEX IF NOT EXISTS idx_myinvois_tenant_status ON myinvois_documents(tenant_id, status);

-- Enable RLS
ALTER TABLE myinvois_documents ENABLE ROW LEVEL SECURITY;

-- Create Policies
DROP POLICY IF EXISTS tenant_isolation_myinvois ON myinvois_documents;
CREATE POLICY tenant_isolation_myinvois ON myinvois_documents
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

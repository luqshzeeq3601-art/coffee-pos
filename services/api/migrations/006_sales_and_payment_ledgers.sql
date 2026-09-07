-- 006_sales_and_payment_ledgers.sql
-- Append-only immutable sales records, payment entries, and manager-approved refunds.

CREATE TABLE IF NOT EXISTS sales_transactions (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    outlet_id UUID NOT NULL REFERENCES outlets(id) ON DELETE CASCADE,
    order_id UUID NOT NULL REFERENCES orders(id) ON DELETE RESTRICT,
    order_number VARCHAR(50) NOT NULL,
    receipt_number VARCHAR(50) NOT NULL,
    cashier_id UUID NOT NULL,
    cashier_name VARCHAR(150) NOT NULL,
    subtotal NUMERIC(19, 4) NOT NULL,
    discount_total NUMERIC(19, 4) NOT NULL DEFAULT 0.0000,
    tax_total NUMERIC(19, 4) NOT NULL DEFAULT 0.0000,
    grand_total NUMERIC(19, 4) NOT NULL,
    paid_amount NUMERIC(19, 4) NOT NULL,
    change_amount NUMERIC(19, 4) NOT NULL DEFAULT 0.0000,
    created_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    CONSTRAINT uq_sales_tenant_receipt UNIQUE (tenant_id, receipt_number)
);

CREATE TABLE IF NOT EXISTS payments (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    transaction_id UUID NOT NULL REFERENCES sales_transactions(id) ON DELETE CASCADE,
    payment_method INT NOT NULL, -- 1: Cash, 2: DuitNowQR, 3: CreditCard, 4: DebitCard, 5: Custom
    amount NUMERIC(19, 4) NOT NULL,
    tendered_amount NUMERIC(19, 4),
    change_amount NUMERIC(19, 4),
    reference_code VARCHAR(100),
    status INT NOT NULL DEFAULT 2, -- 1: Pending, 2: Completed, 3: Failed, 4: Refunded
    created_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS refunds (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    transaction_id UUID NOT NULL REFERENCES sales_transactions(id) ON DELETE CASCADE,
    amount NUMERIC(19, 4) NOT NULL,
    reason TEXT NOT NULL,
    approved_by_user_id UUID NOT NULL,
    approved_by_user_name VARCHAR(150) NOT NULL,
    created_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Performance & Reporting Indexes
CREATE INDEX IF NOT EXISTS idx_sales_tenant_outlet_date ON sales_transactions(tenant_id, outlet_id, created_at_utc DESC);
CREATE INDEX IF NOT EXISTS idx_sales_order ON sales_transactions(tenant_id, order_id);
CREATE INDEX IF NOT EXISTS idx_payments_transaction ON payments(tenant_id, transaction_id);
CREATE INDEX IF NOT EXISTS idx_refunds_transaction ON refunds(tenant_id, transaction_id);

-- Enable RLS
ALTER TABLE sales_transactions ENABLE ROW LEVEL SECURITY;
ALTER TABLE payments ENABLE ROW LEVEL SECURITY;
ALTER TABLE refunds ENABLE ROW LEVEL SECURITY;

-- Create Policies
DROP POLICY IF EXISTS tenant_isolation_sales ON sales_transactions;
CREATE POLICY tenant_isolation_sales ON sales_transactions
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

DROP POLICY IF EXISTS tenant_isolation_payments ON payments;
CREATE POLICY tenant_isolation_payments ON payments
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

DROP POLICY IF EXISTS tenant_isolation_refunds ON refunds;
CREATE POLICY tenant_isolation_refunds ON refunds
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

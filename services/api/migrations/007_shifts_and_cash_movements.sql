-- 007_shifts_and_cash_movements.sql
-- Cash drawer shift lifecycle, payouts (cash-in/cash-out), blind counts, and variance ledgers.

CREATE TABLE IF NOT EXISTS shifts (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    outlet_id UUID NOT NULL REFERENCES outlets(id) ON DELETE CASCADE,
    cashier_id UUID NOT NULL,
    cashier_name VARCHAR(150) NOT NULL,
    status INT NOT NULL DEFAULT 1, -- 1: Open, 2: Closed
    opened_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    closed_at_utc TIMESTAMPTZ,
    opening_float NUMERIC(19, 4) NOT NULL DEFAULT 0.0000,
    cash_sales NUMERIC(19, 4) NOT NULL DEFAULT 0.0000,
    cash_refunds NUMERIC(19, 4) NOT NULL DEFAULT 0.0000,
    actual_counted_cash NUMERIC(19, 4),
    closing_notes TEXT
);

CREATE TABLE IF NOT EXISTS cash_movements (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    shift_id UUID NOT NULL REFERENCES shifts(id) ON DELETE CASCADE,
    type INT NOT NULL, -- 1: CashIn, 2: CashOut
    amount NUMERIC(19, 4) NOT NULL,
    reason TEXT NOT NULL,
    cashier_id UUID NOT NULL,
    cashier_name VARCHAR(150) NOT NULL,
    created_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Performance & Reporting Indexes
CREATE INDEX IF NOT EXISTS idx_shifts_tenant_outlet ON shifts(tenant_id, outlet_id, status);
CREATE INDEX IF NOT EXISTS idx_shifts_cashier ON shifts(tenant_id, cashier_id, status);
CREATE INDEX IF NOT EXISTS idx_cash_movements_shift ON cash_movements(tenant_id, shift_id);

-- Enable RLS
ALTER TABLE shifts ENABLE ROW LEVEL SECURITY;
ALTER TABLE cash_movements ENABLE ROW LEVEL SECURITY;

-- Create Policies
DROP POLICY IF EXISTS tenant_isolation_shifts ON shifts;
CREATE POLICY tenant_isolation_shifts ON shifts
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

DROP POLICY IF EXISTS tenant_isolation_cash_movements ON cash_movements;
CREATE POLICY tenant_isolation_cash_movements ON cash_movements
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

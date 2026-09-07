-- 011_customer_loyalty_and_points.sql
-- Customer club profiles, tier progression, points earning and redemption ledgers.

CREATE TABLE IF NOT EXISTS customers (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    name VARCHAR(150) NOT NULL,
    phone_number VARCHAR(32) NOT NULL,
    email VARCHAR(150),
    tier INT NOT NULL DEFAULT 1, -- 1: Bronze, 2: Silver, 3: Gold, 4: Black
    points_balance INT NOT NULL DEFAULT 0,
    total_spent NUMERIC(19, 4) NOT NULL DEFAULT 0.0000,
    visit_count INT NOT NULL DEFAULT 0,
    joined_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    last_visit_utc TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS loyalty_ledger_entries (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    customer_id UUID NOT NULL REFERENCES customers(id) ON DELETE CASCADE,
    type INT NOT NULL, -- 1: EarnPoints, 2: RedeemPoints, 3: TierUpgradeBonus, 4: ManualAdjustment, 5: Expired
    points_delta INT NOT NULL,
    balance_after INT NOT NULL,
    reason TEXT NOT NULL,
    order_number VARCHAR(64),
    performed_by_id UUID NOT NULL,
    performed_by_name VARCHAR(150) NOT NULL,
    created_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

-- Performance & Lookup Indexes
CREATE INDEX IF NOT EXISTS idx_customers_tenant_phone ON customers(tenant_id, phone_number);
CREATE INDEX IF NOT EXISTS idx_loyalty_ledger_customer ON loyalty_ledger_entries(tenant_id, customer_id);

-- Enable RLS
ALTER TABLE customers ENABLE ROW LEVEL SECURITY;
ALTER TABLE loyalty_ledger_entries ENABLE ROW LEVEL SECURITY;

-- Create Policies
DROP POLICY IF EXISTS tenant_isolation_customers ON customers;
CREATE POLICY tenant_isolation_customers ON customers
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

DROP POLICY IF EXISTS tenant_isolation_loyalty_ledger ON loyalty_ledger_entries;
CREATE POLICY tenant_isolation_loyalty_ledger ON loyalty_ledger_entries
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

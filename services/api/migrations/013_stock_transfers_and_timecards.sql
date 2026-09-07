-- 013_stock_transfers_and_timecards.sql
-- Inter-store stock transfers and employee timesheets / timecards

CREATE TABLE IF NOT EXISTS stock_transfers (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    transfer_number VARCHAR(64) NOT NULL,
    source_outlet_id UUID NOT NULL REFERENCES outlets(id) ON DELETE CASCADE,
    destination_outlet_id UUID NOT NULL REFERENCES outlets(id) ON DELETE CASCADE,
    status INT NOT NULL DEFAULT 1, -- 1: Draft, 2: Dispatched, 3: Received, 4: Cancelled
    dispatched_by_id UUID,
    dispatched_by_name VARCHAR(150),
    dispatched_at_utc TIMESTAMPTZ,
    received_by_id UUID,
    received_by_name VARCHAR(150),
    received_at_utc TIMESTAMPTZ,
    notes TEXT,
    created_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS stock_transfer_items (
    id UUID PRIMARY KEY,
    transfer_id UUID NOT NULL REFERENCES stock_transfers(id) ON DELETE CASCADE,
    stock_item_id UUID NOT NULL REFERENCES stock_items(id) ON DELETE CASCADE,
    quantity NUMERIC(19, 4) NOT NULL
);

CREATE TABLE IF NOT EXISTS staff_timecards (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    outlet_id UUID NOT NULL REFERENCES outlets(id) ON DELETE CASCADE,
    employee_id UUID NOT NULL REFERENCES employees(id) ON DELETE CASCADE,
    employee_name VARCHAR(150) NOT NULL,
    clock_in_utc TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    clock_out_utc TIMESTAMPTZ,
    duration_minutes INT NOT NULL DEFAULT 0,
    regular_hours NUMERIC(6, 2) NOT NULL DEFAULT 0.00,
    overtime_hours NUMERIC(6, 2) NOT NULL DEFAULT 0.00,
    status INT NOT NULL DEFAULT 1, -- 1: ClockedIn, 2: ClockedOut, 3: Approved
    notes TEXT
);

-- Performance & Lookup Indexes
CREATE INDEX IF NOT EXISTS idx_stock_transfers_tenant ON stock_transfers(tenant_id, transfer_number);
CREATE INDEX IF NOT EXISTS idx_staff_timecards_tenant ON staff_timecards(tenant_id, employee_id);

-- Enable RLS
ALTER TABLE stock_transfers ENABLE ROW LEVEL SECURITY;
ALTER TABLE staff_timecards ENABLE ROW LEVEL SECURITY;

-- Create Policies
DROP POLICY IF EXISTS tenant_isolation_stock_transfers ON stock_transfers;
CREATE POLICY tenant_isolation_stock_transfers ON stock_transfers
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

DROP POLICY IF EXISTS tenant_isolation_staff_timecards ON staff_timecards;
CREATE POLICY tenant_isolation_staff_timecards ON staff_timecards
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

-- 009_purchase_orders_and_wastage.sql
-- Supplier purchase orders, receiving workflows, and wastage write-downs with reason codes.

CREATE TABLE IF NOT EXISTS stock_wastage_entries (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    outlet_id UUID NOT NULL REFERENCES outlets(id) ON DELETE CASCADE,
    stock_item_id UUID NOT NULL REFERENCES stock_items(id) ON DELETE CASCADE,
    stock_item_name VARCHAR(150) NOT NULL,
    quantity_wasted NUMERIC(19, 4) NOT NULL,
    unit INT NOT NULL,
    reason INT NOT NULL, -- 1: Spillage, 2: Expired, 3: CalibrationDialIn, 4: QualityDefect, 5: StaffTraining
    cost_impact NUMERIC(19, 4) NOT NULL,
    notes TEXT,
    logged_by_id UUID NOT NULL,
    logged_by_name VARCHAR(150) NOT NULL,
    created_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS purchase_orders (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    outlet_id UUID NOT NULL REFERENCES outlets(id) ON DELETE CASCADE,
    po_number VARCHAR(64) NOT NULL,
    supplier_name VARCHAR(150) NOT NULL,
    status INT NOT NULL DEFAULT 1, -- 1: Draft, 2: Ordered, 3: Received, 4: Cancelled
    notes TEXT,
    created_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    received_at_utc TIMESTAMPTZ
);

CREATE TABLE IF NOT EXISTS purchase_order_items (
    id UUID PRIMARY KEY,
    purchase_order_id UUID NOT NULL REFERENCES purchase_orders(id) ON DELETE CASCADE,
    stock_item_id UUID NOT NULL REFERENCES stock_items(id) ON DELETE RESTRICT,
    stock_item_name VARCHAR(150) NOT NULL,
    unit INT NOT NULL,
    quantity_ordered NUMERIC(19, 4) NOT NULL,
    quantity_received NUMERIC(19, 4) NOT NULL DEFAULT 0.0000,
    unit_cost NUMERIC(19, 4) NOT NULL
);

-- Performance & Reporting Indexes
CREATE INDEX IF NOT EXISTS idx_stock_wastage_tenant_outlet ON stock_wastage_entries(tenant_id, outlet_id);
CREATE INDEX IF NOT EXISTS idx_purchase_orders_tenant_outlet ON purchase_orders(tenant_id, outlet_id, status);

-- Enable RLS
ALTER TABLE stock_wastage_entries ENABLE ROW LEVEL SECURITY;
ALTER TABLE purchase_orders ENABLE ROW LEVEL SECURITY;
ALTER TABLE purchase_order_items ENABLE ROW LEVEL SECURITY;

-- Create Policies
DROP POLICY IF EXISTS tenant_isolation_stock_wastage ON stock_wastage_entries;
CREATE POLICY tenant_isolation_stock_wastage ON stock_wastage_entries
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

DROP POLICY IF EXISTS tenant_isolation_purchase_orders ON purchase_orders;
CREATE POLICY tenant_isolation_purchase_orders ON purchase_orders
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

DROP POLICY IF EXISTS tenant_isolation_purchase_order_items ON purchase_order_items;
CREATE POLICY tenant_isolation_purchase_order_items ON purchase_order_items
    FOR ALL
    USING (EXISTS (
        SELECT 1 FROM purchase_orders po
        WHERE po.id = purchase_order_items.purchase_order_id
        AND po.tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID
    ));

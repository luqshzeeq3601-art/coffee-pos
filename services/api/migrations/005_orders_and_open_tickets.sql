-- 005_orders_and_open_tickets.sql
-- Active order production, cart modifier breakdowns, and named open tickets.

CREATE TABLE IF NOT EXISTS orders (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    outlet_id UUID NOT NULL REFERENCES outlets(id) ON DELETE CASCADE,
    order_number VARCHAR(50) NOT NULL,
    status INT NOT NULL DEFAULT 1, -- 1: Draft, 2: Open, 3: Paid, 4: Cancelled
    dining_option INT NOT NULL DEFAULT 1, -- 1: DineIn, 2: Takeaway, 3: Delivery
    table_or_customer VARCHAR(150),
    cashier_id UUID,
    cashier_name VARCHAR(150),
    discount_total NUMERIC(19, 4) NOT NULL DEFAULT 0.0000,
    tax_percent NUMERIC(19, 4) NOT NULL DEFAULT 6.0000,
    notes TEXT,
    cancel_reason TEXT,
    created_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS order_items (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    order_id UUID NOT NULL REFERENCES orders(id) ON DELETE CASCADE,
    product_id UUID NOT NULL REFERENCES products(id) ON DELETE RESTRICT,
    variant_id UUID,
    name VARCHAR(200) NOT NULL,
    unit_price NUMERIC(19, 4) NOT NULL,
    quantity INT NOT NULL DEFAULT 1,
    notes TEXT
);

CREATE TABLE IF NOT EXISTS order_item_modifiers (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    order_item_id UUID NOT NULL REFERENCES order_items(id) ON DELETE CASCADE,
    modifier_id UUID NOT NULL,
    name VARCHAR(150) NOT NULL,
    price_delta NUMERIC(19, 4) NOT NULL DEFAULT 0.0000
);

-- Performance & Query Indexes
CREATE INDEX IF NOT EXISTS idx_orders_tenant_outlet ON orders(tenant_id, outlet_id, status);
CREATE INDEX IF NOT EXISTS idx_orders_created_at ON orders(tenant_id, created_at_utc DESC);
CREATE INDEX IF NOT EXISTS idx_order_items_tenant_order ON order_items(tenant_id, order_id);
CREATE INDEX IF NOT EXISTS idx_order_item_mods_tenant ON order_item_modifiers(tenant_id, order_item_id);

-- Enable RLS
ALTER TABLE orders ENABLE ROW LEVEL SECURITY;
ALTER TABLE order_items ENABLE ROW LEVEL SECURITY;
ALTER TABLE order_item_modifiers ENABLE ROW LEVEL SECURITY;

-- Create Policies
DROP POLICY IF EXISTS tenant_isolation_orders ON orders;
CREATE POLICY tenant_isolation_orders ON orders
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

DROP POLICY IF EXISTS tenant_isolation_order_items ON order_items;
CREATE POLICY tenant_isolation_order_items ON order_items
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

DROP POLICY IF EXISTS tenant_isolation_order_item_modifiers ON order_item_modifiers;
CREATE POLICY tenant_isolation_order_item_modifiers ON order_item_modifiers
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

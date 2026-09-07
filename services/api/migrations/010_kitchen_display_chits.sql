-- 010_kitchen_display_chits.sql
-- Kitchen Display System (KDS) order chits, preparation station routing, and ticket lifecycle tracking.

CREATE TABLE IF NOT EXISTS kitchen_chits (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    outlet_id UUID NOT NULL REFERENCES outlets(id) ON DELETE CASCADE,
    order_id UUID NOT NULL REFERENCES orders(id) ON DELETE CASCADE,
    order_number VARCHAR(64) NOT NULL,
    dining_option INT NOT NULL, -- 1: DineIn, 2: Takeaway, 3: Delivery
    customer_name VARCHAR(150),
    table_number VARCHAR(50),
    station INT NOT NULL DEFAULT 0, -- 0: All, 1: EspressoBar, 2: FilterBar, 3: PastryKitchen
    status INT NOT NULL DEFAULT 1, -- 1: Queued, 2: Preparing, 3: Ready, 4: Completed, 5: Recalled
    created_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    started_at_utc TIMESTAMPTZ,
    completed_at_utc TIMESTAMPTZ
);

CREATE TABLE IF NOT EXISTS kitchen_chit_items (
    id UUID PRIMARY KEY,
    kitchen_chit_id UUID NOT NULL REFERENCES kitchen_chits(id) ON DELETE CASCADE,
    product_id UUID NOT NULL REFERENCES products(id) ON DELETE RESTRICT,
    product_name VARCHAR(150) NOT NULL,
    variant_name VARCHAR(150),
    modifiers_summary TEXT,
    notes TEXT,
    quantity INT NOT NULL DEFAULT 1,
    is_prepared BOOLEAN NOT NULL DEFAULT FALSE
);

-- Performance & Query Indexes
CREATE INDEX IF NOT EXISTS idx_kitchen_chits_outlet_station_status ON kitchen_chits(tenant_id, outlet_id, station, status);
CREATE INDEX IF NOT EXISTS idx_kitchen_chit_items_chit ON kitchen_chit_items(kitchen_chit_id);

-- Enable RLS
ALTER TABLE kitchen_chits ENABLE ROW LEVEL SECURITY;
ALTER TABLE kitchen_chit_items ENABLE ROW LEVEL SECURITY;

-- Create Policies
DROP POLICY IF EXISTS tenant_isolation_kitchen_chits ON kitchen_chits;
CREATE POLICY tenant_isolation_kitchen_chits ON kitchen_chits
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

DROP POLICY IF EXISTS tenant_isolation_kitchen_chit_items ON kitchen_chit_items;
CREATE POLICY tenant_isolation_kitchen_chit_items ON kitchen_chit_items
    FOR ALL
    USING (EXISTS (
        SELECT 1 FROM kitchen_chits kc
        WHERE kc.id = kitchen_chit_items.kitchen_chit_id
        AND kc.tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID
    ));

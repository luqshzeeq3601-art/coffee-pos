-- 008_inventory_and_stock_ledgers.sql
-- Real-time stock control, fractional units, automated recipe depletions, and append-only stock ledgers.

CREATE TABLE IF NOT EXISTS stock_items (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    outlet_id UUID NOT NULL REFERENCES outlets(id) ON DELETE CASCADE,
    sku VARCHAR(64) NOT NULL,
    name VARCHAR(150) NOT NULL,
    category VARCHAR(100) NOT NULL,
    unit INT NOT NULL, -- 1: Grams, 2: Milliliters, 3: Pieces, 4: Kilograms, 5: Liters
    current_stock NUMERIC(19, 4) NOT NULL DEFAULT 0.0000,
    reorder_threshold NUMERIC(19, 4) NOT NULL DEFAULT 0.0000,
    cost_per_unit NUMERIC(19, 4) NOT NULL DEFAULT 0.0000,
    last_updated_utc TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS stock_ledger_entries (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    stock_item_id UUID NOT NULL REFERENCES stock_items(id) ON DELETE CASCADE,
    type INT NOT NULL, -- 1: SaleDepletion, 2: ReceiveStock, 3: ManualAdjustment, 4: WasteWritedown, 5: Transfer
    quantity_delta NUMERIC(19, 4) NOT NULL,
    balance_after NUMERIC(19, 4) NOT NULL,
    reason TEXT NOT NULL,
    reference_id VARCHAR(100),
    performed_by_id UUID NOT NULL,
    performed_by_name VARCHAR(150) NOT NULL,
    created_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS recipes (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    product_id UUID NOT NULL REFERENCES products(id) ON DELETE CASCADE,
    variant_id UUID REFERENCES variants(id) ON DELETE CASCADE,
    product_name VARCHAR(150) NOT NULL,
    variant_name VARCHAR(150)
);

CREATE TABLE IF NOT EXISTS recipe_items (
    id UUID PRIMARY KEY,
    recipe_id UUID NOT NULL REFERENCES recipes(id) ON DELETE CASCADE,
    stock_item_id UUID NOT NULL REFERENCES stock_items(id) ON DELETE RESTRICT,
    stock_item_name VARCHAR(150) NOT NULL,
    unit INT NOT NULL,
    quantity_required NUMERIC(19, 4) NOT NULL
);

-- Performance & Reporting Indexes
CREATE INDEX IF NOT EXISTS idx_stock_items_tenant_outlet ON stock_items(tenant_id, outlet_id);
CREATE INDEX IF NOT EXISTS idx_stock_ledger_item ON stock_ledger_entries(tenant_id, stock_item_id);
CREATE INDEX IF NOT EXISTS idx_recipes_product ON recipes(tenant_id, product_id, variant_id);

-- Enable RLS
ALTER TABLE stock_items ENABLE ROW LEVEL SECURITY;
ALTER TABLE stock_ledger_entries ENABLE ROW LEVEL SECURITY;
ALTER TABLE recipes ENABLE ROW LEVEL SECURITY;
ALTER TABLE recipe_items ENABLE ROW LEVEL SECURITY;

-- Create Policies
DROP POLICY IF EXISTS tenant_isolation_stock_items ON stock_items;
CREATE POLICY tenant_isolation_stock_items ON stock_items
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

DROP POLICY IF EXISTS tenant_isolation_stock_ledger ON stock_ledger_entries;
CREATE POLICY tenant_isolation_stock_ledger ON stock_ledger_entries
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

DROP POLICY IF EXISTS tenant_isolation_recipes ON recipes;
CREATE POLICY tenant_isolation_recipes ON recipes
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

DROP POLICY IF EXISTS tenant_isolation_recipe_items ON recipe_items;
CREATE POLICY tenant_isolation_recipe_items ON recipe_items
    FOR ALL
    USING (EXISTS (
        SELECT 1 FROM recipes r
        WHERE r.id = recipe_items.recipe_id
        AND r.tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID
    ));

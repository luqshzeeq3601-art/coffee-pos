-- 014_saas_subscriptions_and_branding.sql
-- Multi-tenant SaaS subscription billing tiers, feature entitlements, and white-label receipt branding

CREATE TABLE IF NOT EXISTS tenant_subscriptions (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL UNIQUE REFERENCES tenants(id) ON DELETE CASCADE,
    tier INT NOT NULL DEFAULT 2, -- 1: Starter, 2: Growth, 3: Enterprise
    status INT NOT NULL DEFAULT 2, -- 1: Trialing, 2: Active, 3: PastDue, 4: Canceled
    monthly_price_myr NUMERIC(10, 2) NOT NULL DEFAULT 199.00,
    max_outlets INT NOT NULL DEFAULT 3,
    max_registers INT NOT NULL DEFAULT 6,
    trial_end_utc TIMESTAMPTZ,
    current_period_end_utc TIMESTAMPTZ NOT NULL DEFAULT (NOW() + INTERVAL '1 month')
);

CREATE TABLE IF NOT EXISTS tenant_brandings (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL UNIQUE REFERENCES tenants(id) ON DELETE CASCADE,
    brand_name VARCHAR(150) NOT NULL,
    logo_url TEXT,
    primary_color_hex VARCHAR(16) NOT NULL DEFAULT '#005D52',
    header_text TEXT,
    footer_text TEXT,
    tax_registration_number VARCHAR(64),
    show_wifi_info BOOLEAN NOT NULL DEFAULT FALSE,
    wifi_ssid VARCHAR(100),
    wifi_password VARCHAR(100)
);

-- Enable RLS
ALTER TABLE tenant_subscriptions ENABLE ROW LEVEL SECURITY;
ALTER TABLE tenant_brandings ENABLE ROW LEVEL SECURITY;

-- Create Policies
DROP POLICY IF EXISTS tenant_isolation_subscriptions ON tenant_subscriptions;
CREATE POLICY tenant_isolation_subscriptions ON tenant_subscriptions
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

DROP POLICY IF EXISTS tenant_isolation_brandings ON tenant_brandings;
CREATE POLICY tenant_isolation_brandings ON tenant_brandings
    FOR ALL
    USING (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID)
    WITH CHECK (tenant_id = NULLIF(current_setting('app.current_tenant_id', true), '')::UUID);

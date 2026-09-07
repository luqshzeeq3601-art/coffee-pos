-- 001_initial_tenancy_and_identity.sql
-- Coffee POS initial tenancy, outlets, devices, users, and employees schema.

CREATE TABLE IF NOT EXISTS tenants (
    id UUID PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    currency_code VARCHAR(3) NOT NULL DEFAULT 'MYR',
    time_zone VARCHAR(100) NOT NULL DEFAULT 'Asia/Kuala_Lumpur',
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS outlets (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    name VARCHAR(200) NOT NULL,
    address TEXT,
    is_main_outlet BOOLEAN NOT NULL DEFAULT FALSE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    CONSTRAINT uq_outlets_tenant_name UNIQUE (tenant_id, name)
);

CREATE TABLE IF NOT EXISTS devices (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    outlet_id UUID NOT NULL REFERENCES outlets(id) ON DELETE CASCADE,
    name VARCHAR(150) NOT NULL,
    device_type INT NOT NULL, -- 1: POS, 2: KDS, 3: KitchenDisplay, 4: BarcodeScanner
    status INT NOT NULL,      -- 1: PendingEnrollment, 2: Active, 3: Revoked, 4: Offline
    enrollment_code VARCHAR(50),
    hardware_fingerprint VARCHAR(255),
    enrolled_at_utc TIMESTAMPTZ,
    last_seen_at_utc TIMESTAMPTZ,
    CONSTRAINT uq_devices_enrollment_code UNIQUE (enrollment_code)
);

CREATE TABLE IF NOT EXISTS users (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    email VARCHAR(255) NOT NULL,
    full_name VARCHAR(200) NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    salt VARCHAR(255) NOT NULL,
    role INT NOT NULL, -- 1: Owner, 2: Manager, 3: Cashier, 4: Barista
    is_mfa_enabled BOOLEAN NOT NULL DEFAULT FALSE,
    mfa_secret VARCHAR(255),
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    CONSTRAINT uq_users_tenant_email UNIQUE (tenant_id, email)
);

CREATE TABLE IF NOT EXISTS employees (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    display_name VARCHAR(150) NOT NULL,
    pin_hash VARCHAR(255) NOT NULL,
    pin_salt VARCHAR(255) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at_utc TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS employee_roles (
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    employee_id UUID NOT NULL REFERENCES employees(id) ON DELETE CASCADE,
    role INT NOT NULL,
    PRIMARY KEY (tenant_id, employee_id, role)
);

CREATE TABLE IF NOT EXISTS employee_outlets (
    tenant_id UUID NOT NULL REFERENCES tenants(id) ON DELETE CASCADE,
    employee_id UUID NOT NULL REFERENCES employees(id) ON DELETE CASCADE,
    outlet_id UUID NOT NULL REFERENCES outlets(id) ON DELETE CASCADE,
    PRIMARY KEY (tenant_id, employee_id, outlet_id)
);

-- Indexes for tenancy & lookup performance
CREATE INDEX IF NOT EXISTS idx_outlets_tenant_id ON outlets(tenant_id);
CREATE INDEX IF NOT EXISTS idx_devices_tenant_outlet ON devices(tenant_id, outlet_id);
CREATE INDEX IF NOT EXISTS idx_users_tenant_email ON users(tenant_id, email);
CREATE INDEX IF NOT EXISTS idx_employees_tenant_id ON employees(tenant_id);
CREATE INDEX IF NOT EXISTS idx_employee_roles_tenant_emp ON employee_roles(tenant_id, employee_id);
CREATE INDEX IF NOT EXISTS idx_employee_outlets_tenant_emp ON employee_outlets(tenant_id, employee_id);

export type Role = 'Owner' | 'Manager' | 'Cashier' | 'Barista';

export type Permission =
  | 'pos:checkout'
  | 'pos:refund'
  | 'pos:void'
  | 'pos:discount'
  | 'pos:cash_drawer'
  | 'pos:reprint_receipt'
  | 'catalog:read'
  | 'catalog:write'
  | 'inventory:view'
  | 'inventory:adjust'
  | 'reports:view'
  | 'reports:export'
  | 'shifts:open'
  | 'shifts:close'
  | 'shifts:manage'
  | 'admin:users'
  | 'admin:devices'
  | 'admin:settings'
  | 'override:authorize';

export interface UserProfile {
  id: string;
  email: string;
  fullName: string;
  role: Role;
  tenantId: string;
  status: 'Active' | 'Suspended';
}

export interface EmployeeProfile {
  id: string;
  displayName: string;
  roles: Role[];
  tenantId: string;
  assignedOutletIds: string[];
  status: 'Active' | 'Inactive';
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  requiresMfa: boolean;
  mfaChallengeId?: string;
  user?: UserProfile;
  activeTenantId?: string;
  availableOutlets?: Array<{ id: string; name: string }>;
  token?: string;
  expiresAtUtc?: string;
}

export interface MfaVerifyRequest {
  challengeId: string;
  code: string;
}

export interface PinLoginRequest {
  deviceId: string;
  pin: string;
}

export interface PinLoginResponse {
  employee: EmployeeProfile;
  permissions: Permission[];
  tenantId: string;
  outletId: string;
  deviceId: string;
  token: string;
  expiresAtUtc: string;
}

export interface ManagerOverrideRequest {
  managerPin: string;
  action: 'void' | 'discount' | 'refund' | 'price_override' | 'cash_drawer_manual';
  reason?: string;
  metadata?: Record<string, string>;
}

export interface ManagerOverrideResponse {
  authorized: boolean;
  authorizingEmployeeId?: string;
  authorizingEmployeeName?: string;
  auditEventId?: string;
}

export interface SessionState {
  isAuthenticated: boolean;
  userType?: 'User' | 'Employee';
  userId?: string;
  displayName?: string;
  tenantId?: string;
  outletId?: string;
  deviceId?: string;
  roles: Role[];
  permissions: Permission[];
}

export interface ProblemDetails {
  type?: string;
  title: string;
  status: number;
  detail?: string;
  instance?: string;
  errors?: Record<string, string[]>;
}

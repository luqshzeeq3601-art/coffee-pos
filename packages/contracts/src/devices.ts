export type DeviceType = 'POS' | 'KDS' | 'KitchenDisplay' | 'BarcodeScanner';
export type DeviceStatus = 'PendingEnrollment' | 'Active' | 'Revoked' | 'Offline';

export interface DeviceDto {
  id: string;
  tenantId: string;
  outletId: string;
  name: string;
  deviceType: DeviceType;
  status: DeviceStatus;
  enrolledAtUtc?: string;
  lastSeenAtUtc?: string;
}

export interface DeviceEnrollmentRequest {
  enrollmentCode: string;
  deviceName: string;
  deviceType: DeviceType;
  hardwareFingerprint?: string;
}

export interface DeviceEnrollmentResponse {
  deviceId: string;
  tenantId: string;
  outletId: string;
  deviceToken: string;
  status: DeviceStatus;
}

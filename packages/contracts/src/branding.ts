export interface TenantBrandingDto {
  tenantId: string;
  brandName: string;
  logoUrl?: string;
  primaryColorHex: string;
  headerText?: string;
  footerText?: string;
  taxRegistrationNumber?: string;
  showWifiInfo: boolean;
  wifiSsid?: string;
  wifiPassword?: string;
}

export interface UpdateBrandingRequest {
  brandName: string;
  logoUrl?: string;
  primaryColorHex: string;
  headerText?: string;
  footerText?: string;
  taxRegistrationNumber?: string;
  showWifiInfo: boolean;
  wifiSsid?: string;
  wifiPassword?: string;
}

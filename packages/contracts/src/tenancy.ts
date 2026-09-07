export interface TenantDto {
  id: string;
  name: string;
  currencyCode: string;
  timeZone: string;
  createdAtUtc: string;
}

export interface OutletDto {
  id: string;
  tenantId: string;
  name: string;
  address?: string;
  isMainOutlet: boolean;
  createdAtUtc: string;
}

export interface PilotStoreConfigDto {
  outletName: string;
  address: string;
  currency: string;
  taxRatePercent: number;
  taxName: string;
  openingFloat: number;
  operatingHours: string;
  dialInTarget: {
    doseInGrams: number;
    yieldInGrams: number;
    timeInSeconds: number;
  };
}

export const PILOT_STORE_CONFIG: PilotStoreConfigDto = {
  outletName: 'Artisan Roast Co. (Bangsar Flagship)',
  address: 'No. 12, Jalan Telawi 3, Bangsar, 59100 Kuala Lumpur',
  currency: 'MYR',
  taxRatePercent: 6.0,
  taxName: 'Malaysian Sales & Service Tax (SST)',
  openingFloat: 300.00,
  operatingHours: '07:30 - 22:00 Daily',
  dialInTarget: {
    doseInGrams: 18.0,
    yieldInGrams: 36.0,
    timeInSeconds: 27
  }
};

export type SubscriptionTier = 'Starter' | 'Growth' | 'Enterprise';
export type SubscriptionStatus = 'Trialing' | 'Active' | 'PastDue' | 'Canceled';

export interface TenantSubscriptionDto {
  id: string;
  tenantId: string;
  tier: SubscriptionTier;
  status: SubscriptionStatus;
  monthlyPriceMyr: number;
  maxOutlets: number;
  maxRegisters: number;
  featuresEnabled: string[];
  trialEndUtc?: string;
  currentPeriodEndUtc: string;
}

export interface MerchantOnboardRequest {
  businessName: string;
  outletName: string;
  ownerName: string;
  ownerEmail: string;
  tier: SubscriptionTier;
  currency: string;
  taxRegistrationNumber?: string;
}

export interface EntitlementsDto {
  canUseMyInvois: boolean;
  canUseMultiOutlet: boolean;
  canUseTimecards: boolean;
  canUseAdvancedInventory: boolean;
  canUseCustomBranding: boolean;
  maxOutlets: number;
  maxRegisters: number;
}

export type LoyaltyTier = 'Bronze' | 'Silver' | 'Gold' | 'Black';
export type LoyaltyLedgerType = 'EarnPoints' | 'RedeemPoints' | 'TierUpgradeBonus' | 'ManualAdjustment' | 'Expired';

export interface LoyaltyLedgerEntryDto {
  id: string;
  tenantId: string;
  customerId: string;
  type: LoyaltyLedgerType;
  pointsDelta: number;
  balanceAfter: number;
  reason: string;
  orderNumber?: string;
  performedByName: string;
  createdAtUtc: string;
}

export interface CustomerDto {
  id: string;
  tenantId: string;
  name: string;
  phoneNumber: string;
  email?: string;
  tier: LoyaltyTier;
  pointsBalance: number;
  totalSpent: number;
  visitCount: number;
  joinedAtUtc: string;
  lastVisitUtc: string;
}

export interface CreateCustomerRequest {
  name: string;
  phoneNumber: string;
  email?: string;
}

export interface EarnPointsRequest {
  amountSpent: number;
  orderNumber: string;
}

export interface RedeemPointsRequest {
  pointsToRedeem: number;
  orderNumber: string;
}

export interface LoyaltyDiscountRuleDto {
  tier: LoyaltyTier;
  pointsPerRm: number;
  discountPercentage: number;
  pointsToRedeemCoffee: number;
}

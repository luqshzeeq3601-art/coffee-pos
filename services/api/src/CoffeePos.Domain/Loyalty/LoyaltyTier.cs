namespace CoffeePos.Domain.Loyalty;

public enum LoyaltyTier
{
    Bronze = 1,
    Silver = 2,
    Gold = 3,
    Black = 4
}

public enum LoyaltyLedgerType
{
    EarnPoints = 1,
    RedeemPoints = 2,
    TierUpgradeBonus = 3,
    ManualAdjustment = 4,
    Expired = 5
}

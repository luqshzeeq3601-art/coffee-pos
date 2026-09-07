namespace CoffeePos.Domain.Loyalty;

public sealed class Customer
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public string PhoneNumber { get; private set; }
    public string? Email { get; private set; }
    public LoyaltyTier Tier { get; private set; }
    public int PointsBalance { get; private set; }
    public decimal TotalSpent { get; private set; }
    public int VisitCount { get; private set; }
    public DateTime JoinedAtUtc { get; private set; }
    public DateTime LastVisitUtc { get; private set; }

    public Customer(
        Guid id,
        Guid tenantId,
        string name,
        string phoneNumber,
        string? email = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Customer name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(phoneNumber)) throw new ArgumentException("Phone number is required.", nameof(phoneNumber));

        Id = id;
        TenantId = tenantId;
        Name = name.Trim();
        PhoneNumber = phoneNumber.Trim();
        Email = email?.Trim().ToLowerInvariant();
        Tier = LoyaltyTier.Bronze;
        PointsBalance = 0;
        TotalSpent = 0m;
        VisitCount = 0;
        JoinedAtUtc = DateTime.UtcNow;
        LastVisitUtc = DateTime.UtcNow;
    }

    public LoyaltyLedgerEntry EarnPoints(
        decimal amountSpent,
        string orderNumber,
        Guid performedById,
        string performedByName)
    {
        if (amountSpent <= 0) throw new ArgumentException("Amount spent must be positive.", nameof(amountSpent));

        // 1 pt per RM 1.00 spent
        var pointsEarned = (int)Math.Floor(amountSpent);
        PointsBalance += pointsEarned;
        TotalSpent += amountSpent;
        VisitCount += 1;
        LastVisitUtc = DateTime.UtcNow;

        CheckTierUpgrade();

        return new LoyaltyLedgerEntry(
            Guid.NewGuid(),
            TenantId,
            Id,
            LoyaltyLedgerType.EarnPoints,
            pointsEarned,
            PointsBalance,
            $"Earned {pointsEarned} pts from order {orderNumber} (RM {amountSpent:F2})",
            orderNumber,
            performedById,
            performedByName);
    }

    public LoyaltyLedgerEntry RedeemPoints(
        int pointsToRedeem,
        string orderNumber,
        Guid performedById,
        string performedByName)
    {
        if (pointsToRedeem <= 0) throw new ArgumentException("Points to redeem must be positive.", nameof(pointsToRedeem));
        if (pointsToRedeem > PointsBalance)
            throw new InvalidOperationException($"Insufficient points. Available: {PointsBalance}, requested: {pointsToRedeem}");

        PointsBalance -= pointsToRedeem;
        LastVisitUtc = DateTime.UtcNow;

        return new LoyaltyLedgerEntry(
            Guid.NewGuid(),
            TenantId,
            Id,
            LoyaltyLedgerType.RedeemPoints,
            -pointsToRedeem,
            PointsBalance,
            $"Redeemed {pointsToRedeem} pts for discount on order {orderNumber}",
            orderNumber,
            performedById,
            performedByName);
    }

    private void CheckTierUpgrade()
    {
        if (TotalSpent >= 1000m)
        {
            Tier = LoyaltyTier.Black;
        }
        else if (TotalSpent >= 500m)
        {
            Tier = LoyaltyTier.Gold;
        }
        else if (TotalSpent >= 200m)
        {
            Tier = LoyaltyTier.Silver;
        }
        else
        {
            Tier = LoyaltyTier.Bronze;
        }
    }
}

public sealed class LoyaltyLedgerEntry
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid CustomerId { get; private set; }
    public LoyaltyLedgerType Type { get; private set; }
    public int PointsDelta { get; private set; }
    public int BalanceAfter { get; private set; }
    public string Reason { get; private set; }
    public string? OrderNumber { get; private set; }
    public Guid PerformedById { get; private set; }
    public string PerformedByName { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public LoyaltyLedgerEntry(
        Guid id,
        Guid tenantId,
        Guid customerId,
        LoyaltyLedgerType type,
        int pointsDelta,
        int balanceAfter,
        string reason,
        string? orderNumber,
        Guid performedById,
        string performedByName)
    {
        Id = id;
        TenantId = tenantId;
        CustomerId = customerId;
        Type = type;
        PointsDelta = pointsDelta;
        BalanceAfter = balanceAfter;
        Reason = reason;
        OrderNumber = orderNumber;
        PerformedById = performedById;
        PerformedByName = performedByName;
        CreatedAtUtc = DateTime.UtcNow;
    }
}

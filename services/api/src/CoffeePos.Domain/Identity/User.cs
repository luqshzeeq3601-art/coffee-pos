namespace CoffeePos.Domain.Identity;

public sealed class User
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Email { get; private set; }
    public string FullName { get; private set; }
    public string PasswordHash { get; private set; }
    public string Salt { get; private set; }
    public Role Role { get; private set; }
    public bool IsMfaEnabled { get; private set; }
    public string? MfaSecret { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public User(
        Guid id,
        Guid tenantId,
        string email,
        string fullName,
        string passwordHash,
        string salt,
        Role role = Role.Owner,
        bool isMfaEnabled = false,
        string? mfaSecret = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("User ID cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty.", nameof(email));
        if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Full name cannot be empty.", nameof(fullName));

        Id = id;
        TenantId = tenantId;
        Email = email.Trim().ToLowerInvariant();
        FullName = fullName.Trim();
        PasswordHash = passwordHash;
        Salt = salt;
        Role = role;
        IsMfaEnabled = isMfaEnabled;
        MfaSecret = mfaSecret;
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
    }
}

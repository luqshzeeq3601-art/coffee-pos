using CoffeePos.Application.Common;
using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Identity;

namespace CoffeePos.Infrastructure.Services;

public sealed class AuthService : IAuthService
{
    private readonly IIdentityStore _store;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokenService;
    private readonly ICurrentUser _currentUser;
    private readonly ITenantContext _tenantContext;
    private readonly IAuditService _audit;

    public AuthService(
        IIdentityStore store,
        IPasswordHasher hasher,
        ITokenService tokenService,
        ICurrentUser currentUser,
        ITenantContext tenantContext,
        IAuditService audit)
    {
        _store = store;
        _hasher = hasher;
        _tokenService = tokenService;
        _currentUser = currentUser;
        _tenantContext = tenantContext;
        _audit = audit;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("Email and password are required.");
        }

        var user = await _store.GetUserByEmailAsync(request.Email, cancellationToken);
        if (user == null || !user.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        if (!_hasher.VerifyPassword(request.Password, user.PasswordHash, user.Salt))
        {
            await _audit.RecordAsync(
                user.Id.ToString(),
                "User",
                "auth:login_failed",
                user.TenantId,
                reason: "Password mismatch",
                cancellationToken: cancellationToken);

            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        if (user.IsMfaEnabled)
        {
            var challengeId = Guid.NewGuid().ToString("N");
            return new LoginResponseDto(
                RequiresMfa: true,
                MfaChallengeId: challengeId,
                User: null,
                ActiveTenantId: null,
                AvailableOutlets: null,
                Token: null,
                ExpiresAtUtc: null);
        }

        var outlets = await _store.GetOutletsByTenantIdAsync(user.TenantId, cancellationToken);
        var primaryOutlet = outlets.FirstOrDefault(o => o.IsMainOutlet) ?? outlets.FirstOrDefault();

        var token = _tokenService.GenerateUserToken(user, primaryOutlet?.Id);
        var expiresAt = DateTime.UtcNow.AddHours(8).ToString("o");

        await _audit.RecordAsync(
            user.Id.ToString(),
            "User",
            "auth:login_success",
            user.TenantId,
            primaryOutlet?.Id,
            cancellationToken: cancellationToken);

        return new LoginResponseDto(
            RequiresMfa: false,
            MfaChallengeId: null,
            User: new UserProfileDto(
                user.Id.ToString(),
                user.Email,
                user.FullName,
                user.Role.ToString(),
                user.TenantId.ToString(),
                user.IsActive ? "Active" : "Suspended"),
            ActiveTenantId: user.TenantId.ToString(),
            AvailableOutlets: outlets.Select(o => new OutletSummaryDto(o.Id.ToString(), o.Name)).ToList(),
            Token: token,
            ExpiresAtUtc: expiresAt);
    }

    public Task<LoginResponseDto> VerifyMfaAsync(MfaVerifyRequestDto request, CancellationToken cancellationToken = default)
    {
        // Placeholder for TOTP MFA verification
        if (string.IsNullOrWhiteSpace(request.ChallengeId) || string.IsNullOrWhiteSpace(request.Code))
        {
            throw new ArgumentException("MFA Challenge and Code are required.");
        }

        throw new NotImplementedException("MFA verification is configured for Phase 1 security hardening.");
    }

    public Task<SessionStateDto> GetCurrentSessionAsync(CancellationToken cancellationToken = default)
    {
        if (!_currentUser.IsAuthenticated)
        {
            return Task.FromResult(new SessionStateDto(
                IsAuthenticated: false,
                UserType: null,
                UserId: null,
                DisplayName: null,
                TenantId: null,
                OutletId: null,
                DeviceId: null,
                Roles: Array.Empty<string>(),
                Permissions: Array.Empty<string>()));
        }

        return Task.FromResult(new SessionStateDto(
            IsAuthenticated: true,
            UserType: _currentUser.UserType,
            UserId: _currentUser.Id,
            DisplayName: _currentUser.DisplayName,
            TenantId: _tenantContext.TenantId?.ToString(),
            OutletId: _tenantContext.OutletId?.ToString(),
            DeviceId: _tenantContext.DeviceId?.ToString(),
            Roles: _currentUser.Roles,
            Permissions: _currentUser.Permissions));
    }
}

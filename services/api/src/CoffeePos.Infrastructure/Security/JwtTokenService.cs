using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Identity;

namespace CoffeePos.Infrastructure.Security;

public sealed class JwtTokenService : ITokenService
{
    private readonly byte[] _key;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtTokenService(string? signingSecret = null, string issuer = "CoffeePos", string audience = "CoffeePosClients")
    {
        _issuer = issuer;
        _audience = audience;
        var secret = string.IsNullOrWhiteSpace(signingSecret)
            ? "CoffeePos_Development_Super_Secret_Key_At_Least_32_Chars!"
            : signingSecret;
        _key = Encoding.UTF8.GetBytes(secret);
    }

    public string GenerateUserToken(User user, Guid? outletId = null, TimeSpan? lifetime = null)
    {
        var claims = new Dictionary<string, object>
        {
            ["sub"] = user.Id.ToString(),
            ["user_type"] = "User",
            ["tenant_id"] = user.TenantId.ToString(),
            ["email"] = user.Email,
            ["name"] = user.FullName,
            ["role"] = new[] { user.Role.ToString() },
            ["iss"] = _issuer,
            ["aud"] = _audience,
            ["exp"] = DateTimeOffset.UtcNow.Add(lifetime ?? TimeSpan.FromHours(8)).ToUnixTimeSeconds()
        };

        if (outletId.HasValue)
        {
            claims["outlet_id"] = outletId.Value.ToString();
        }

        return CreateJwt(claims);
    }

    public string GenerateEmployeeToken(Employee employee, Guid outletId, Guid deviceId, TimeSpan? lifetime = null)
    {
        var roleNames = employee.Roles.Select(static r => r.ToString()).ToArray();
        var permissions = employee.Roles.SelectMany(Permissions.GetPermissionsForRole).Distinct().ToArray();

        var claims = new Dictionary<string, object>
        {
            ["sub"] = employee.Id.ToString(),
            ["user_type"] = "Employee",
            ["tenant_id"] = employee.TenantId.ToString(),
            ["outlet_id"] = outletId.ToString(),
            ["device_id"] = deviceId.ToString(),
            ["name"] = employee.DisplayName,
            ["role"] = roleNames,
            ["permissions"] = permissions,
            ["iss"] = _issuer,
            ["aud"] = _audience,
            ["exp"] = DateTimeOffset.UtcNow.Add(lifetime ?? TimeSpan.FromHours(12)).ToUnixTimeSeconds()
        };

        return CreateJwt(claims);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return null;

        var parts = token.Split('.');
        if (parts.Length != 3) return null;

        var headerJson = Base64UrlDecode(parts[0]);
        var payloadJson = Base64UrlDecode(parts[1]);
        var signature = parts[2];

        var unsignedData = $"{parts[0]}.{parts[1]}";
        using var hmac = new HMACSHA256(_key);
        var computedSig = Base64UrlEncode(hmac.ComputeHash(Encoding.UTF8.GetBytes(unsignedData)));

        if (!CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(signature), Encoding.UTF8.GetBytes(computedSig)))
        {
            return null;
        }

        try
        {
            using var doc = JsonDocument.Parse(payloadJson);
            var root = doc.RootElement;

            if (root.TryGetProperty("exp", out var expProp) && expProp.TryGetInt64(out var expUnix))
            {
                if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() > expUnix)
                {
                    return null; // Expired
                }
            }

            var identity = new ClaimsIdentity("JwtBearer");

            foreach (var prop in root.EnumerateObject())
            {
                if (prop.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in prop.Value.EnumerateArray())
                    {
                        identity.AddClaim(new Claim(prop.Name, item.GetString() ?? ""));
                    }
                }
                else
                {
                    identity.AddClaim(new Claim(prop.Name, prop.Value.GetString() ?? prop.Value.ToString() ?? ""));
                }
            }

            return new ClaimsPrincipal(identity);
        }
        catch
        {
            return null;
        }
    }

    private string CreateJwt(Dictionary<string, object> payload)
    {
        var header = new { alg = "HS256", typ = "JWT" };
        var headerBase64 = Base64UrlEncode(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(header)));
        var payloadBase64 = Base64UrlEncode(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload)));

        var unsigned = $"{headerBase64}.{payloadBase64}";
        using var hmac = new HMACSHA256(_key);
        var signature = Base64UrlEncode(hmac.ComputeHash(Encoding.UTF8.GetBytes(unsigned)));

        return $"{unsigned}.{signature}";
    }

    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    private static string Base64UrlDecode(string input)
    {
        var output = input.Replace('-', '+').Replace('_', '/');
        switch (output.Length % 4)
        {
            case 2: output += "=="; break;
            case 3: output += "="; break;
        }
        return Encoding.UTF8.GetString(Convert.FromBase64String(output));
    }
}

using System.Security.Claims;
using CoffeePos.Domain.Identity;

namespace CoffeePos.Application.Interfaces;

public interface IPasswordHasher
{
    (string Hash, string Salt) HashPassword(string password);
    bool VerifyPassword(string password, string hash, string salt);
    (string Hash, string Salt) HashPin(string pin);
    bool VerifyPin(string pin, string hash, string salt);
}

public interface ITokenService
{
    string GenerateUserToken(User user, Guid? outletId = null, TimeSpan? lifetime = null);
    string GenerateEmployeeToken(Employee employee, Guid outletId, Guid deviceId, TimeSpan? lifetime = null);
    ClaimsPrincipal? ValidateToken(string token);
}

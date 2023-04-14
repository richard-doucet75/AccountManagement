using AccountServices.UseCases.ValueTypes;

namespace AccountServices.Services;

public interface IPasswordHasher
{
    Task<string> Hash(Password password);
    Task<bool> Verify(Password password, string passwordHash);
}
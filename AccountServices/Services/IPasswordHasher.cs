using AccountServices.UseCases.ValueTypes;

namespace AccountServices.Services;

public interface IPasswordHasher
{
    public Task<string> Hash(Password password);
}
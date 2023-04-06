using AccountServices.Services;
using AccountServices.UseCases.ValueTypes;

namespace AccountServices.Tests.UseCases.Services;

public class PasswordHasher : IPasswordHasher
{
    private readonly string _passwordHash;

    public PasswordHasher(string passwordHash)
    {
        _passwordHash = passwordHash;
    }
    
    public async Task<string> Hash(Password password)
    {
        return await Task.FromResult(_passwordHash);
    }
}
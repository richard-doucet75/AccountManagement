using AccountServices.Services;
using AccountServices.UseCases.ValueTypes;

namespace AccountServices.Tests.Services;

public class PasswordHasher : IPasswordHasher
{
    private readonly string _passwordHash;
    private readonly bool _verify;

    public PasswordHasher(string passwordHash, bool verify = false)
    {
        _passwordHash = passwordHash;
        _verify = verify;
    }
    
    public async Task<string> Hash(Password password)
    {
        return await Task.FromResult(_passwordHash);
    }

    public async Task<bool> Verify(Password password, string passwordHash)
    {
        return await Task.FromResult(_verify);

    }
}
using AccountServices.Services;
using AccountServices.UseCases.ValueTypes;
using System.Security.Cryptography;

namespace AccountServices.Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 128 / 8;
    private const int KeySize = 256 / 8;
    private const int Iterations = 100000;
    private static readonly HashAlgorithmName HashAlgorithmName = HashAlgorithmName.SHA256;
    private const char Delimiter = ';';

    public async Task<string> Hash(Password password)
    {
        return await Task.Run(() =>
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName, KeySize);
            return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        });
    }

    public async Task<bool> Verify(Password password, string passwordHash)
    {
        return await Task.Run(() =>
        {
            var elements = passwordHash.Split(Delimiter);
            var salt = Convert.FromBase64String(elements[0]);
            var hash = Convert.FromBase64String(elements[1]);
            
            var inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName, KeySize);

            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        });
    }
}
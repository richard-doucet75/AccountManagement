using AccountServices.Infrastructure.Services;
using NUnit.Framework;

namespace AccountServices.Infrastructure.Tests.Services
{
    public class PasswordHasherTests
    {
        [Test]
        public async Task HashDoesNotEqualOriginal()
        {
            const string password = "Pa$$word1";

            var hasher = new PasswordHasher();
            Assert.That(await hasher.Hash(password), Is.Not.EqualTo(password));
                
        }

        [Test]
        public async Task MatchesOnlyWhenSame()
        {
            const string password = "Pa$$word1";
            var hasher = new PasswordHasher();

            var hash = await hasher.Hash(password);
            Assert.Multiple(async () =>
            {
                Assert.That(await hasher.Verify(password, hash));
                Assert.That(await hasher.Verify(password + "X", hash), Is.False);
            });
        }
    }
}

using AccountServices.Gateways.Entities;
using AccountServices.Infrastructure.Gateways;
using AccountServices.UseCases.ValueTypes;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace AccountServices.Infrastructure.Tests.Gateways
{
    public class AccountGatewayTests : IDisposable
    {
        private AccountDbContext? _context;
        private AccountGateway? _gateway;

        [SetUp] 
        public void Setup()
        {
            var option = new DbContextOptionsBuilder<AccountDbContext>()
                .UseInMemoryDatabase("Database")
                .Options;
                
            _context = new AccountDbContext(option);
            _gateway = new AccountGateway(_context);

            _context.Database.EnsureCreated();
        }

        [Test]
        public async Task NotExist()
        {
            Assert.That(await _gateway!.Exist("email.address.does.not.exist@domain.com"), Is.False);
        }

        [Test]
        public async Task Exist()
        {
            const string emailAddress = "email.address.exists@domain.com";
            _context!.Accounts.Add(new Account(Guid.Empty, emailAddress, string.Empty));
            Assert.That(await _gateway!.Exist("email.address.does.not.exist@domain.com"), Is.False);
        }

        [Test]
        public async Task Create()
        {
            const string emailAddress = "new.email.address@domain.com";
            await _gateway!.Create(new Account(Guid.Empty, emailAddress, string.Empty));

            var account = _context!
                .Accounts
                .SingleOrDefault(a => a.EmailAddress == emailAddress);
            
            Assert.That(account, Is.Not.Null);
            Assert.That(account!.Id, Is.Not.EqualTo(Guid.Empty));
        }
        
        [Test]
        public async Task Update()
        {
            const string emailAddress = "email.address@domain.com";
            const string newerEmailAddress = "newer.email@domain.com";
            await _gateway!.Create(new Account(Guid.Empty, emailAddress, string.Empty));

            var account = _context!
                .Accounts
                .SingleOrDefault(a => a.EmailAddress == emailAddress);
            
            account!.EmailAddress = newerEmailAddress;
            await _gateway!.Update(account);
            
            var updatedAccount = _context!
                .Accounts
                .SingleOrDefault(a => a.Id == account.Id);
            
            Assert.That(updatedAccount!.EmailAddress, Is.EqualTo((EmailAddress)newerEmailAddress));
        }

        public void Dispose()
        {
            _context!.Dispose();
        }
    }
}

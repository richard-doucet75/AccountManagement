using AccountServices.Gateways;
using AccountServices.Gateways.Entities;
using AccountServices.UseCases.ValueTypes;
using Microsoft.EntityFrameworkCore;

namespace AccountServices.Infrastructure.Gateways;

public sealed class AccountGateway : IAccountGateway
{
    private readonly AccountDbContext _context;

    public AccountGateway(AccountDbContext context)
    {
        _context = context;
    }
    public async Task Create(Account account)
    {
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Exist(EmailAddress emailAddress)
    {
        return await _context.Accounts
            .AnyAsync(a => a.EmailAddress == emailAddress);
    }

    public async Task<Account?> Find(EmailAddress emailAddress)
    {
        return await _context.Accounts
            .SingleOrDefaultAsync(c => c.EmailAddress == emailAddress);
    }
}
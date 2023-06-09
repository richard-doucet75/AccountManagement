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
    public async Task<Account> Create(Account account)
    {
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return account;
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

    public async Task<Account?> Find(Guid accountId)
    {
        return await _context.Accounts
            .SingleOrDefaultAsync(c => c.Id == accountId);
    }

    public async Task Update(Account account)
    {
        _context.Update(account);
        await _context.SaveChangesAsync();
    }
}
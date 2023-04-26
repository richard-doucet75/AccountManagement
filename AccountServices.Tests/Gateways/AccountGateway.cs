using AccountServices.Gateways;
using AccountServices.Gateways.Entities;
using AccountServices.UseCases.ValueTypes;

namespace AccountServices.Tests.Gateways;

public class AccountGateway : IAccountGateway
{
    private readonly GatewayMode _gatewayMode;
    private readonly List<Account> _accounts;

    public AccountGateway(GatewayMode gatewayMode)
    {
        _gatewayMode = gatewayMode;
        _accounts = new List<Account>();
    }

    public bool AccountsContains(EmailAddress emailAddress)
    {
        return _accounts.Any(a=>a.EmailAddress == emailAddress);
    }
    
    public async Task<Account> Create(Account account)
    {
        var storable = (Account) account.Clone();
        storable.Id = Guid.NewGuid();
        
        await _gatewayMode.Execute(Task.Run(() => _accounts.Add(storable)));
        return (Account)storable.Clone();
    }

    public async Task<bool> Exist(EmailAddress emailAddress)
    {
        var exists = false;
        await Task.Run(() =>
            {
                exists = _accounts.Any(a => a.EmailAddress == emailAddress);
            }
        );
        return exists;
    }

    public async Task<Account?> Find(EmailAddress emailAddress)
    {
        return await Task.Run(() =>
            _accounts.SingleOrDefault(a => a.EmailAddress == emailAddress)
                ?.Clone() as Account
        );
    }

    public async Task<Account?> Find(Guid accountId)
    {
        return await Task.Run(() =>
            _accounts.SingleOrDefault(a => a.Id == accountId)?.Clone() as Account
        );
    }

    public async Task Update(Account account)
    {
        await Task.Run(() =>
        {
            var itemsRemoved = _accounts.Where(c => c.Id != account.Id).ToList();
            _accounts.Clear();
            _accounts.AddRange(itemsRemoved);
            _accounts.Add((Account)account.Clone());
        });
    }
}
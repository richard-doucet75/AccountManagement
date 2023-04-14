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
    
    public async Task Create(Account account)
    {
        await _gatewayMode.Execute(Task.Run(() => _accounts.Add(account)));
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
        );
    }
}
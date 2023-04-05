using AccountServices.Gateways;
using AccountServices.Gateways.Entities;
using AccountServices.UseCases.ValueTypes;

namespace AccountServicesTests.Gateways;

public class AccountGateway : IAccountGateway
{
    private readonly GatewayMode _gatewayMode;
    private readonly List<Account> _emailAddresses;

    public AccountGateway(GatewayMode gatewayMode)
    {
        _gatewayMode = gatewayMode;
        _emailAddresses = new List<Account>();
    }

    public bool AccountsContain(EmailAddress emailAddress)
    {
        return _emailAddresses.Any(a=>a.EmailAddress == emailAddress);
    }
    
    public Account? Find(EmailAddress emailAddress)
    {
        return _emailAddresses.SingleOrDefault(a=>a.EmailAddress == emailAddress);
    }

    public async Task Create(Account account)
    {
        await _gatewayMode.Execute(Task.Run(() => _emailAddresses.Add(account)));
    }

    public async Task<bool> Exist(EmailAddress emailAddress)
    {
        var exists = false;
        await Task.Run(() =>
            {
                exists = _emailAddresses.Any(a => a.EmailAddress == emailAddress);
            }
        );
        return exists;
    }
}
using AccountServices.Gateways;
using AccountServices.Gateways.Entities;
using AccountServices.Services;
using AccountServices.UseCases.ValueTypes;

namespace AccountServices.UseCases;

public class CreateAccount
{
    public const string UnexpectedGatewayError = "UNEXPECTED_ACCOUNT_GATEWAY_ERROR";
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAccountGateway _accountGateway;

    public CreateAccount(IPasswordHasher passwordHasher,
        IAccountGateway accountGateway)
    {
        _passwordHasher = passwordHasher;
        _accountGateway = accountGateway;
    }

    public interface IPresenter
    {
        Task PresentAccountCreateError(string message);
        Task PresentAccountCreated();
        Task PresentAccountExists();
        Task PresentPasswordMismatch();
    }
    
    public async Task Execute(IPresenter presenter, EmailAddress emailAddress, Password password, Password verifyPassword)
    {
        if (await PresentWhenPasswordsMatch(presenter, password, verifyPassword)) return;
        if (await PresentWhenEmailInUse(presenter, emailAddress)) return;
        
        await PresentCreateNewAccount(presenter, emailAddress, password);
    }

    private static async Task<bool> PresentWhenPasswordsMatch(IPresenter presenter, Password password, Password verifyPassword)
    {
        if (password == verifyPassword) return false;

        await presenter.PresentPasswordMismatch();
        return true;
    }

    private async Task<bool> PresentWhenEmailInUse(IPresenter presenter, EmailAddress emailAddress)
    {
        if (!await _accountGateway.Exist(emailAddress)) return false;
        
        await presenter.PresentAccountExists();
        return true;

    }
    
    private async Task PresentCreateNewAccount(IPresenter presenter, EmailAddress emailAddress, Password password)
    {
        var passwordHash = await _passwordHasher.Hash(password);
        await _accountGateway.Create(new Account(emailAddress, passwordHash))
            .ContinueWith(r =>
                r.Exception is null
                    ? presenter.PresentAccountCreated()
                    : presenter.PresentAccountCreateError(UnexpectedGatewayError));
    }
}
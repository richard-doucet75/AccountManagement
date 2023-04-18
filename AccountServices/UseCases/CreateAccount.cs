using AccountServices.Gateways;
using AccountServices.Gateways.Entities;
using AccountServices.UseCases.Models;
using AccountServices.UseCases.ValueTypes;

namespace AccountServices.UseCases;

public class CreateAccount
{
    public const string UnexpectedGatewayError = "UNEXPECTED_ACCOUNT_GATEWAY_ERROR";
    private readonly IAccountGateway _accountGateway;


    public CreateAccount(IAccountGateway accountGateway)
    {
        _accountGateway = accountGateway;
    }

    public interface IPresenter
    {
        Task PresentAccountCreateError(string message);
        Task PresentAccountCreated();
        Task PresentAccountExists();
        Task PresentPasswordMismatch();
    }
    
    public async Task Execute(IPresenter presenter, 
        CreateAccountModel model)
    {
        if (await PresentWhenPasswordsMatch(presenter, model.Password, model.VerifyPassword)) return;
        if (await PresentWhenEmailInUse(presenter, model.EmailAddress)) return;
        
        await PresentCreateNewAccount(presenter, model.EmailAddress, model.Password);
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
        var passwordHash = await password.Hash();
        await _accountGateway.Create(new Account(Guid.Empty, emailAddress, passwordHash))
            .ContinueWith(r =>
                r.Exception is null
                    ? presenter.PresentAccountCreated()
                    : presenter.PresentAccountCreateError(UnexpectedGatewayError));
    }
}
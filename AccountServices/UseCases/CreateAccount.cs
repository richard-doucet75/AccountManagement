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
        Task PresentAccountCreated(Guid accountId);
        Task PresentAccountExists();
        Task PresentPasswordMismatch();
    }
    
    public async Task Execute(IPresenter presenter, 
        CreateAccountModel model)
    {
        if (await WhenPasswordsMatch(presenter, model.Password, model.VerifyPassword)) return;
        if (await WhenEmailInUse(presenter, model.EmailAddress)) return;
        
        await WhenCreatingNewAccount(presenter, model.EmailAddress, model.Password);
    }

    private static async Task<bool> WhenPasswordsMatch(IPresenter presenter, Password password, Password verifyPassword)
    {
        if (password == verifyPassword) return false;

        await presenter.PresentPasswordMismatch();
        return true;
    }

    private async Task<bool> WhenEmailInUse(IPresenter presenter, EmailAddress emailAddress)
    {
        if (!await _accountGateway.Exist(emailAddress)) return false;
        
        await presenter.PresentAccountExists();
        return true;

    }
    
    private async Task WhenCreatingNewAccount(IPresenter presenter, EmailAddress emailAddress, Password password)
    {
        var passwordHash = await password.Hash();
        try
        {
            var account = await _accountGateway.Create(new Account(Guid.Empty, emailAddress, passwordHash));
            await presenter.PresentAccountCreated(account.Id);
        }
        catch
        { 
            await presenter.PresentAccountCreateError(UnexpectedGatewayError);
        }
    }
}
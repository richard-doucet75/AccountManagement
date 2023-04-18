using AccountServices.Gateways;
using AccountServices.Gateways.Entities;
using AccountServices.UseCases.Models;
using AccountServices.UseCases.ValueTypes;

namespace AccountServices.UseCases;

public class ChangePassword
{
    private readonly IAccountGateway _accountGateway;

    public interface IPresenter
    {
        Task PresentAccountNotFound();
        Task PresentWrongPassword();
        Task PresentPasswordNotVerified();
        Task PasswordChanged();
    }

    public ChangePassword(IAccountGateway accountGateway)
    {
        _accountGateway = accountGateway;
    }
    
    public async Task Execute(
        IPresenter presenter, 
        IUserContext userContext, 
        ChangePasswordModel model)
    {
        var account = userContext.AccountId.HasValue 
            ? await _accountGateway.Find(userContext.AccountId.Value)
            : null;

        if (!(await PresentAccountNotFound(presenter, account)
              || await PresentPasswordNotMatched(presenter, account!, model.OldPassword)
              || await PresentPasswordNotVerified(presenter, model.NewPassword, model.VerifyPassword)))
        {
            account!.PasswordHash = await model.NewPassword.Hash();
            await _accountGateway.Update(account!);
            await presenter.PasswordChanged();
        }
    }

    private static async Task<bool> PresentAccountNotFound(IPresenter presenter, Account? account)
    {
        var presentable = account == null;
        if(presentable)
            await presenter.PresentAccountNotFound();

        return presentable;
    }

    private async Task<bool> PresentPasswordNotMatched(IPresenter presenter, Account account, Password oldPassword)
    {
        var presentable = !await oldPassword.Verify(account.PasswordHash);
        if (presentable)
            await presenter.PresentWrongPassword();

        return presentable;
    }
    
    private static async Task<bool> PresentPasswordNotVerified(IPresenter presenter, Password newPassword, Password verifyPassword)
    {
        var presentable = newPassword != verifyPassword;
        if (presentable)
        {
            await presenter.PresentPasswordNotVerified();
        }

        return presentable;
    }
}
using AccountServices.UseCases;

namespace AccountServicesApi.Presenters;

public class CreateAccountPresenter : CreateAccount.IPresenter
{
    public IResult Result { get; private set; } = Results.BadRequest("Unexpected");

    public async Task PresentAccountCreateError(string message)
    {
        Result = await Task.FromResult(Results.BadRequest(message));
    }

    public async Task PresentAccountCreated()
    {
        Result = await Task.FromResult(Results.Ok());
    }

    public async Task PresentAccountExists()
    {
        Result = await Task.FromResult(Results.BadRequest("Account already exists"));
    }

    public async Task PresentPasswordMismatch()
    {
        Result = await Task.FromResult(Results.BadRequest("Passwords must match"));
    }
}
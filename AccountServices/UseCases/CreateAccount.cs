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
    
    private interface IHandler
    {
        Task Execute();
    }

    private class HandlerFactory
    {
        private readonly IAccountGateway _accountGateway;

        public HandlerFactory(IAccountGateway accountGateway)
        {
            _accountGateway = accountGateway;
        }

        public async Task<IHandler> CreateHandler(IPresenter presenter, EmailAddress emailAddress, Password password, Password verifyPassword)
        {
            if(password != verifyPassword)
                return new HandlePasswordDoesntMatch(presenter);

            if (await _accountGateway.Exist(emailAddress))
                return new HandleEmailInUse(presenter);

            return new HandleCreatingNewAccount(_accountGateway, presenter, emailAddress, password);
        }

        private class HandlePasswordDoesntMatch : IHandler
        {
            private readonly IPresenter _presenter;

            public HandlePasswordDoesntMatch(IPresenter presenter)
            {
                _presenter = presenter;
            }
            
            public async Task Execute()
            {
                await _presenter.PresentPasswordMismatch();
            }
        }

        private class HandleEmailInUse : IHandler
        {
            private readonly IPresenter _presenter;

            public HandleEmailInUse(IPresenter presenter)
            {
                _presenter = presenter;
            }
            
            public async Task Execute()
            {
                await _presenter.PresentAccountExists();
            }
        }
        
        private class HandleCreatingNewAccount: IHandler
        {
            private readonly IAccountGateway _accountGateway;
            private readonly IPresenter _presenter;
            private readonly EmailAddress _emailAddress;
            private readonly Password _password;

            public HandleCreatingNewAccount(IAccountGateway accountGateway, IPresenter presenter, EmailAddress emailAddress, Password password)
            {
                _accountGateway = accountGateway;
                _presenter = presenter;
                _emailAddress = emailAddress;
                _password = password;
            }
            
            public async Task Execute()
            {
                var passwordHash = await _password.Hash();
                try
                {
                    var account = await _accountGateway.Create(new Account(Guid.Empty, _emailAddress, passwordHash));
                    await _presenter.PresentAccountCreated(account.Id);
                }
                catch
                { 
                    await _presenter.PresentAccountCreateError(UnexpectedGatewayError);
                }
            }
        }
    }

    public async Task Execute(
        IPresenter presenter, 
        CreateAccountModel model)
    {
        var factory = new HandlerFactory(_accountGateway);
        var handler = await factory.CreateHandler(presenter, model.EmailAddress, model.Password, model.VerifyPassword);
        await handler.Execute();
    }
}
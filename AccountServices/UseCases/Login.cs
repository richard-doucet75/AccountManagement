using AccountServices.Gateways;
using AccountServices.Gateways.Entities;
using AccountServices.UseCases.Models;
using AccountServices.UseCases.ValueTypes;

namespace AccountServices.UseCases
{
    public class Login
    {
        private readonly IAccountGateway _accountGateway;

        public interface IPresenter
        {
            Task PresentNotFound();
            Task PresentAccessDenied();
            Task PresentSuccess(Guid accountId, EmailAddress emailAddress);
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
            
            public async Task<IHandler> CreateHandler(IPresenter presenter, EmailAddress emailAddress, Password password)
            { 
                var account = await _accountGateway.Find(emailAddress);
                if (account is null)
                    return new HandleAccountNotFound(presenter);
                if (!await password.Verify(account.PasswordHash))
                    return new HandleAccessDenied(presenter);

                return new HandleLoginSuccess(presenter, account);
            }

            private class HandleAccountNotFound : IHandler
            {
                private readonly IPresenter _presenter;

                public HandleAccountNotFound(IPresenter presenter)
                {
                    _presenter = presenter;
                }

                public async Task Execute()
                {
                    await _presenter.PresentNotFound();
                }
            }

            private class HandleAccessDenied : IHandler
            {
                private readonly IPresenter _presenter;
                public HandleAccessDenied(IPresenter presenter)
                {
                    _presenter = presenter;
                }
                
                public async Task Execute()
                {
                    await _presenter.PresentAccessDenied();
                }
            }
            
            private class HandleLoginSuccess : IHandler
            {
                private readonly IPresenter _presenter;
                private readonly Account _account;

                public HandleLoginSuccess(IPresenter presenter, Account account)
                {
                    _presenter = presenter;
                    _account = account;
                }

                public async Task Execute()
                {
                    await _presenter.PresentSuccess(_account.Id, _account.EmailAddress);
                }
            }
        }

        public Login(IAccountGateway accountGateway)
        {
            _accountGateway = accountGateway;
        }

        public async Task Execute(IPresenter presenter, LoginModel model)
        {
            var factory = new HandlerFactory(_accountGateway);
            var handler = await factory.CreateHandler(presenter, model.EmailAddress, model.Password);
            await handler.Execute();
        }
    }
}

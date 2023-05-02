using AccountServices.Gateways;
using AccountServices.Gateways.Entities;
using AccountServices.UseCases.Models;
using AccountServices.UseCases.ValueTypes;

namespace AccountServices.UseCases
{
    public class GetAccount
    {
        private readonly IAccountGateway _accountGateway;

        public record PresentableAccount(Guid Id,  EmailAddress EmailAddress);

        public GetAccount(IAccountGateway accountGateway)
        {
            _accountGateway = accountGateway;
        }
        
        public interface IPresenter
        {
            Task PresentAccountNotFound();
            Task Present(PresentableAccount presentableAccount);
            Task PresentAccessDenied();
        }

        private interface IHandler
        {
            Task Execute();
        }

        private class HandlerFactory
        {
            private readonly IUserContext _userContext;
            private readonly IAccountGateway _accountGateway;

            public HandlerFactory(IUserContext userContext, IAccountGateway accountGateway)
            {
                _userContext = userContext;
                _accountGateway = accountGateway;
            }

            public async Task<IHandler> CreateHandler(IPresenter presenter, Guid accountId)
            {
                if (_userContext.AccountId is null
                    || _userContext.AccountId != accountId)
                    return new HandleAccessDenied(presenter);
                
                var account = await _accountGateway.Find(accountId);
                if (account is null)
                {
                    return new HandleAccountNotFound(presenter);
                }

                return new HandlePresentAccount(presenter, account);
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
            
            private class HandleAccountNotFound : IHandler
            {
                private readonly IPresenter _presenter;

                public HandleAccountNotFound(IPresenter presenter)
                {
                    _presenter = presenter;
                }
                
                public async Task Execute()
                {
                    await _presenter.PresentAccountNotFound();
                }
            }

            private class HandlePresentAccount : IHandler
            {
                private readonly IPresenter _presenter;
                private readonly Account _account;

                public HandlePresentAccount(IPresenter presenter, Account account)
                {
                    _presenter = presenter;
                    _account = account;
                }

                public async Task Execute()
                {
                    await _presenter.Present(new PresentableAccount(_account.Id, _account.EmailAddress));
                }
            }
        }
            
        public async Task Execute(IPresenter presenter, IUserContext userContext, Guid accountId)
        {
            var factory = new HandlerFactory(userContext, _accountGateway);
            var handler = await factory.CreateHandler(presenter, accountId);
            await handler.Execute();
        }
    }
}

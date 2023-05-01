using AccountServices.Gateways;
using AccountServices.Gateways.Entities;
using AccountServices.UseCases.Models;
using AccountServices.UseCases.ValueTypes;

namespace AccountServices.UseCases;

public class ChangeEmailAddress
{
    private readonly IAccountGateway _accountGateway;

    public interface IPresenter
    {
        Task PresentNotFound();
        Task PresentAccessDenied();
        Task PresentNoChangeRequired();
        Task PresentEmailAddressChanged();
    }

    public ChangeEmailAddress(IAccountGateway accountGateway)
    {
        _accountGateway = accountGateway;
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

        public async Task<IHandler> CreateHandler(
            IPresenter presenter, 
            IUserContext userContext, 
            Guid accountId,
            EmailAddress emailAddress)
        {
            if (userContext.AccountId != accountId)
                return new HandleAccessDenied(presenter);

            var account = await _accountGateway.Find(accountId); 
            if(account == null) 
                return new HandleAccountNotFound(presenter);

            if (emailAddress == account.EmailAddress)
                return new HandleNoChangeRequired(presenter);

            return new HandleChangeEmail(
                presenter, 
                _accountGateway, 
                account, emailAddress);
        }

        private class HandleAccountNotFound
            : IHandler
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

        private class HandleAccessDenied
            : IHandler
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

        private class HandleNoChangeRequired
            : IHandler
        {
            private readonly IPresenter _presenter;

            public HandleNoChangeRequired(IPresenter presenter)
            {
                _presenter = presenter;
            }

            public async Task Execute()
            {
                await _presenter.PresentNoChangeRequired();
            }
        }

        private class HandleChangeEmail
            : IHandler
        {
            private readonly IPresenter _presenter;
            private readonly IAccountGateway _accountGateway;
            private readonly Account _account;
            private readonly EmailAddress _emailAddress;

            public HandleChangeEmail(IPresenter presenter, IAccountGateway accountGateway,
                Account account, EmailAddress emailAddress)
            {
                _presenter = presenter;
                _accountGateway = accountGateway;
                _account = account;
                _emailAddress = emailAddress;
            }

            public async Task Execute()
            {
                _account.EmailAddress = _emailAddress;
                await _accountGateway.Update(_account);
                await _presenter.PresentEmailAddressChanged();
            }
        }
    }

    public async Task Execute(
        IPresenter presenter,
        IUserContext userContext,
        Guid accountId,
        EmailAddress newEmailAddress)
    {
        var factory = new HandlerFactory(_accountGateway);
        var handler = await factory.CreateHandler(presenter, userContext, accountId, newEmailAddress);

        await handler.Execute();
    }
}
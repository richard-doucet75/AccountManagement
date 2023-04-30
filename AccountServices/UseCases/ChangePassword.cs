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
        Task PresentAccessDenied();
        Task PresentAccountNotFound();
        Task PresentWrongPassword();
        Task PresentPasswordNotVerified();
        Task PasswordChanged();
    }

    public ChangePassword(IAccountGateway accountGateway)
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

        public async Task<IHandler> CreateHandler(IPresenter presenter, IUserContext userContext, Guid accountId,
            ChangePasswordModel model)
        {
            if (userContext.AccountId == null
                || userContext.AccountId.Value != accountId)
            {
                return new HandleAccessDenied(presenter);
            }

            var account = await _accountGateway.Find(accountId);
            if (account is null)
            {
                return new HandleAccountNotFound(presenter);
            }

            if (!await model.OldPassword.Verify(account.PasswordHash))
            {
                return new HandleWrongPassword(presenter);
            }

            if (model.NewPassword != model.VerifyPassword)
            {
                return new HandlePasswordNotVerified(presenter);
            }

            return new HandleChangePassword(presenter, _accountGateway, account, model.NewPassword);
        }

        private class HandleAccessDenied : IHandler
        {
            private readonly IPresenter _presenter;

            public HandleAccessDenied(IPresenter presenter)
            {
                _presenter = presenter;
            }

            public Task Execute()
            {
                return _presenter.PresentAccessDenied();
            }
        }

        private class HandleAccountNotFound : IHandler
        {
            private readonly IPresenter _presenter;

            public HandleAccountNotFound(IPresenter presenter)
            {
                _presenter = presenter;
            }

            public Task Execute()
            {
                return _presenter.PresentAccountNotFound();
            }
        }

        private class HandleWrongPassword : IHandler
        {
            private readonly IPresenter _presenter;

            public HandleWrongPassword(IPresenter presenter)
            {
                _presenter = presenter;
            }

            public Task Execute()
            {
                return _presenter.PresentWrongPassword();
            }
        }

        private class HandlePasswordNotVerified : IHandler
        {
            private readonly IPresenter _presenter;

            public HandlePasswordNotVerified(IPresenter presenter)
            {
                _presenter = presenter;
            }

            public Task Execute()
            {
                return _presenter.PresentPasswordNotVerified();
            }
        }

        private class HandleChangePassword : IHandler
        {
            private readonly IPresenter _presenter;
            private readonly IAccountGateway _accountGateway;
            private readonly Account _account;
            private readonly Password _newPassword;

            public HandleChangePassword(IPresenter presenter, IAccountGateway accountGateway, Account account, Password newPassword)
            {
                _presenter = presenter;
                _accountGateway = accountGateway;
                _account = account;
                _newPassword = newPassword;
            }

            public async Task Execute()
            {
                _account.PasswordHash = await _newPassword.Hash();
                await _accountGateway.Update(_account);
                await _presenter.PasswordChanged();
            }
        }
    }

    public async Task Execute(
        IPresenter presenter, 
        IUserContext userContext, 
        Guid accountId,
        ChangePasswordModel model)
    {
        var handlerFactory = new HandlerFactory(_accountGateway);
        var handler = await handlerFactory.CreateHandler(presenter, userContext, accountId, model);

        await handler.Execute();
    }
}
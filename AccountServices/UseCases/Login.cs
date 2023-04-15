using AccountServices.Gateways;
using AccountServices.Services;
using AccountServices.UseCases.ValueTypes;

namespace AccountServices.UseCases
{
    public class Login
    {
        private readonly IAccountGateway _accountGateway;
        private readonly IPasswordHasher _passwordHasher;

        public interface IPresenter
        {
            Task PresentNotFound();
            Task PresentAccessDenied();
            Task PresentSuccess(Guid accountId, EmailAddress emailAddress);
        }

        public Login(IAccountGateway accountGateway, IPasswordHasher passwordHasher)
        {
            _accountGateway = accountGateway;
            _passwordHasher = passwordHasher;
        }

        public async Task Execute(IPresenter presenter, EmailAddress emailAddress, Password password)
        {
            var account = await _accountGateway.Find(emailAddress);
            if (account == null)
            {
                await presenter.PresentNotFound();
                return;
            }

            if (!await _passwordHasher.Verify(password, account.PasswordHash))
            {
                await presenter.PresentAccessDenied();
                return;
            }

            await presenter.PresentSuccess(account.Id, emailAddress);
        }
    }
}

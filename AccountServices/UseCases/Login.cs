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
            void PresentNotFound();
            void PresentAccessDenied();
            void PresentSuccess();
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
                presenter.PresentNotFound();
                return;
            }

            if (!await _passwordHasher.Verify(password, account.PasswordHash))
            {
                presenter.PresentAccessDenied();
            }

            presenter.PresentSuccess();
        }
    }
}

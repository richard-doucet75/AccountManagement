using AccountServices.Gateways;
using AccountServices.Gateways.Entities;
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

        public Login(IAccountGateway accountGateway)
        {
            _accountGateway = accountGateway;
        }

        public async Task Execute(IPresenter presenter, EmailAddress emailAddress, Password password)
        {
            var account = await _accountGateway.Find(emailAddress);
            if(!(await PresentAccountNotFound(presenter, account) || 
                    await PresentAccessDenied(presenter, account!, password))) 
                await PresentSuccess(presenter, account!);
        }

        private static async Task<bool> PresentAccountNotFound(IPresenter presenter, Account? account)
        {
            
            var presentable = account == null;
            if(presentable)
                await presenter.PresentNotFound();

            return presentable;
        }

        private async Task<bool> PresentAccessDenied(IPresenter presenter, Account account, Password password)
        {
            var presentable = !await password.Verify(account.PasswordHash);
            if(presentable)
                await presenter.PresentAccessDenied();

            return presentable;
        }
        
        private static async Task PresentSuccess(IPresenter presenter, Account account)
        {
            await presenter.PresentSuccess(account.Id, account.EmailAddress);
        }
    }
}

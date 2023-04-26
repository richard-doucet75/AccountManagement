using AccountServices.Gateways;
using AccountServices.UseCases.Models;
using AccountServices.UseCases.ValueTypes;

namespace AccountServices.UseCases
{
    public class GetAccount
    {
        private readonly IAccountGateway _accountGateway;

        public record PresentableAccount(Guid Id, EmailAddress EmailAddress);

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
        
        public async Task Execute(IPresenter presenter, IUserContext userContext, Guid accountId)
        {
            if (userContext.AccountId is null 
                || userContext.AccountId != accountId)
            {
                await presenter.PresentAccessDenied();
                return;
            }
            
            var account = await _accountGateway.Find(accountId);
            if (account is null)
            {
                await presenter.PresentAccountNotFound();
                return;
            }

            await presenter.Present(new PresentableAccount(account.Id, account.EmailAddress));
        }
    }
}

using AccountServices.Tests.Gateways;
using AccountServices.Tests.UseCases.Models;
using AccountServices.UseCases;
using AccountServices.UseCases.Models;

using NUnit.Framework;

using static AccountServices.UseCases.GetAccount;

namespace AccountServices.Tests.UseCases
{
    public class GetAccountTests
    {
        private Presenter? _presenter;
        private AccountGateway? _accountGateway;
        private GetAccount? _getAccount;


        private class Presenter : IPresenter
        {
            public bool AccountNotFoundPresented { get; private set; }
            public bool AccessDeniedPresented { get; private set; }
            public PresentableAccount? AccountPresented { get; private set; }

            public async Task PresentAccountNotFound()
            {
                AccountNotFoundPresented = true;
                await Task.CompletedTask;
            }

            public async Task Present(PresentableAccount presentableAccount)
            {
                AccountPresented = presentableAccount;
                await Task.CompletedTask;
            }

            public async Task PresentAccessDenied()
            {
                AccessDeniedPresented = true;
                await Task.CompletedTask;
            }
        }

        [SetUp]
        public void SetUp()
        {
            _presenter = new Presenter();
            _accountGateway = new AccountGateway(GatewayMode.SuccessMode);
            _getAccount = new GetAccount(_accountGateway!);
        }
        
       

        public class GivenUserContextAccountIdNotSet
            : GetAccountTests
        {
            [Test]
            public async Task PresentAccessDenied()
            {
                await _getAccount!.Execute(_presenter!, new UserContext(null), Guid.NewGuid());
                Assert.That(_presenter!.AccessDeniedPresented);
            }
        }

        public class GivenUserContextAccountIdSet
            : GetAccountTests
        {
            public class GivenUserContextAccountIdDoesNotMachAccountRequested:
                GivenUserContextAccountIdSet
            {
                [Test]
                public async Task PresentAccountNotFound()
                {
                    await _getAccount!.Execute(_presenter!, new UserContext(Guid.NewGuid()), Guid.NewGuid());
                    Assert.That(_presenter!.AccessDeniedPresented);
                }
            }

            public class GivenUserAccountIdMatchesAccountRequested:
                GivenUserContextAccountIdSet
            {
                public class GivenAccountDoesNotExits :
                    GivenUserContextAccountIdSet
                {
                    [Test]
                    public async Task PresentPresentableAccount()
                    {
                        var accountId = Guid.NewGuid();
                        await _getAccount!.Execute(_presenter!, new UserContext(accountId), accountId);

                        Assert.That(_presenter!.AccountNotFoundPresented);
                    }
                }

                public class GivenAccountExits :
                    GivenUserContextAccountIdSet
                {
                    public class AccountMatchesUserContext
                        : GivenAccountExits
                    {
                        [Test]
                        public async Task PresentPresentableAccount()
                        {
                            var createAccount = new CreateAccount(_accountGateway!);
                            await createAccount.Execute(new CreateAccountTests.Presenter(),
                                new CreateAccountModel("emailAddress@domain.com", "Pa$$w0rd", "Pa$$w0rd"));

                            var account = await _accountGateway!.Find("emailAddress@domain.com");
                            await _getAccount!.Execute(_presenter!, new UserContext(account!.Id), account.Id);

                            Assert.That(_presenter!.AccountPresented?.Id, Is.EqualTo(account.Id));
                        }
                    }

                    public class AccountDowsNotMatchesUserContext
                        : GivenAccountExits
                    {
                        [Test]
                        public async Task PresentPresentableAccount()
                        {
                            var createAccount = new CreateAccount(_accountGateway!);
                            await createAccount.Execute(new CreateAccountTests.Presenter(),
                                new CreateAccountModel("emailAddress@domain.com", "Pa$$w0rd", "Pa$$w0rd"));

                            var account = await _accountGateway!.Find("emailAddress@domain.com");
                            await _getAccount!.Execute(_presenter!, new UserContext(Guid.NewGuid()), account!.Id);

                            Assert.That(_presenter!.AccessDeniedPresented);
                        }
                    }
                }
            }
            
        }
    }
}

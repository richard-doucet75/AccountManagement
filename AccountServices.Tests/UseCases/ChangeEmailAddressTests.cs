using AccountServices.Tests.Gateways;
using AccountServices.Tests.UseCases.Models;
using AccountServices.UseCases;
using AccountServices.UseCases.Models;
using NUnit.Framework;

namespace AccountServices.Tests.UseCases
{
    public class ChangeEmailAddressTests
    {
        private const string NewEmailAddress = "new-eamil-address@domain.com";
        private AccountGateway? _accountGateway;
        private ChangeEmailAddress? _changeEmailAddress;

        private class Presenter : ChangeEmailAddress.IPresenter
        {
            public bool NotFoundPresented { get; private set; }
            public bool AccessDeniedPresented { get; private set; }
            public bool NoChangeRequiredPresented { get; private set; }
            public bool EmailAddressChangedPresented { get; private set; }

            public async Task PresentNotFound()
            {
                NotFoundPresented = true;
                await Task.CompletedTask;
            }

            public async Task PresentAccessDenied()
            {
                AccessDeniedPresented = true;
                await Task.CompletedTask;
            }

            public async Task PresentNoChangeRequired()
            {
                NoChangeRequiredPresented = true;
                await Task.CompletedTask;
            }

            public async Task PresentEmailAddressChanged()
            {
                EmailAddressChangedPresented = true;
                await Task.CompletedTask;
            }
        }

        [SetUp]
        public void SetUp()
        {
            _accountGateway = new AccountGateway(GatewayMode.SuccessMode);
            _changeEmailAddress = new ChangeEmailAddress(_accountGateway!);
        }

        public class GivenAccountIdDoesNotExist
            : ChangeEmailAddressTests
        {
            [Test]
            public async Task PresentNotFound()
            {
                var presenter = new Presenter();

                await _changeEmailAddress!.Execute(
                    presenter, 
                    new UserContext(Guid.NewGuid()), 
                    new ChangeEmailAddressModel(
                        Guid.NewGuid(), NewEmailAddress)
                    );
                Assert.That(presenter.NotFoundPresented);
            }
        }

        public class GivenAccountIdExists
            : ChangeEmailAddressTests
        {
            private const string CurrentEmailAddress = "local-part@domain.com";
            private Guid? _accountId;
            
            [SetUp]
            public async Task SetUpGivenAccountIdExists()
            {
                var accountPresenter = new CreateAccountTests.Presenter();
                var createAccount = new CreateAccount(_accountGateway!);
                await createAccount.Execute(accountPresenter,
                    new CreateAccountModel(
                        CurrentEmailAddress,
                        "Pa$$w0rd",
                        "Pa$$w0rd"));

                _accountId = accountPresenter.AccountCreated;
            }
            
            public class GivenAccountIdDoesNotMatchUserContext
                : GivenAccountIdExists
            {
                [Test]
                public async Task PresentAccessDenied()
                {
                    var presenter = new Presenter();

                    await _changeEmailAddress!.Execute(
                        presenter,
                        new UserContext(Guid.NewGuid()),
                        new ChangeEmailAddressModel(
                            _accountId!.Value,
                            NewEmailAddress));
                    
                    Assert.That(presenter.AccessDeniedPresented);
                }
            }

            public class GivenAccountIdMatchesUserContext
                : GivenAccountIdExists
            {
                public class GivenEmailAddressIsSameAsCurrent
                    : GivenAccountIdMatchesUserContext
                {
                    [Test]
                    public async Task NoChangeRequiredPresented()
                    {
                        var presenter = new Presenter();
                        await _changeEmailAddress!.Execute(
                            presenter,
                            new UserContext(_accountId!.Value),
                            new ChangeEmailAddressModel(
                                _accountId!.Value,
                                CurrentEmailAddress)
                            );

                        Assert.That(presenter.NoChangeRequiredPresented);
                    }
                }

                public class GivenEEmailAddressIsDifferentThanCurrent
                    : GivenAccountIdMatchesUserContext
                {
                    [Test]
                    public async Task NoChangeRequiredPresented()
                    {
                        var presenter = new Presenter();
                        await _changeEmailAddress!.Execute(
                            presenter,
                            new UserContext(_accountId!.Value),
                            new ChangeEmailAddressModel(
                                _accountId!.Value,
                                NewEmailAddress)
                            );

                        var accountWithNewEmail = await _accountGateway!.Find(NewEmailAddress);
                        var accountWithOriginalEmail = await _accountGateway!.Find(CurrentEmailAddress);
                        Assert.Multiple(() =>
                        {
                            Assert.That(presenter.EmailAddressChangedPresented);
                            Assert.That(accountWithNewEmail, Is.Not.Null);
                            Assert.That(accountWithNewEmail!.Id, Is.EqualTo(_accountId));
                            Assert.That(accountWithOriginalEmail, Is.Null);
                        });
                    }
                }
            }
        }
    }
}

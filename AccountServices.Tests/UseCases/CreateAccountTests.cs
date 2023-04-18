using NUnit.Framework;
using AccountServices.UseCases;
using AccountServices.Gateways.Entities;
using AccountServices.Tests.Gateways;
using AccountServices.UseCases.ValueTypes;
using static AccountServices.UseCases.CreateAccount;
using static AccountServices.Tests.Gateways.GatewayMode;
using AccountServices.UseCases.Models;

namespace AccountServices.Tests.UseCases;
public class CreateAccountTests
{
    private const string PasswordHash = "PasswordHash";
    private const string ValidPassword = "v@lidPassw0rd";
    private const string NewEmailAddress = "new-email-address@somedomain.com";
    
    private Presenter? _presenter;
    private AccountGateway? _gateway;
    private CreateAccount? _useCase;
    private EmailAddress? _emailAddress;

    #region Presenter 
    public class Presenter : IPresenter
    {
        public bool ExistingAccount { get; private set; }
        public bool AccountCreated { get; private set; }
        public string? AccountError { get; private set; }
        public bool PasswordMismatch { get; private set; }
        
        public async Task PresentAccountCreated()
        {
            await Task.Run(() =>
            {
                AccountCreated = true;
            });
        }

        public async Task PresentAccountExists()
        {
            await Task.Run(() =>
            {
                ExistingAccount = true;
            });
        }

        public async Task PresentPasswordMismatch()
        {
            await Task.Run(() =>
            {
                PasswordMismatch = true;
            });
        }

        public async Task PresentAccountCreateError(string message)
        {
            await Task.Run(() =>
            {
                AccountError = message;
            });
        }
    }
    #endregion

    [SetUp]
    public void SetUp()
    {
        _presenter = new Presenter();
    }

    public class GivenGatewaySuccess : CreateAccountTests
    {
        [SetUp]
        public void SetUpGivenGatewaySuccess()
        {
            _gateway = new AccountGateway(SuccessMode);
            _useCase = new CreateAccount(_gateway);    
        }
        
        
        public class GivenNewEmailAddress 
            : GivenGatewaySuccess
        {
            [SetUp]
            public void SetupGivenNewEmailAddress()
            {
                _emailAddress = NewEmailAddress;
            }

            public class GivenMatchingPasswords
                : GivenNewEmailAddress
            {
                [Test]
                public async Task AccountCreatedAndAccountCreatePresented()
                {
                    Assert.DoesNotThrowAsync(
                        () => _useCase!.Execute(
                            _presenter!, 
                            new CreateAccountModel(_emailAddress!, ValidPassword, ValidPassword)));

                    var account = await _gateway!.Find(_emailAddress!);

                    Assert.That(account, Is.Not.Null);
                    Assert.Multiple(() =>
                    {
                        Assert.That(account!.EmailAddress, Is.EqualTo(_emailAddress));
                        Assert.That(_presenter!.AccountCreated);
                    });
                    
                    Assert.That(await ((Password)ValidPassword).Verify(account!.PasswordHash));
                }
            }

            public class GivenMismatchedPasswords
                : GivenNewEmailAddress
            {
                [Test]
                public void AccountNotCreated()
                {
                    Assert.Multiple(() =>
                    {
                        Assert.DoesNotThrowAsync(
                            async () => await _useCase!.Execute(
                                _presenter!,
                                new CreateAccountModel(_emailAddress!, ValidPassword, "mismatchedP@ssw0rd")));
                        
                        Assert.That(_presenter!.AccountCreated, Is.False);
                        Assert.That(_presenter!.PasswordMismatch);
                        Assert.That(_gateway!.AccountsContains(_emailAddress!), Is.False);
                    });
                }
            }
        }

        public class GivenAnExistingEmailAddress
            : GivenGatewaySuccess
        {
            [SetUp]
            public async Task SetupGivenNewEmailAddress()
            {
                _emailAddress = "existing-email-address@somedomain.com";
                await _gateway!.Create(new Account(Guid.Empty, _emailAddress!, string.Empty));
            }

            public class GivenMatchingPasswords
                : GivenAnExistingEmailAddress
            {
                [Test]
                public void AccountNotCreatedAndExistingAccountPresented()
                {
                    Assert.Multiple(() =>
                    {
                        Assert.DoesNotThrowAsync(
                            async () => await _useCase!.Execute(
                                _presenter!,
                                new CreateAccountModel( _emailAddress!, ValidPassword, ValidPassword)));
                        
                        Assert.That(_presenter!.ExistingAccount);
                        Assert.That(_presenter!.AccountCreated, Is.False);
                    });
                }
            }
        }
    }
    
    public class GivenGatewayException : CreateAccountTests
    {
        [SetUp]
        public void SetUpGivenGatewayException()
        {
            _gateway = new AccountGateway(ExceptionMode);
            _useCase = new CreateAccount(_gateway!);
            _emailAddress = NewEmailAddress;
        }
        
        [Test]
        public void PresentUnexpectedGatewayError()
        {
            Assert.Multiple(() =>
            {
                Assert.DoesNotThrowAsync(
                    () => _useCase!.Execute(
                        _presenter!,
                        new CreateAccountModel(_emailAddress!, ValidPassword, ValidPassword)));
                
                Assert.That(_presenter!.AccountError, Is.EqualTo(UnexpectedGatewayError));
            });
        }
    }
}
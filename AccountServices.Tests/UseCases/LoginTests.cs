using NUnit.Framework;
using AccountServices.Tests.Gateways;
using AccountServices.UseCases;
using static AccountServices.UseCases.Login;
using AccountServices.UseCases.ValueTypes;
using AccountServices.UseCases.Models;

namespace AccountServices.Tests.UseCases
{
    public class LoginTests
    {
        private Login? _login;
        private Presenter? _presenter;
        private string? _password;


        public class Presenter : IPresenter
        {
            public bool NotFoundPresented { get; private set; }
            public bool AccessDenied { get; private set; }
            public bool Success { get; private set; }
            public EmailAddress? EmailAddress { get; private set; }

            public async Task PresentNotFound()
            {
                NotFoundPresented = true;
                await Task.CompletedTask;
            }

            public async Task PresentAccessDenied()
            {
                AccessDenied = true;
                await Task.CompletedTask;
            }


            public async Task PresentSuccess(Guid accountId, EmailAddress emailAddress)
            {
                Success = true;
                EmailAddress = emailAddress;
                await Task.CompletedTask;
            }
        }

        [SetUp]
        public void SetUp()
        {
            _presenter = new Presenter();
            _password = "Pa$$w0rd";
        }

        public class GivenSuccessGateway 
            : LoginTests
        {
            private AccountGateway? _accountGateway;

            [SetUp]
            public void SetUpGivenSuccessGatewayMode()
            {
                _accountGateway = new AccountGateway(GatewayMode.SuccessMode);
                _login = new Login(_accountGateway);
            }

            public class GivenEmailDoesNotExist 
                : GivenSuccessGateway
            {
                private string? _nonExistingEmail;
                
                [SetUp]
                public void SetUpGivenEmailDoesNotExist()
                {
                    _nonExistingEmail = "does.not.exist@domain.com";
                }
                
                [Test]
                public async Task PresentNotFound()
                {
                    
                    await _login!.Execute(_presenter!, new LoginModel(_nonExistingEmail!, _password!));
                    Assert.That(_presenter!.NotFoundPresented);
                }
            }

            public class GivenEmailExists 
                : GivenSuccessGateway
            {
                private string? _existingEmail;

                [SetUp]
                public async Task SetUpGivenEmailExists()
                {
                    _existingEmail = "exists@domain.com";
                    await new CreateAccount(_accountGateway!)
                        .Execute(
                            new CreateAccountTests.Presenter(),
                            new CreateAccountModel(_existingEmail, _password!, _password!));
                }

                public class GivenPasswordHashNotVerified
                    : GivenEmailExists
                {
                    [Test]
                    public async Task AccessDenied()
                    {
                        await _login!.Execute(_presenter!, new LoginModel(_existingEmail!, _password! + "X"));
                        Assert.That(_presenter!.AccessDenied);
                    }
                }

                public class GivenPasswordHashVerified
                    : GivenEmailExists
                {
                    [Test]
                    public async Task AccessDenied()
                    {
                        await _login!.Execute(_presenter!, new LoginModel(_existingEmail!, _password!));
                        Assert.Multiple(() =>
                        {
                            Assert.That(_presenter!.Success);
                            Assert.That(_presenter!.EmailAddress, Is.EqualTo((EmailAddress)_existingEmail!));
                        });
                    }
                }
            }
        }




    }
}

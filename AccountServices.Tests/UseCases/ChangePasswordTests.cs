using NUnit.Framework;

using AccountServices.UseCases;
using AccountServices.UseCases.Models;
using AccountServices.UseCases.ValueTypes;
using AccountServices.Tests.Gateways;
using AccountServices.Tests.UseCases.Models;
using static AccountServices.UseCases.ChangePassword;

namespace AccountServices.Tests.UseCases;
public class ChangePasswordTests
{
    private const string OldPassword = "Pa$$w0rd";
    private const string EmailAddress = "local-part@domain.com";
    private ChangePassword? _changePassword;
    private AccountGateway? _accountGateway;
    private Presenter? _presenter;

    private class Presenter : IPresenter
    {
        public bool AccountNotFoundPresented { get; private set; }
        public bool WrongPasswordPresented { get; private set; }
        public bool PasswordNotVerifiedPresented { get; private set; }
        public bool PasswordChangedPresented { get; private set; }
        

        public async Task PresentAccountNotFound()
        {
            AccountNotFoundPresented = true;
            await Task.CompletedTask;
        }

        public async Task PresentWrongPassword()
        {
            WrongPasswordPresented = true;
            await Task.CompletedTask;
        }

        public async Task PresentPasswordNotVerified()
        {
            PasswordNotVerifiedPresented = true;
            await Task.CompletedTask;
        }

        public async Task PasswordChanged()
        {
            PasswordChangedPresented = true;
            await Task.CompletedTask;
        }
    }
    

    [SetUp]
    public void SetUp()
    {
        _presenter = new Presenter();
        _accountGateway = new AccountGateway(GatewayMode.SuccessMode);
        _changePassword = new ChangePassword(_accountGateway);
    }

    public class GivenNonExistentAccount
            : ChangePasswordTests
    {
        [Test]
        public async Task PresentAccountNotFound()
        {
            await _changePassword!.Execute(_presenter!, new UserContext(Guid.NewGuid()), new ChangePasswordModel(OldPassword, OldPassword, OldPassword));
            Assert.That(_presenter!.AccountNotFoundPresented);
        }   
    }

    public class GivenExistingAccount
        : ChangePasswordTests
    {
        private Guid _accountId;
        private Password? _oldPassword;
        
        [SetUp]
        public async Task SetUpExistingAccount()
        {
            var createAccount = new CreateAccount(_accountGateway!);
            await createAccount.Execute(
                new CreateAccountTests.Presenter(), 
                new CreateAccountModel(EmailAddress, OldPassword, OldPassword));
        
            _accountId = (await _accountGateway!.Find(EmailAddress))!.Id;    
        }


        public class GivenOldPasswordIsWrong
            : GivenExistingAccount
        {
            private const string WrongPassword = "wr0ngPa$$w0rd";
            

            [SetUp]
            public void SetupWrongPassword()
            {
                _oldPassword = WrongPassword;
            }
            
            [Test]
            public async Task PresentWrongPassword()
            {
                await _changePassword!.Execute(_presenter!, new UserContext(_accountId), new ChangePasswordModel(_oldPassword!, _oldPassword!, _oldPassword!));
                Assert.That(_presenter!.WrongPasswordPresented);
            }
        }

        public class GivenOldPasswordIsCorrect
            : GivenExistingAccount
        {
            private string? _newPassword;
            private string? _verifyPassword;
            
            [SetUp]
            public void SetupCorrectPassword()
            {
                _oldPassword = OldPassword;
                _newPassword   = OldPassword;
                _verifyPassword= OldPassword;
            }
            
            public class GivenNewPasswordDoesNotMatchVerifyPassword 
                : GivenOldPasswordIsCorrect
            {
                [SetUp]
                public void NewPasswordDoesNotMatchVerifyPassword()
                {
                    _newPassword = "NewPa$$w0rd";
                    _verifyPassword = "VerifyPa$$w0rd";
                }
                
                
                [Test]
                public async Task PresentPasswordNotVerified()
                {
                    await _changePassword!.Execute(_presenter!, new UserContext(_accountId),new ChangePasswordModel(_oldPassword!, _newPassword!, _verifyPassword!));
                    Assert.That(_presenter!.PasswordNotVerifiedPresented);
                }
            }
            
            public class GivenNewPasswordMatchVerifyPassword 
                : GivenOldPasswordIsCorrect
            {
                [SetUp]
                public void NewPasswordDoesNotMatchVerifyPassword()
                {
                    _newPassword = "NewPa$$w0rd";
                    _verifyPassword = _newPassword;
                }
                
                [Test]
                public async Task PresentPasswordChanged()
                {
                    var login = new Login(_accountGateway!);
                    var loginPresenter = new LoginTests.Presenter();
                    await _changePassword!.Execute(_presenter!, new UserContext(_accountId), new ChangePasswordModel(_oldPassword!, _newPassword!, _verifyPassword!));
                    Assert.That(_presenter!.PasswordChangedPresented);

                    await login.Execute(loginPresenter, new LoginModel(EmailAddress, _newPassword!));
                    Assert.That(loginPresenter.Success);
                }
            }
        }
    }
}
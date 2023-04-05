using NUnit.Framework;
using AccountServices.UseCases.ValueTypes;
using static AccountServices.UseCases.ValueTypes.Password;

namespace AccountServicesTests.UseCases.ValueTypes;
public class PasswordTests
{
    [Test]
    public void InstanceAndEquality()
    {
        const string passwordTest = "Pass$0rd";
        const string notPasswordTest = "N0tPas$w0rd";
        
        var password = (Password)passwordTest;
        
        Assert.That(password, Is.TypeOf<Password>());
        Assert.That((string)password, Is.EqualTo(passwordTest));
        Assert.That(password, Is.Not.EqualTo((Password)notPasswordTest));
    }

    [Test]
    public void PasswordMustBeLongerThanMinimumPasswordLength()
    {
        Assert.Throws<InvalidPasswordException>(() =>
        {
            var _ = (Password) "short";
        });
    }
    
    [Test]
    public void PasswordMustBeShorterThanMaximumPasswordLength()
    {
        Assert.Throws<InvalidPasswordException>(() =>
        {
            var _ = (Password) "way-way-way-way-way-way-too-long";
        });
    }
        
    [Test]
    public void PasswordMustContainOneNumber()
    {
        Assert.Throws<InvalidPasswordException>(() =>
        {
            var _ = (Password) "password";
        });
    }
    
    [Test]
    public void PasswordMustContainOneLowerCaseLetter()
    {
        Assert.Throws<InvalidPasswordException>(() =>
        {
            var _ = (Password) "PASSW0RD";
        });
    }
                
    [Test]
    public void PasswordMustContainOneUpperCaseLetter()
    {
        Assert.Throws<InvalidPasswordException>(() =>
        {
            var _ = (Password) "passw0rd";
        });
    }
    
                    
    [Test]
    public void PasswordMustContainOneSpecialCharacter()
    {
        Assert.Throws<InvalidPasswordException>(() =>
        {
            var _ = (Password) "Passw0rd";
        });
    }
}
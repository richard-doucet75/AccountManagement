using AccountServices.UseCases.ValueTypes;
using NUnit.Framework;
using static AccountServices.UseCases.ValueTypes.EmailAddress;

namespace AccountServicesTests.UseCases.ValueTypes;

public class EmailAddressTests
{
    [Test]
    public void InstanceAndEqualityTests()
    {
        const string testEmailAddress = "EmailAddress@domain.com";
        const string notTestEmailAddress = "NotTheEmailAddress@domain.com";
        
        var emailAddress = (EmailAddress) testEmailAddress;
        
        Assert.Multiple(() =>
        {
            Assert.That(emailAddress, Is.TypeOf<EmailAddress>());
            Assert.That((string)emailAddress, Is.EqualTo(testEmailAddress));
            Assert.That(emailAddress, Is.EqualTo((EmailAddress)testEmailAddress));
            Assert.That(emailAddress, Is.Not.EqualTo((EmailAddress)notTestEmailAddress));
        });
    }

    [Test]
    public void ContainsOneAtSymbol()
    {
        Assert.Throws<InvalidEmailAddressException>(() =>
        {
            var _ = (EmailAddress) "notAValidEmailAddress";
        });
        
        Assert.Throws<InvalidEmailAddressException>(() =>
        {
            var _ = (EmailAddress) "notAValid@@EmailAddress";
        });
    }

    [Test]
    public void HasLocalPart()
    {
        Assert.Throws<InvalidEmailAddressException>(() =>
        {
            var _ = (EmailAddress) "@notAValidEmailAddress";
        });
    }
    
    [Test]
    public void HasDomainPart()
    {
        Assert.Throws<InvalidEmailAddressException>(() =>
        {
            var _ = (EmailAddress) "notAValidEmailAddress@";
        });
    }

    [Test]
    public void DomainHasArLeastOnDot()
    {
        Assert.Throws<InvalidEmailAddressException>(() =>
        {
            var _ = (EmailAddress) "notAValidEmailAddress@domain";
        });
    }
    
    [Test]
    public void AllSubDomainsHaveAtLeastOneCharacter()
    {
        Assert.Throws<InvalidEmailAddressException>(() =>
        {
            var _ = (EmailAddress) "notAValidEmailAddress@domain..com";
        });
    }
}
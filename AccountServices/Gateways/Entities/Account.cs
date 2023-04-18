using AccountServices.UseCases.ValueTypes;

namespace AccountServices.Gateways.Entities;
public class Account : ICloneable
{
    public Guid Id { get; }
    public EmailAddress EmailAddress { get; set; }
    public string PasswordHash { get; set; }

    public Account(Guid id, EmailAddress emailAddress, string passwordHash)
    {
        Id = id;
        EmailAddress = emailAddress;
        PasswordHash = passwordHash;
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
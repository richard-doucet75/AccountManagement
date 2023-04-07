namespace AccountServices.UseCases.ValueTypes;

public sealed class EmailAddress
{
    public const int MinimumLength = 3;
    public const int MaximumLenght = 320;
    private readonly string _value;

    private EmailAddress(string value)
    {
        _value = value;
    }

    private static EmailAddress OfValue(string value)
    {
        var parts = value.Split('@');
        if(parts.Length != 2) 
            throw new InvalidEmailAddressException();

        var localPart = parts[0];
        if (localPart.Length == 0)
            throw new InvalidEmailAddressException();

        var domainPart = parts[1];
        if(domainPart.Length == 0 || domainPart.All(c => c != '.'))
            throw new InvalidEmailAddressException();

        var subDomains = domainPart.Split('.');
        if(subDomains.Any(c=>c.Length == 0))
            throw new InvalidEmailAddressException();
        
        return new EmailAddress(value);
    }

    public static implicit operator string(EmailAddress emailAddress)
    {
        return emailAddress._value;
    }
    
    public static implicit operator EmailAddress(string value)
    {
        return OfValue(value);
    }
    
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is EmailAddress other && Equals(other);
    }
    
    private bool Equals(EmailAddress other)
    {
        return _value == other._value;
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public static bool operator ==(EmailAddress? left, EmailAddress? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(EmailAddress? left, EmailAddress? right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return _value;
    }

    public class InvalidEmailAddressException : Exception {}
}
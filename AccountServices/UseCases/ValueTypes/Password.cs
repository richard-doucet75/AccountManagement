using AccountServices.UseCases.Services;
using static System.Char;

namespace AccountServices.UseCases.ValueTypes;

public class Password
{
    public const int MinimumLength = 8;
    public const int MaximumLength = 256;
    private static readonly char[] SpecialCharacters = "!@#$%^&*".ToArray();
    
    private readonly string _value;

    private Password(string value)
    {
        _value = value;
    }

    public async Task<string> Hash()
    {
        return await PasswordHasher.Hash(this);
    }
    
    public async Task<bool> Verify(string hash)
    {
        return await PasswordHasher.Verify(this, hash);
    }

    public static implicit operator Password(string value)
    {
        if (value.Length is < MinimumLength 
            or > MaximumLength)
            throw new InvalidPasswordException();
        
        if(value.All(c => !IsNumber(c)))
            throw new InvalidPasswordException();
        
        if(value.All(c=>!IsLower(c)))
            throw new InvalidPasswordException();
        
        if(value.All(c=>!IsUpper(c)))
            throw new InvalidPasswordException();
        
        if(value.All(c=>!SpecialCharacter(c)))
            throw new InvalidPasswordException();
        
        return OfValue(value);
    }

    private static bool SpecialCharacter(char value)
    {
        return SpecialCharacters.Any(c => value == c);
    }

    private static Password OfValue(string value)
    {
        return new Password(value);
    }

    public static implicit operator string(Password password)
    {
        return password._value;
    }
    
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() 
               && Equals((Password) obj);
    }

    private bool Equals(Password other)
    {
        return _value == other._value;
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public static bool operator ==(Password? left, Password? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Password? left, Password? right)
    {
        return !Equals(left, right);
    }

    public class InvalidPasswordException : Exception
    {
        
    }
}
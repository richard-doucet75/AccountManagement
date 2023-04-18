using AccountServices.UseCases.ValueTypes;
using System.ComponentModel.DataAnnotations;

namespace AccountServices.UseCases.Models
{
    [Serializable]
    public record CreateAccountModel(
        [property:
        Required,
        MinLength(EmailAddress.MinimumLength),
        MaxLength(EmailAddress.MaximumLenght)]
        EmailAddress EmailAddress,
        [property:
        Required,
        MinLength(Password.MinimumLength),
        MaxLength(Password.MaximumLength)]
        Password Password,
        [property:
        Required,
        MinLength(Password.MinimumLength),
        MaxLength(Password.MaximumLength)]
        Password VerifyPassword
    );
}

using AccountServices.UseCases.ValueTypes;
using System.ComponentModel.DataAnnotations;

namespace AccountServices.UseCases.Models
{
    [Serializable]
    public record ChangePasswordModel(
        [property: Required]
        Password OldPassword,
        [property: Required]
        Password NewPassword,
        [property: Required]
        Password VerifyPassword
    );
}

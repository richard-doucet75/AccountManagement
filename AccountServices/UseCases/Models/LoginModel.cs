using AccountServices.UseCases.ValueTypes;
using System.ComponentModel.DataAnnotations;

namespace AccountServices.UseCases.Models
{
    [Serializable]
    public record LoginModel(
        [property: Required]
        EmailAddress EmailAddress,
        [property: Required]
        Password Password
    );
}

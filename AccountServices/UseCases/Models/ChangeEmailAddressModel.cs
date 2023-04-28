using AccountServices.UseCases.ValueTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountServices.UseCases.Models
{
    public record ChangeEmailAddressModel(
        [property: Required] 
        Guid AccountId,
        [property: Required] 
        EmailAddress EmailAddress);
}

using AccountServices.UseCases.Models;

namespace AccountServices.Tests.UseCases.Models
{
    public class UserContext : IUserContext
    {
        public Guid? AccountId { get; }

        public UserContext(Guid? accountId)
        {
            AccountId = accountId;
        }
    }
}

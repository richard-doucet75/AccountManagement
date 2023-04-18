using AccountServices.Gateways.Entities;
using AccountServices.UseCases.ValueTypes;

namespace AccountServices.Gateways;

public interface IAccountGateway
{
    Task Create(Account account);
    Task<bool> Exist(EmailAddress emailAddress);
    Task<Account?> Find(EmailAddress emailAddress);
    Task<Account?> Find(Guid accountId);
    Task Update(Account original, Account updated);
}
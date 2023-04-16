using AccountServices.UseCases.ValueTypes;

namespace AccountServices.Gateways.Entities;
public record Account(Guid Id, EmailAddress EmailAddress, string PasswordHash);

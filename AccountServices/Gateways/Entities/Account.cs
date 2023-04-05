using AccountServices.UseCases.ValueTypes;

namespace AccountServices.Gateways.Entities;

public record Account(EmailAddress EmailAddress, string PasswordHash);

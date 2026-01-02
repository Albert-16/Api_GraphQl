namespace Davivienda.GraphQL.Api.GraphQL.Inputs;

public sealed record CreateAccountInput(Guid CustomerId, string Number, decimal Balance, string Currency);

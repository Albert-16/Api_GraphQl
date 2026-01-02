namespace Davivienda.GraphQL.Api.Contracts;

public sealed record RegisterRequest(string Email, string FullName, string Password);

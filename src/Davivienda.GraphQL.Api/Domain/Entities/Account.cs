namespace Davivienda.GraphQL.Api.Domain.Entities;

public sealed class Account
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Number { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "COP";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public Customer Customer { get; set; } = null!;
}

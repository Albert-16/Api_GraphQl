namespace Davivienda.GraphQL.Api.Domain.Entities;

public sealed class Customer
{
    public Guid Id { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}

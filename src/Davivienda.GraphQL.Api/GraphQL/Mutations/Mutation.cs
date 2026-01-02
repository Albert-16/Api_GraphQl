using Davivienda.GraphQL.Api.Domain.Entities;
using Davivienda.GraphQL.Api.GraphQL.Inputs;
using Davivienda.GraphQL.Api.GraphQL.Payloads;
using Davivienda.GraphQL.Api.Infrastructure.Data;
using HotChocolate.Authorization;

namespace Davivienda.GraphQL.Api.GraphQL.Mutations;

public sealed class Mutation
{
    [Authorize]
    public async Task<CustomerPayload> CreateCustomerAsync(
        CreateCustomerInput input,
        [Service] AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            DocumentNumber = input.DocumentNumber,
            FullName = input.FullName,
            CreatedAt = DateTimeOffset.UtcNow
        };

        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CustomerPayload(customer);
    }

    [Authorize]
    public async Task<AccountPayload> CreateAccountAsync(
        CreateAccountInput input,
        [Service] AppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var account = new Account
        {
            Id = Guid.NewGuid(),
            CustomerId = input.CustomerId,
            Number = input.Number,
            Balance = input.Balance,
            Currency = input.Currency,
            CreatedAt = DateTimeOffset.UtcNow
        };

        dbContext.Accounts.Add(account);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AccountPayload(account);
    }
}

using Davivienda.GraphQL.Api.Domain.Entities;
using Davivienda.GraphQL.Api.Infrastructure.Data;
using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Davivienda.GraphQL.Api.GraphQL.Queries;

public sealed class Query
{
    [Authorize]
    [UsePaging(DefaultPageSize = 20, MaxPageSize = 100)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Customer> GetCustomers([Service] AppDbContext dbContext)
        => dbContext.Customers.AsNoTracking();

    [Authorize]
    [UsePaging(DefaultPageSize = 20, MaxPageSize = 100)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Account> GetAccounts([Service] AppDbContext dbContext)
        => dbContext.Accounts.AsNoTracking();
}

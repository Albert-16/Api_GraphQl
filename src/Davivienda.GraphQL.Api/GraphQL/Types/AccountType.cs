using Davivienda.GraphQL.Api.Domain.Entities;
using HotChocolate.Types;

namespace Davivienda.GraphQL.Api.GraphQL.Types;

public sealed class AccountType : ObjectType<Account>
{
    protected override void Configure(IObjectTypeDescriptor<Account> descriptor)
    {
        descriptor.Description("Represents a customer bank account.");

        descriptor.Field(account => account.Id)
            .Description("Unique identifier for the account.");

        descriptor.Field(account => account.CustomerId)
            .Description("Identifier of the owning customer.");

        descriptor.Field(account => account.Number)
            .Description("Account number used for transactions.");

        descriptor.Field(account => account.Balance)
            .Description("Current balance for the account.");

        descriptor.Field(account => account.Currency)
            .Description("Currency code for the account balance.");

        descriptor.Field(account => account.CreatedAt)
            .Description("UTC timestamp when the account was created.");
    }
}

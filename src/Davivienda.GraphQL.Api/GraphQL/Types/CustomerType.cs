using Davivienda.GraphQL.Api.Domain.Entities;
using HotChocolate.Types;

namespace Davivienda.GraphQL.Api.GraphQL.Types;

public sealed class CustomerType : ObjectType<Customer>
{
    protected override void Configure(IObjectTypeDescriptor<Customer> descriptor)
    {
        descriptor.Description("Represents a Davivienda customer.");

        descriptor.Field(customer => customer.Id)
            .Description("Unique identifier for the customer.");

        descriptor.Field(customer => customer.DocumentNumber)
            .Description("Government-issued document number.");

        descriptor.Field(customer => customer.FullName)
            .Description("Customer full name.");

        descriptor.Field(customer => customer.CreatedAt)
            .Description("UTC timestamp when the customer was created.");

        descriptor.Field(customer => customer.Accounts)
            .Description("Bank accounts owned by the customer.");
    }
}

using Microsoft.AspNetCore.Identity;

namespace Davivienda.GraphQL.Api.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}

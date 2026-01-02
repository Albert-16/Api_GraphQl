namespace Davivienda.GraphQL.Api.Infrastructure.Identity;

public interface ITokenService
{
    Task<string> CreateTokenAsync(ApplicationUser user);
}

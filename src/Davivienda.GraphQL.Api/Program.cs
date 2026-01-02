using System.Text;
using Davivienda.GraphQL.Api.Contracts;
using Davivienda.GraphQL.Api.GraphQL.Mutations;
using Davivienda.GraphQL.Api.GraphQL.Queries;
using Davivienda.GraphQL.Api.GraphQL.Types;
using Davivienda.GraphQL.Api.Infrastructure.Data;
using Davivienda.GraphQL.Api.Infrastructure.Identity;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    var provider = builder.Configuration.GetValue<string>("Database:Provider") ?? "SqlServer";
    var connectionString = builder.Configuration.GetConnectionString(provider)
        ?? throw new InvalidOperationException($"Missing connection string for provider '{provider}'.");

    if (provider.Equals("PostgreSql", StringComparison.OrdinalIgnoreCase))
    {
        options.UseNpgsql(connectionString);
        return;
    }

    options.UseSqlServer(connectionString);
});

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));

builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 10;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager();

builder.Services.AddScoped<ITokenService, JwtTokenService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSection = builder.Configuration.GetSection("Jwt");
        var signingKey = jwtSection["SigningKey"];

        if (string.IsNullOrWhiteSpace(signingKey))
        {
            throw new InvalidOperationException("Jwt:SigningKey is required.");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<CustomerType>()
    .AddType<AccountType>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .ModifyRequestOptions(options =>
    {
        options.IncludeExceptionDetails = builder.Environment.IsDevelopment();
    });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/auth/register", async (
    RegisterRequest request,
    UserManager<ApplicationUser> userManager) =>
{
    var user = new ApplicationUser
    {
        UserName = request.Email,
        Email = request.Email,
        FullName = request.FullName
    };

    var result = await userManager.CreateAsync(user, request.Password);

    if (!result.Succeeded)
    {
        var errors = result.Errors.Select(error => error.Description);
        return Results.BadRequest(new { Errors = errors });
    }

    return Results.Created($"/users/{user.Id}", new { user.Id, user.Email });
});

app.MapPost("/auth/login", async (
    LoginRequest request,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ITokenService tokenService) =>
{
    var user = await userManager.FindByEmailAsync(request.Email);

    if (user is null)
    {
        return Results.Unauthorized();
    }

    var passwordValid = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

    if (!passwordValid.Succeeded)
    {
        return Results.Unauthorized();
    }

    var token = await tokenService.CreateTokenAsync(user);
    return Results.Ok(new { AccessToken = token });
});

app.MapGraphQL();

app.Run();

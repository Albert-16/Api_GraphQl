using Davivienda.GraphQL.Api.Domain.Entities;
using Davivienda.GraphQL.Api.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Davivienda.GraphQL.Api.Infrastructure.Data;

public sealed class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Customer>(entity =>
        {
            entity.HasKey(customer => customer.Id);
            entity.Property(customer => customer.DocumentNumber)
                .HasMaxLength(50)
                .IsRequired();
            entity.Property(customer => customer.FullName)
                .HasMaxLength(200)
                .IsRequired();
            entity.HasMany(customer => customer.Accounts)
                .WithOne(account => account.Customer)
                .HasForeignKey(account => account.CustomerId);
        });

        builder.Entity<Account>(entity =>
        {
            entity.HasKey(account => account.Id);
            entity.Property(account => account.Number)
                .HasMaxLength(20)
                .IsRequired();
            entity.Property(account => account.Currency)
                .HasMaxLength(3)
                .IsRequired();
            entity.Property(account => account.Balance)
                .HasPrecision(18, 2);
        });
    }
}

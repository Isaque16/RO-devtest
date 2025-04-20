namespace RO.DevTest.Persistence;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

/// <summary>
/// Represents the database context used for the application, inheriting from <see cref="IdentityDbContext{TUser}"/> to support ASP.NET Core Identity functionality.
/// Provides access to the application's entities and configurations.
/// </summary>
public class DefaultContext : IdentityDbContext<User> 
{
    public DefaultContext() { }

    public DefaultContext(DbContextOptions<DefaultContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Sale> Sales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString = "Host=localhost;Port=5432;Database=devtest;Username=postgres;Password=root";
        if (!optionsBuilder.IsConfigured) 
            optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder builder) 
    {
        builder.HasPostgresExtension("uuid-ossp");
        builder.ApplyConfigurationsFromAssembly(typeof(DefaultContext).Assembly);

        base.OnModelCreating(builder);
    }
}

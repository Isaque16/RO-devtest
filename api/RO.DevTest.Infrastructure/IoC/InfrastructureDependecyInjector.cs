namespace RO.DevTest.Infrastructure.IoC;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RO.DevTest.Application.Contracts.Infrastructure;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Infrastructure.Abstractions;
using RO.DevTest.Persistence;
using RO.DevTest.Persistence.Repositories;

/// <summary>
/// Provides methods to configure and inject the dependencies required
/// by the Infrastructure layer into an <see cref="IServiceCollection"/>.
/// </summary>
public static class InfrastructureDependencyInjector {
    /// <summary>
    /// Configures and injects the dependencies required by the Infrastructure layer into the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the dependencies will be added.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance with the Infrastructure dependencies injected.</returns>
    public static IServiceCollection InjectInfrastructureDependencies(
        this IServiceCollection services)
    {
        services.AddDefaultIdentity();
        services.RepositoryConfig();
        services.CorsConfig();
    
        return services;
    }

    /// <summary>
    /// Configures and adds CORS settings to the provided <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the CORS settings will be added.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance with CORS configured.</returns>
    private static IServiceCollection CorsConfig(
        this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policy =>
                {
                    policy.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });
        
        return services;
    }

    /// <summary>
    /// Configures and injects the repository-related dependencies into the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to which the repository dependencies will be added.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance with the repository dependencies injected.</returns>
    private static IServiceCollection RepositoryConfig(
        this IServiceCollection services)
    {
        services.AddScoped<IIdentityAbstractor, IdentityAbstractor>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        
        return services;
    }

    /// <summary>
    /// Adds the default ASP.NET Core Identity configuration to the specified <see cref="IServiceCollection"/>.
    /// This includes setting up the Identity user and role types, integrating with the Entity Framework context,
    /// and enabling the default token providers.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to which the default identity configuration will be added.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance with the default identity configuration added.</returns>
    private static IServiceCollection AddDefaultIdentity(
        this IServiceCollection services)
    {
        services.AddDefaultIdentity<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DefaultContext>()
            .AddDefaultTokenProviders();
        
        return services;
    }
}

using DbLevel.Interfaces;
using DbLevel;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

public static class ScopedServiceExtentions
{
    public static IServiceCollection AddScopedService(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        var repositoryAssembly = typeof(IEntity).Assembly;
        var repositoryTypes = repositoryAssembly.GetTypes()
            .Where(t => typeof(IEntity).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
            .ToList();

        foreach (var repoType in repositoryTypes)
        {
            var interfaceTypes = repoType.GetInterfaces()
                .Where(i => i != typeof(IEntity) && typeof(IEntity).IsAssignableFrom(i))
                .ToList();

            if (interfaceTypes.Any())
            {
                foreach (var interfaceType in interfaceTypes)
                {
                    services.AddScoped(interfaceType, repoType);
                }
            }
        }

        return services;
    }
}

using DbLevel.Interfaces;
using DbLevel;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Authorization.Interfaces;
using Authorization.JWT;
using BussinesLogic.Interfaces;
using BussinesLogic.Services;

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

        services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IHallService, HallService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IEquipmentService, EquipmentService>();

        return services;
    }
}

using DbLevel;
using Microsoft.Extensions.DependencyInjection;
using Authorization.Interfaces;
using Authorization.JWT;
using BussinesLogic.Interfaces;
using BussinesLogic.Services;
using BussinesLogic.Exporters;
using ABPTestTask.Common.ExporterInterfaces;
using ABPTestTask.Common.User;
using ABPTestTask.Common.Booking;
using ABPTestTask.Common.Hall;
using ABPTestTask.Common.Equipment;
using ABPTestTask.Common.Interfaces;

public static class ScopedServiceExtentions
{
    public static IServiceCollection AddScopedService(this IServiceCollection services)
    {
        // Register the generic repository interface and implementation
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Get the assembly that contains the IEntity interface
        var repositoryAssembly = typeof(IEntity).Assembly;

        // Find all concrete classes that implement IEntity
        var repositoryTypes = repositoryAssembly.GetTypes()
            .Where(t => typeof(IEntity).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
            .ToList();

        // Register each repository with its corresponding interface
        foreach (var repoType in repositoryTypes)
        {
            // Get all interfaces implemented by the repository, excluding IEntity itself
            var interfaceTypes = repoType.GetInterfaces()
                .Where(i => i != typeof(IEntity) && typeof(IEntity).IsAssignableFrom(i))
                .ToList();

            // Register the repository with each interface it implements
            if (interfaceTypes.Any())
            {
                foreach (var interfaceType in interfaceTypes)
                {
                    services.AddScoped(interfaceType, repoType);
                }
            }
        }

        // Register specific application services
        services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IHallService, HallService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IReportExporterFactory, ReportExporterFactory>();

        // Return the IServiceCollection for further chaining
        return services;
    }
}

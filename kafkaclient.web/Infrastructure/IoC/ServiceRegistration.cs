using kafkaclient.web.Core.Interfaces;
using kafkaclient.web.Core.Services;
using kafkaclient.web.Infrastructure.Data;
using kafkaclient.web.Infrastructure.Respository;
using Microsoft.EntityFrameworkCore;

namespace kafkaclient.web.Infrastructure.IoC;

public static class ServiceRegistration
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlLiteConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString)
        );

        services.AddScoped<IClusterRepository, ClusterRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient(typeof(ClusterService), typeof(ClusterService));
        services.AddTransient(typeof(ClusterClientService), typeof(ClusterClientService));
    }
}
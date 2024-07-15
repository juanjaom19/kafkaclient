using kafkaclient.web.Core.Entities;

namespace kafkaclient.web.Infrastructure.Data.Config;

public static class DBInitializer
{   
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var scopedService = scope.ServiceProvider;
            try
            {
                var context = scopedService.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                if(!context.Clusters.Any())
                {
                    var clusters = new Cluster[]
                    {
                        new Cluster(){
                            Name = "Type 1"
                        }
                    };
                    await context.Clusters.AddRangeAsync(clusters);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Manejar errores aquí
                var logger = scopedService.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}
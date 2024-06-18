using Microsoft.EntityFrameworkCore;

namespace ElectricityApp.EfStructures;

public static class DbExtensions
{
    public static IServiceCollection AddElectricityDbContext(this IServiceCollection services, string databaseName)
    {
        var connectionString =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), databaseName);

        services.AddDbContext<ElectricityDbContext>(options =>
            options.UseSqlite($"Data Source={connectionString}"));
        return services;
    }
}
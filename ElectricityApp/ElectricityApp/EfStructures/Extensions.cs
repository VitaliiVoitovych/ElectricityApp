using Microsoft.EntityFrameworkCore;

namespace ElectricityApp.EfStructures;

public static class Extensions
{
    public static IServiceCollection AddElectricityDbContext(this IServiceCollection services)
    {
        var connectionString =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "electricity.db");

        services.AddDbContext<ElectricityDbContext>(options =>
            options.UseSqlite($"Data Source={connectionString}"));
        return services;
    }
}
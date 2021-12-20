using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SummitDiary.Core.Common.Config;
using SummitDiary.Core.Common.Interfaces;
using SummitDiary.Infrastructure.Data;

namespace SummitDiary.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services,
            DatabaseConfiguration configuration)
        {
            if (configuration.UseSqlite)
                services.AddDbContext<IApplicationDbContext, AppDbContext>(options =>
                    options.UseSqlite(configuration.ConnectionString));
            else if (configuration.UsePostgres)
                services.AddDbContext<IApplicationDbContext, AppDbContext>(options =>
                    options.UseNpgsql(configuration.ConnectionString));
            else if (configuration.UseMySql)
                services.AddDbContext<IApplicationDbContext, AppDbContext>(options =>
                    options.UseMySql(configuration.ConnectionString,
                        ServerVersion.AutoDetect(configuration.ConnectionString)));
            else
                throw new InvalidOperationException("Please specify at least one database type");

            return services;
        }
    }
}
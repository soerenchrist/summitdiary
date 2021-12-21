using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SummitDiary.Core.Services;
using SummitDiary.Core.Services.Interfaces;

namespace SummitDiary.Core;
public static class DependencyInjection
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddTransient<IElevationService, ElevationService>();
        services.AddTransient<IGpxAnalyzer, GpxAnalyzer>();
        
        return services;
    }
}

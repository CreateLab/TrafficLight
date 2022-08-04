using Microsoft.Extensions.DependencyInjection;
using TrafficLight.Core;

namespace TrafficLight.Extension;

public static class DIExtension
{
    public static IServiceCollection AddTrafficLight(this IServiceCollection services)
    {
        services.AddSingleton<ITrafficLight, Core.TrafficLight>();
        return services;
    }
}
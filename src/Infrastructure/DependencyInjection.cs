using Domain.Interfaces;
using Infrastructure.ExternalServices;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddHttpClient<IPokeApiService, PokeApiService>();
        services.AddMemoryCache();

        return services;
    }
}

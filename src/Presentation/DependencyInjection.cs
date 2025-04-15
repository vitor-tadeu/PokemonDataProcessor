using Microsoft.Extensions.DependencyInjection;
using Presentation.View;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddScoped<PokemonConsole>();

        return services;
    }
}

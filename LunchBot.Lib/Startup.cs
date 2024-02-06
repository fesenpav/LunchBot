using LunchBot.Lib.Services;
using LunchBot.Lib.Services.Scrapers;
using Microsoft.Extensions.DependencyInjection;

namespace LunchBot.Lib;

/// <summary>
/// Start-up class handles application start-up routines.
/// At the moment, mainly for registering services from the library.
/// </summary>
public static class Startup
{
    /// <summary>
    /// Configures services from the library.
    /// </summary>
    /// <param name="collection">Service collection to register services to</param>
    /// <returns>Provided service collection with injected library services</returns>
    public static IServiceCollection ConfigureServices(IServiceCollection collection)
    {
        collection
            .AddSingleton<WebService>()
            .AddScoped<IWebScraperService, MenickaScraperService>();

        return collection;
    }
}
using LunchBot.Lib;
using LunchBot.Lib.Models.Configuration;
using LunchBot.Lib.Services;
using LunchBot.Lib.Services.Scrapers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LunchBot.Bot;

public abstract class Program
{
    public static void Main(string[] args)
    {
        // Build configuration
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        var configuration = configurationBuilder.Build();
        
        // Build service collection
        var serviceCollection = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .Configure<BotConfiguration>(configuration.GetSection(nameof(BotConfiguration)));
        
        var provider = BuildServiceProvider(serviceCollection);
        /*
        var scraperService = provider.GetRequiredService<IWebScraperService>();
        var kafarna = scraperService.ScrapeMenu("Kafárna na kus řeči", "https://www.menicka.cz/2044-kafarna-na-kus-reci.html").Result;
        var redHook = scraperService.ScrapeMenu("Red hook", "https://www.menicka.cz/4486-red-hook.html").Result;
        */
        var botService = provider.GetRequiredService<BotService>();
        botService.Start().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Builds dependency injection service provider.
    /// </summary>
    /// <returns>Service provider with system services</returns>
    private static ServiceProvider BuildServiceProvider(IServiceCollection serviceCollection)
    {
        serviceCollection = Startup.ConfigureServices(serviceCollection);
        return serviceCollection.BuildServiceProvider();
    }
}
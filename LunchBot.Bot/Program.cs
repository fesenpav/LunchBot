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
        
        // Send embed
        var provider = BuildServiceProvider(serviceCollection);
        var botService = provider.GetRequiredService<WebhookService>();
        botService.SendEmbed();
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
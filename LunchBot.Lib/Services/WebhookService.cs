using System.Diagnostics;
using Discord;
using Discord.Webhook;
using LunchBot.Lib.Models.Configuration;
using LunchBot.Lib.Services.Scrapers;
using Microsoft.Extensions.Options;

namespace LunchBot.Lib.Services;

public class WebhookService
{
    private readonly DiscordWebhookClient _client;
    private readonly IWebScraperService _webScraperService;

    public WebhookService(IOptions<BotConfiguration> botConfiguration, IWebScraperService webScraperService)
    {
        var configuration = botConfiguration.Value;
        _client = new DiscordWebhookClient(configuration.WebhookUrl);
        _webScraperService = webScraperService;
    }
    
    public async void SendEmbed()
    {
        var embedBuilder = new EmbedBuilder()
            .WithTitle("\ud83c\udf54 Blíží se doba oběda!")
            .WithDescription("Žij proto, abys jedl. Nejez proto, abys žil.")
            .WithColor(Color.Orange);

        var stopwatch = Stopwatch.StartNew();
        var kafarna = _webScraperService.ScrapeMenu("Kafárna na kus řeči", "https://www.menicka.cz/2044-kafarna-na-kus-reci.html").Result;
        var redHook = _webScraperService.ScrapeMenu("Red hook", "https://www.menicka.cz/4486-red-hook.html").Result;
        stopwatch.Stop();
            
        embedBuilder.Footer = new EmbedFooterBuilder()
            .WithText($"Vygeneroval Lunch Manažer ({stopwatch.Elapsed.TotalSeconds}s)")
            .WithIconUrl("https://cdn.discordapp.com/avatars/1204565926360649819/4e332285934da75540f26d7506f22518.webp?size=32");
            
        embedBuilder = kafarna.AddToEmbed(embedBuilder);
        embedBuilder = redHook.AddToEmbed(embedBuilder);
        
        await _client.SendMessageAsync(embeds: new[] { embedBuilder.Build() });
    }
}
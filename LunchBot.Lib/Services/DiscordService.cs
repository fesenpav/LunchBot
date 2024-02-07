using System.Diagnostics;
using Discord;
using Discord.WebSocket;
using LunchBot.Lib.Models.Configuration;
using LunchBot.Lib.Services.Scrapers;
using Microsoft.Extensions.Options;

namespace LunchBot.Lib.Services;

/// <summary>
/// Service implementing a communication with Discord Bot API.
/// Contains methods to login, set activity and send messages.
/// </summary>
public class DiscordService(IOptions<BotConfiguration> configuration, IWebScraperService webScraperService)
{
    private readonly DiscordSocketClient _client = new();
    private readonly BotConfiguration _configuration = configuration.Value;

    /// <summary>
    /// Handles the client ready event.
    /// </summary>
    /// <param name="readyFunc">Function that gets run when the client is ready</param>
    public void SetClientReadyHandler(Func<Task> readyFunc) {
        _client.Ready += readyFunc;
        _client.Ready += () =>
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle("\ud83c\udf54 Blíží se doba oběda!")
                .WithDescription("Žij proto, abys jedl. Nejez proto, abys žil.")
                .WithColor(Color.Orange);

            var stopwatch = Stopwatch.StartNew();
            var kafarna = webScraperService.ScrapeMenu("Kafárna na kus řeči", "https://www.menicka.cz/2044-kafarna-na-kus-reci.html").Result;
            var redHook = webScraperService.ScrapeMenu("Red hook", "https://www.menicka.cz/4486-red-hook.html").Result;
            stopwatch.Stop();
            
            embedBuilder.Footer = new EmbedFooterBuilder()
                .WithText($"Vygeneroval Lunch Manažer ({stopwatch.Elapsed.TotalSeconds}s)")
                .WithIconUrl("https://cdn.discordapp.com/avatars/1204565926360649819/4e332285934da75540f26d7506f22518.webp?size=32");
            
            embedBuilder = kafarna.AddToEmbed(embedBuilder);
            embedBuilder = redHook.AddToEmbed(embedBuilder);
            
            SendEmbed(_configuration.ChannelId, embedBuilder.Build()).GetAwaiter().GetResult();
            return Task.CompletedTask;
        };
    }
    
    /// <summary>
    /// Sets the client's Discord presence.
    /// </summary>
    /// <param name="activity">Activity to set</param>
    public async Task SetClientActivity(IActivity activity)
    {
        await _client.SetActivityAsync(activity);
    }
    
    /// <summary>
    /// Logs the given message to the console.
    /// </summary>
    /// <param name="message">Message to log</param>
    private Task Log(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }

    
    /// <summary>
    /// Registers the bot's event handlers and starts the bot.
    /// </summary>
    public async Task Start()
    {
        _client.Log += Log;
        
        await _client.LoginAsync(TokenType.Bot, _configuration.Token);
        await _client.StartAsync();
        
        await Task.Delay(-1);
    }
    
    /// <summary>
    /// Sends the given message to the given channel.
    /// </summary>
    /// <param name="channelId">Channel Id to send the message to</param>
    /// <param name="embed">Embed to send</param>
    public async Task SendEmbed(ulong channelId, Embed embed) {
        var channel = _client.GetChannel(channelId) as IMessageChannel;
        await channel?.SendMessageAsync(embed: embed)!;
    }
}
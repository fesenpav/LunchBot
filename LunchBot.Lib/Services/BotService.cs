using Discord;

namespace LunchBot.Lib.Services;

public class BotService(DiscordService discordService)
{
    public async Task Start()
    {
        // Register the bot's event handlers.
        discordService.SetClientReadyHandler(async () => {
            // Set the bot's activity.
            await discordService.SetClientActivity(new Game("Kitchen Nightmares"));
        });
        
        // Start the bot
        await discordService.Start();
    }
}
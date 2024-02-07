using Newtonsoft.Json;

namespace LunchBot.Lib.Models.Configuration;

public class BotConfiguration {

    [JsonProperty(nameof(Token))]
    public string Token { get; set; } = string.Empty;

    [JsonProperty(nameof(GuildId))]
    public ulong GuildId { get; set; } = 0;
    
    [JsonProperty(nameof(ChannelId))]
    public ulong ChannelId { get; set; } = 0;

    public BotConfiguration() {
        // Empty constructor
    }
}
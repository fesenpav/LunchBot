using Newtonsoft.Json;

namespace LunchBot.Lib.Models.Configuration;

public class BotConfiguration {
    
    [JsonProperty(nameof(WebhookUrl))]
    public string WebhookUrl { get; set; }

    public BotConfiguration() {
        // Empty constructor
    }
}
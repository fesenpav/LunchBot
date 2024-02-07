using System.Text;

namespace LunchBot.Lib.Services;

/// <summary>
/// Service for web interaction.
/// </summary>
public class WebService : IDisposable
{
    private readonly HttpClient _client = new();

    public WebService()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }
    
    /// <summary>
    /// Downloads the provided URL as a string.
    /// </summary>
    /// <param name="url">URL to download</param>
    /// <returns>Downloaded URL webpage as string</returns>
    public async Task<string> Get(string url)
    {
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        // Because of encoding issues, we need to read the content as bytes and then convert to a string.
        var html = response.Content.ReadAsStringAsync().Result;

        return html;
    }
    
    /// <summary>
    /// Properly disposes of the WebService instance.
    /// </summary>
    public void Dispose()
    {
        _client.Dispose();
    }
}
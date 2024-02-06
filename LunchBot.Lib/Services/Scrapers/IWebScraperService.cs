using LunchBot.Lib.Models;

namespace LunchBot.Lib.Services.Scrapers;

/// <summary>
/// Interface for implementation of various web scraping services.
/// </summary>
public interface IWebScraperService
{
    /// <summary>
    /// Scrapes a restaurant menu from a given URL.
    /// </summary>
    /// <param name="restaurantName">Name of the restaurant</param>
    /// <param name="url">URL to scrape restaurant menu from</param>
    /// <returns>Scraped Restaurant menu as object</returns>
    public Task<RestaurantMenu> ScrapeMenu(string restaurantName, string url);
}
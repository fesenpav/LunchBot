using HtmlAgilityPack;
using LunchBot.Lib.Exceptions;
using LunchBot.Lib.Models;

namespace LunchBot.Lib.Services.Scrapers;

/// <summary>
/// Menu service for scraping menus from Menicka.cz.
/// </summary>
public class MenickaScraperService(WebService webService) : IWebScraperService
{
    /// <summary>
    /// Scrapes Menicka.cz menu from a given URL.
    /// </summary>
    /// <param name="url">URL to scrape restaurant menu from</param>
    /// <returns>Scraped Restaurant menu as object</returns>
    public Task<RestaurantMenu> ScrapeMenu(string restaurantName, string url)
    {
        // Load page HTML content
        var html = webService.Get(url).Result;
        
        // Parse HTML content with menus
        var document = new HtmlDocument();
        document.LoadHtml(html);
        var menuNodes = document.DocumentNode.SelectNodes("//div[contains(@class, 'menicka')]");
        
        // If menu is empty, throw an exception
        if (menuNodes.Count == 0)
        {
            throw new NoRestaurantMenuException("No menu found on the provided URL.", url);
        }

        // Get today's menu
        var todayNode = menuNodes[0];
        
        // Parse soup
        var soup = ParseFoodOption(todayNode.SelectSingleNode(".//li[contains(@class, 'polevka')]"));
        
        // Parse main courses
        var mainCourseNodes = todayNode.SelectNodes(".//li[contains(@class, 'jidlo')]");
        if(mainCourseNodes.Count == 0)
        {
            throw new NoRestaurantMenuException("Main course list is empty for provided URL.", url);
        }
        var mainCourses = mainCourseNodes.Select(ParseFoodOption).ToList();

        // Finally return
        return Task.FromResult(new RestaurantMenu(restaurantName, soup, mainCourses));
    }
    
    /// <summary>
    /// Parses a food option from the provided HTML path.
    /// </summary>
    /// <param name="node">Node to parse food option from</param>
    /// <returns>Parsed Food option</returns>
    private static FoodOption ParseFoodOption(HtmlNode node)
    {
        Console.WriteLine(node.InnerHtml);
        
        var name = node.SelectSingleNode(".//div[contains(@class, 'polozka')]").InnerText;
        var price = node.SelectSingleNode(".//div[contains(@class, 'cena')]").InnerText;
        
        return new FoodOption(name, price);
    }
}
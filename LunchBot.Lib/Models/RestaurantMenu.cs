using Discord;

namespace LunchBot.Lib.Models;

public class RestaurantMenu(string name, FoodOption soup, List<FoodOption> mainCourses)
{
    public string Name { get; set; } = name;
    public FoodOption Soup { get; set; } = soup;
    public List<FoodOption> MainCourses { get; set; } = mainCourses;
    
    public EmbedBuilder AddToEmbed(EmbedBuilder embedBuilder)
    {
        embedBuilder.AddField(Name, $"\ud83e\udd63 `{Soup.Name}`\n\ud83d\udd39" + string.Join("\n\ud83d\udd39", MainCourses.Select(x => $"`{FixName(x.Name)}`")));
        return embedBuilder;
    }

    /// <summary>
    /// TODO: Remove and fix other way
    /// </summary>
    private string FixName(string foodName)
    {
        return
            foodName
                .Replace("Menu 1, ", "")
                .Replace("Menu 2, ", "")
                .Replace("Menu 3, ", "")
                .Replace("Menu 4, ", "")
                .Replace("Menu 5, ", "");
    }
}
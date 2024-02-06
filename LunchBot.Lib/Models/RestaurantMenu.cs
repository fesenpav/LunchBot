namespace LunchBot.Lib.Models;

public class RestaurantMenu(string name, FoodOption soup, List<FoodOption> mainCourses)
{
    public string Name { get; set; } = name;
    public FoodOption Soup { get; set; } = soup;
    public List<FoodOption> MainCourses { get; set; } = mainCourses;
}
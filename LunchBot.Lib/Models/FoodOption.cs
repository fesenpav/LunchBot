namespace LunchBot.Lib.Models;

public class FoodOption(string name, string price)
{
    public string Name { get; set; } = name;
    public string Price { get; set; } = price;
}
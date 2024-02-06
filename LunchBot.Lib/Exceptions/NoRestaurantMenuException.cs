namespace LunchBot.Lib.Exceptions;

/// <summary>
/// Simple implementation of an exception for when a restaurant menu is not found.
/// </summary>
public class NoRestaurantMenuException(string message, string url) : Exception(message);
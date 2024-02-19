namespace SantanderTest.Models;

public sealed record Story(string By, List<int> Kids, int Score, long Time, string Title, Uri Url);

namespace SantanderTest.Models;

public record StoryResponse(string Title, Uri Uri, string PostedBy, DateTime Time, int Score, int CommentCount);

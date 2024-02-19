using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using SantanderTest.Controllers;
using SantanderTest.Models;
using SantanderTest.Services;

namespace SantanderTest.UnitTests;

public sealed class StoriesControllerTests
{
    [Fact]
    public async Task GetStoriesAsync_StoryRequested_ReturnsStoryResponse()
    {
        // Arrange
        var hackerNewsService = Substitute.For<IHackerNewsService>();
        var utcNow = DateTime.UtcNow;
        hackerNewsService.GetBestStoriesAsync(1).Returns(new[] { new StoryResponse("Test Story", new Uri("https://teststory.com"), "petersanderson", utcNow, 100, 0) });

        var storiesController = new StoriesController(hackerNewsService);

        // Act
        var result = await storiesController.GetStoriesAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);

        var okObjectResult = (OkObjectResult)result;
        var storyResponses = okObjectResult.Value as IEnumerable<StoryResponse>;
        Assert.NotNull(storyResponses);
        Assert.Single(storyResponses);

        var storyResponse = storyResponses.FirstOrDefault();
        Assert.NotNull(storyResponse);
        Assert.Equal("Test Story", storyResponse.Title);
        Assert.Equal(new Uri("https://teststory.com"), storyResponse.Uri);
        Assert.Equal("petersanderson", storyResponse.PostedBy);
        Assert.Equal(utcNow, storyResponse.Time);
        Assert.Equal(100, storyResponse.Score);
        Assert.Equal(0, storyResponse.CommentCount);
    }

    [Fact]
    public async Task GetStoriesAsync_ZeroStoriesRequested_ReturnsBadRequest()
    {
        // Arrange
        var storiesController = new StoriesController(Substitute.For<IHackerNewsService>());

        // Act
        var result = await storiesController.GetStoriesAsync(0);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result);
    }
}

using Microsoft.AspNetCore.Mvc.Testing;
using SantanderTest.Models;
using System.Net;

namespace SantanderTest.IntegrationTests;

public sealed class StoriesControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetStoriesAsync_StoriesRequested_ReturnsStoryResponses(int number)
    {
        // Arrange
        var httpClient = factory.CreateClient();

        // Act
        using var response = await httpClient.GetAsync($"/Stories?number={number}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var storyResponses = await response.Content.ReadFromJsonAsync<IEnumerable<StoryResponse>>();
        Assert.Equal(number, storyResponses!.Count());
    }

    [Fact]
    public async Task GetStoriesAsync_ZeroStoriesRequested_ReturnsBadRequest()
    {
        // Arrange
        var httpClient = factory.CreateClient();

        // Act
        using var response = await httpClient.GetAsync("/Stories?number=0");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

using Microsoft.Extensions.Caching.Memory;
using SantanderTest.Extensions;
using SantanderTest.Models;

namespace SantanderTest.Services;

public sealed class HackerNewsService(HttpClient httpClient, IMemoryCache cache, ILogger<HackerNewsService> logger) : IHackerNewsService, IDisposable
{
    public void Dispose() => httpClient.Dispose();

    public async Task<IEnumerable<StoryResponse?>?> GetBestStoriesAsync(int top, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Retrieving best stories");
        List<int>? ids;

        try
        {
            ids = await httpClient.GetFromJsonAsync<List<int>>("/v0/beststories.json", cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Could not retrieve best stories");
            return null;
        }

        if (ids is null)
        {
            return Enumerable.Empty<StoryResponse>();
        }

        if (top > ids!.Count)
        {
            top = ids.Count;
        }

        var stories = new List<Task<StoryResponse?>>(top);
        logger.LogInformation("Taking top {Top} stories", top);

        foreach (var id in ids.Take(top))
        {
            stories.Add(Task.Run(() => GetStoryAsync(id, cancellationToken)));
        }

        return await Task.WhenAll(stories);
    }

    private async Task<StoryResponse?> GetStoryAsync(int id, CancellationToken cancellationToken)
    {
        if (cache.TryGetValue(id, out StoryResponse? storyResponse))
        {
            logger.LogInformation("{ID} found in cache", id);
        }
        else
        {
            logger.LogInformation("{ID} not found in cache; retrieving from API", id);
            Story? story;

            try
            {
                story = await httpClient.GetFromJsonAsync<Story>($"/v0/item/{id}.json", cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Could not retrieve {ID} from API", id);
                return null;
            }

            if (story is not null)
            {
                logger.LogInformation("Adding {ID} to cache", id);
                storyResponse = new StoryResponse(story.Title, story.Url, story.By, story.Time.ToDateTime(), story.Score, story.Kids.Count);
                cache.Set(id, storyResponse, new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(10) });
            }
        }

        return storyResponse;
    }
}

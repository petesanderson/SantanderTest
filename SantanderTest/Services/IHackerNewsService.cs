using SantanderTest.Models;

namespace SantanderTest.Services;

public interface IHackerNewsService
{
    Task<IEnumerable<StoryResponse?>?> GetBestStoriesAsync(int top, CancellationToken cancellationToken = default);
}

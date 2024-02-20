using Microsoft.AspNetCore.Mvc;
using SantanderTest.Services;

namespace SantanderTest.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class StoriesController(IHackerNewsService hackerNewsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStoriesAsync([FromQuery] int number, CancellationToken cancellationToken)
    {
        if (number < 1)
        {
            return BadRequest();
        }

        var stories = await hackerNewsService.GetBestStoriesAsync(number, cancellationToken);

        if (stories is null)
        {
            return StatusCode(StatusCodes.Status424FailedDependency);
        }

        return Ok(stories.Where(s => s is not null).OrderByDescending(s => s!.Score));
    }
}

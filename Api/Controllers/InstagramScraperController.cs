using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InstagramScraperController : ControllerBase
{
    #region Initialization

    private readonly IInstagramScraperService _instagramScraperService;

    public InstagramScraperController(IInstagramScraperService instagramScraperService)
    {
        _instagramScraperService = instagramScraperService;
    }

    #endregion

    #region Apis

    [HttpPost(nameof(ScrapePost))]
    public async Task<IActionResult> ScrapePost([FromBody] string postUrlOrPostCode)
    {
        var instagramPostDto = await _instagramScraperService.ScrapePost(postUrlOrPostCode);

        return Ok(instagramPostDto);
    }

    [HttpPost(nameof(ScrapePosts))]
    public async Task<IActionResult> ScrapePosts([FromBody] string username, byte pageNumber, byte pageSize = 12)
    {
        var instagramPostsDto = await _instagramScraperService.ScrapePosts(username, pageNumber, pageSize);

        return Ok(instagramPostsDto);
    }

    #endregion
}

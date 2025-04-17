using static Infrastructure.Dtos.InstagramDto;

namespace Api.Services;

public interface IInstagramScraperService
{
    Task<InstagramPostDto> ScrapePost(string postUrlOrPostCode);
    Task<List<InstagramPostDto>> ScrapePosts(string username, byte pageNumber, byte pageSize = 12);
}

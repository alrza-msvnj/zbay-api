using Newtonsoft.Json;
using static Infrastructure.Dtos.InstagramDto;

namespace Api.Services;

public class ApifyInstagramScraperService : IInstagramScraperService
{
    #region Initialization

    private readonly HttpClient _httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(360)
    };
    private const string ApifyToken = "apify_api_XF4mc10247dwCQxaZQt7NHJ7LJsOMf2IXIU5";
    private static readonly string ApiUrl = $"https://api.apify.com/v2/acts/apify~instagram-post-scraper/runs?token={ApifyToken}";

    #endregion

    #region Methods

    public async Task<InstagramPostDto> ScrapePost(string postUrlOrPostCode)
    {
        throw new NotImplementedException();
    }

    public async Task<List<InstagramPostDto>> ScrapePosts(string username, byte pageNumber, byte pageSize = 12)
    {
        var input = new
        {
            username = new string[]
                {
                    "honey.jewelries"
                },
            resultsLimit = 2
        };

        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.apify.com/v2/acts/apify~instagram-post-scraper/run-sync-get-dataset-items");
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Authorization", $"Bearer {ApifyToken}");
        string requestBody = JsonConvert.SerializeObject(input, Formatting.Indented);
        var content = new StringContent(requestBody, null, "application/json");
        request.Content = content;
        var response = await client.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseContent);

        return new List<InstagramPostDto>();
    }

    #endregion
}

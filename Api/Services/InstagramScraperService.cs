using Api.Factories;
using Infrastructure.Dtos;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Api.Services;

public class InstagramScraperService : IInstagramScraperService
{
    #region Initialization

    private readonly HttpClient _httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(20)
    };
    private const string InstagramApiBaseUrl = "https://www.instagram.com/graphql/query";
    private readonly string InstagramDocumentId;
    private readonly string InstagramAccountDocumentId;
    private readonly List<string> userAgentList;

    public InstagramScraperService(IConfiguration configuration)
    {
        InstagramDocumentId = configuration.GetValue<string>("InstagramApi:InstagramDocumentId");
        InstagramAccountDocumentId = configuration.GetValue<string>("InstagramApi:InstagramAccountDocumentId");
        userAgentList = new()
        {
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
            "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:60.0) Gecko/20100101 Firefox/60.0",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Edge/91.0.864.64 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Safari/537.36 Edge/91.0.864.64",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36 Edg/91.0.864.64",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.82 Safari/537.36 Edge/89.0.774.75",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:58.0) Gecko/20100101 Firefox/58.0",
            "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.0",
            "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:50.0) Gecko/20100101 Firefox/50.0",
            "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:72.0) Gecko/20100101 Firefox/72.0",
            "Mozilla/5.0 (Windows NT 6.1; Trident/7.0; AS; AS; en-US) like Gecko",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:79.0) Gecko/20100101 Firefox/79.0",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:80.0) Gecko/20100101 Firefox/80.0",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.97 Safari/537.36",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 13_5_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.5.1 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.3 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 15_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.1 Mobile/19B74 Safari/604.1",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 13_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (iPad; CPU OS 13_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (iPad; CPU OS 14_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.2 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (iPad; CPU OS 15_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.1 Mobile/19B74 Safari/604.1",
            "Mozilla/5.0 (Windows NT 6.1; rv:61.0) Gecko/20100101 Firefox/61.0",
            "Mozilla/5.0 (Windows NT 6.1; rv:62.0) Gecko/20100101 Firefox/62.0",
            "Mozilla/5.0 (Windows NT 6.1; rv:64.0) Gecko/20100101 Firefox/64.0",
            "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:50.0) Gecko/20100101 Firefox/50.0",
            "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:78.0) Gecko/20100101 Firefox/78.0",
            "Mozilla/5.0 (X11; Linux x86_64; rv:80.0) Gecko/20100101 Firefox/80.0",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:76.0) Gecko/20100101 Firefox/76.0",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.82 Safari/537.36"
        };
    }

    #endregion

    #region Methods

    public async Task<InstagramDto.InstagramPostDto> ScrapePost(string postUrlOrPostCode)
    {
        string postShortCode;
        if (postUrlOrPostCode.Contains("http"))
        {
            postShortCode = postUrlOrPostCode.Split("/p/").Last().Split('/').First();
        }
        else
        {
            postShortCode = postUrlOrPostCode;
        }

        var variables = new Dictionary<string, object?>
        {
            { "shortcode", postShortCode },
            { "fetch_tagged_user_count", null },
            { "hoisted_comment_id", null },
            { "hoisted_reply_id", null }
        };
        var jsonVariables = JsonConvert.SerializeObject(variables, Formatting.None, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include
        });
        var encodedVariables = WebUtility.UrlEncode(jsonVariables);
        var body = $"variables={encodedVariables}&doc_id={InstagramDocumentId}";

        var request = new HttpRequestMessage(HttpMethod.Post, InstagramApiBaseUrl);
        request.Content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
        var randomUserAgent = userAgentList[new Random().Next(userAgentList.Count)];
        request.Headers.Add("User-Agent", randomUserAgent);
        request.Headers.Add("X-Requested-With", "XMLHttpRequest");
        request.Headers.Add("Referer", "https://www.instagram.com/");
        request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
        request.Headers.Add("Connection", "keep-alive");

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception("Response error.");
        }

        var result = await response.Content.ReadAsStringAsync();
        var instagramPostDto = InstagramFactory.MapInstagramPostToInstagramPostDto(result);

        return instagramPostDto;
    }

    public async Task<List<InstagramDto.InstagramPostDto>> ScrapePosts(string username, byte pageNumber, byte pageSize = 12)
    {
        string? endCursor = null;
        byte currentPage = 1;

        while (true)
        {
            var variables = new Dictionary<string, object?>
            {
                { "after", null },
                { "before", null },
                { "data", new Dictionary<string, object>
                {
                    { "count", pageSize },
                    { "include_reel_media_seen_timestamp", true },
                    { "include_relationship_info", true },
                    { "latest_besties_reel_media", true },
                    { "latest_reel_media", true },
                } },
                { "first", pageSize },
                { "last", null },
                { "username", username },
                { "__relay_internal__pv__PolarisIsLoggedInrelayprovider", true },
                { "__relay_internal__pv__PolarisShareSheetV3relayprovider", true }
            };

            var jsonVariables = JsonConvert.SerializeObject(variables, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include
            });
            var encodedVariables = WebUtility.UrlEncode(jsonVariables);
            var body = $"variables={encodedVariables}&doc_id={InstagramAccountDocumentId}";

            var request = new HttpRequestMessage(HttpMethod.Post, InstagramApiBaseUrl);
            request.Content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
            var randomUserAgent = userAgentList[new Random().Next(userAgentList.Count)];
            request.Headers.Add("User-Agent", randomUserAgent);
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("Referer", "https://www.instagram.com/");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            request.Headers.Add("Connection", "keep-alive");

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Response error.");
            }

            var result = await response.Content.ReadAsStringAsync();
            var instagramPostsDto = InstagramFactory.MapInstagramPostsToInstagramPostsDto(result);

            return instagramPostsDto;
        }
    }

    #endregion
}

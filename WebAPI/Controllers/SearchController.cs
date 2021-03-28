using WebAPI.SearchService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebAPI;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private const int CACHE_TIMER_IN_SECONDS = 3600;
        private readonly IMemoryCache _cache;
        private readonly ILogger<SearchController> _logger;
        private readonly ISearchService _searchService;
        public SearchController(ILogger<SearchController> logger, ISearchService searchService, IMemoryCache memoryCache)
        {
            _logger = logger;
            _searchService = searchService;
            _cache = memoryCache;
        }

        // GET Search/keywords/url
        [HttpGet("{keywords}/{url}")]
        public async Task<SearchItem> GetAsync(string keywords, string url)
        {
            if(string.IsNullOrEmpty(keywords.Trim()) || string.IsNullOrEmpty(url.Trim()))
            {
                _logger.Log(LogLevel.Error, "keyword or url can not be empty or null");
                throw new ArgumentNullException();
            }

            _logger.Log(LogLevel.Information, $"Search keyword '{keywords}' with '{url}'");

            string cacheKey = keywords + url;
            SearchItem cacheEntry;

            // Look for cache key.
            if (!_cache.TryGetValue(cacheKey, out cacheEntry))
            {
                _logger.Log(LogLevel.Information, "Cache missed.");
                // Key not in cache, so get data.
                var html = await _searchService.GetRawResult(keywords);
                string result = _searchService.Parse(html, url);

                cacheEntry = new SearchItem()
                {
                    Keywords = keywords,
                    Url = url,
                    Result = result
                };

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(CACHE_TIMER_IN_SECONDS));

                // Save data in cache.
                _cache.Set(cacheKey, cacheEntry, cacheEntryOptions);
            }

            return cacheEntry;
        }
    }
}

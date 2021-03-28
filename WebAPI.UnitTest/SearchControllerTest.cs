using Controllers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using WebAPI.SearchService;

namespace WebAPI.UnitTest
{
    class SearchControllerTest
    {


        [SetUp]
        public void UnitTestBaseSetUp()
        {
        }

        [Test]
        public void TestGetAsync_Keywords_NullOrEmpty_Throw_ExceptionAsync()
        {
            var mockSearchServiceObj = new Mock<ISearchService>();
            var mockLoggerObj = new Mock<ILogger<SearchController>>();
            var mockCacheObj = new Mock<IMemoryCache>();

            SearchController searchController = new SearchController(mockLoggerObj.Object, mockSearchServiceObj.Object, mockCacheObj.Object);
            Assert.ThrowsAsync<ArgumentNullException>(async ()=> await searchController.GetAsync("", "url"));
        }

        [Test]
        public void TestGetAsync_Url_NullOrEmpty_Throw_Exception()
        {
            var mockSearchServiceObj = new Mock<ISearchService>();
            var mockLoggerObj = new Mock<ILogger<SearchController>>();
            var mockCacheObj = new Mock<IMemoryCache>();

            SearchController searchController = new SearchController(mockLoggerObj.Object, mockSearchServiceObj.Object, mockCacheObj.Object);
            Assert.ThrowsAsync<ArgumentNullException>(async () => await searchController.GetAsync("keywords", ""));
        }

        [Test]
        public async Task TestGetAsync_CatchKey_Found_Fetch_ResultFromCatchAsync()
        {
            var mockSearchServiceObj = new Mock<ISearchService>();
            var mockLoggerObj = new Mock<ILogger<SearchController>>();
            var searchItem = new SearchItem() { Keywords = "keywords", Url = "url", Result = "1,2" };
            var cache = GetMemoryCache();

            SearchController searchController = new SearchController(mockLoggerObj.Object, mockSearchServiceObj.Object, cache);
            cache.Set("keywordsurl", searchItem);

            var result = await searchController.GetAsync("keywords", "url");
            Assert.AreEqual(searchItem, result);
        }

        [Test]
        public async Task TestGetAsync_CatchKey_NotFound_Fetch_ResultFromSearchServiceAsync()
        {
            var mockSearchServiceObj = new Mock<ISearchService>();
            var mockLoggerObj = new Mock<ILogger<SearchController>>();
            var cache = GetMemoryCache();
            
            SearchController searchController = new SearchController(mockLoggerObj.Object, mockSearchServiceObj.Object, cache);

            mockSearchServiceObj.Setup(x => x.GetRawResult(It.IsAny<string>())).Returns(Task.FromResult("html"));
            mockSearchServiceObj.Setup(x => x.Parse(It.IsAny<string>(), It.IsAny<string>())).Returns("result");
            
            var result = await searchController.GetAsync("keywords", "url");
            mockSearchServiceObj.Verify(x => x.GetRawResult(It.IsAny<string>()), Times.Once);
            Assert.AreEqual(new SearchItem() { Keywords="keywords", Url="url", Result="result" }, result);
        }

        private IMemoryCache GetMemoryCache()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();

            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            return memoryCache;
        }
    }
}

using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Dynamic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string WEB_API_END_POINT = "http://localhost:5000/search/{0}/{1}";
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Index(string keywords, string url)
        {
            if (!string.IsNullOrEmpty(keywords) && !string.IsNullOrEmpty(url))
            {
                string friendlyKeywords = HttpUtility.JavaScriptStringEncode(keywords);
                string friendlyUrl = HttpUtility.JavaScriptStringEncode(url);
                HttpResponseMessage result = await _httpClient.GetAsync(string.Format(WEB_API_END_POINT, friendlyKeywords, friendlyUrl));

                string jsonString = await result.Content.ReadAsStringAsync();
                dynamic obj = JsonSerializer.Deserialize<ExpandoObject>(jsonString);

                ViewData["result"] = obj.result.ToString();
            }
            else
            {
                ViewData["result"] = "keywords or url cannot be empty!";
            }
            return View("Index");
        }
    }
}

using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace WebAPI.SearchService
{
    public class GoogleSearch : ISearchService
    {
        //<div class="BNeawe UPmit AP7Wnd"></div>
        private const string REGULAR_EXPRESSION_RECORD_MATCH_STRING = "<div class=\"BNeawe UPmit AP7Wnd\">[^<]*<\\/div>";
        private const int TOP_100_RECORDS = 100;
        private const string GoogleQueryString = "https://www.google.com.au/search?q={0}&num={1}";
        private const string NONE_STRING = "0";

        private readonly Regex _regex = new Regex(REGULAR_EXPRESSION_RECORD_MATCH_STRING);
        private readonly HttpClient _httpClient;

        public GoogleSearch(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> GetRawResult(string keywords)
        {
            if (string.IsNullOrEmpty(keywords))
            {
                throw new ArgumentNullException();
            }

            var urlFriendlyString = HttpUtility.JavaScriptStringEncode(keywords);
            var result = await _httpClient.GetAsync(string.Format(GoogleQueryString, keywords, TOP_100_RECORDS));

            return await result.Content.ReadAsStringAsync();
        }

        public string Parse(string rawResult, string url)
        {
            if(string.IsNullOrEmpty(rawResult) || string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException();
            }

            var matches = _regex.Matches(rawResult);
            if (matches.Count == 0)
            {
                return NONE_STRING;
            }
            else
            {
                var result = string.Empty;
                for (int i = 0; i < matches.Count; i++)
                {
                    if (matches[i].Value.Contains(url))
                    {
                        result += (i + 1) + ",";
                    }
                }
                if (string.IsNullOrEmpty(result))
                {
                    return NONE_STRING;
                }
                else
                {
                    return result;
                }
            }
        }
    }
}

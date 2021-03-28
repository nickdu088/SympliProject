using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebAPI.SearchService
{
    public interface ISearchService
    {
        Task<string> GetRawResult(string keywords);
        string Parse(string rawResult, string url);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.SearchService
{
    public class BingSearch : ISearchService
    {
        public Task<string> GetRawResult(string keywords)
        {
            throw new NotImplementedException();
        }

        public string Parse(string rawResult, string url)
        {
            throw new NotImplementedException();
        }
    }
}

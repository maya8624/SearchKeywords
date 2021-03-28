using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SearchKeywords.Models;
using SearchKeywords.Services;
using SearchKeywords.ViewModels;
using SearchKeyWords.Interface;
using SearchKeyWords.Services;

namespace SearchKeyWords.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchKeywordsController : ControllerBase
    {
        private readonly IList<ISearchEngineService> _searchEngineServices;
        private readonly IEngineApplication _engineApplication;

        public SearchKeywordsController(IEngineApplication engineApplication)
        {
            _searchEngineServices = new List<ISearchEngineService>();
            _engineApplication = engineApplication;
        }

        [HttpGet]
        [Route("{keywords}/{url}")]
        public async Task<List<SearchResultView>> Get(string keywords, string url = "https://www.infortack.com.au")
        {
            // Task<SearchResultView[]>
            if (string.IsNullOrEmpty(keywords) || string.IsNullOrEmpty(url))
            {
                return null;
            }

            var tasks = new List<Task<SearchResultView>>();

            _searchEngineServices.Add(new GoogleService(_engineApplication));
            _searchEngineServices.Add(new BingService(_engineApplication));

            foreach (var service in _searchEngineServices)
            {
                tasks.Add(service.GetAllPagesAsync(keywords, url));
            }

            // public static async Task<User[]> GetUsersAsync(IEnumerable<int> userIds)
            // var getTasks =_searchEngineServices.Select(service => service.GetResult(keywords, url));

            IEnumerable<SearchResultView> results = await Task.WhenAll<SearchResultView>(tasks);

            return results.ToList();
        }
    }
}
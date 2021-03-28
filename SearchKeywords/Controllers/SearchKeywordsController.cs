using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SearchKeyWords.ViewModels;
using SearchKeyWords.Interface;
using SearchKeyWords.Services;

namespace SearchKeyWords.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchKeywordsController : ControllerBase
    {
        private readonly IList<ISearchEngineService> searchEngineServices;
        private readonly IEngineProcess engineProcess;

        public SearchKeywordsController(IEngineProcess engineProcess)
        {
            searchEngineServices = new List<ISearchEngineService>();
            this.engineProcess = engineProcess;
        }

        [HttpGet]
        [Route("{searchKeywords}/{searchUrl}")]        
        public async Task<SearchResultView[]> GetPages(string searchKeywords, string searchUrl)
        {            
            if (string.IsNullOrEmpty(searchKeywords) || string.IsNullOrEmpty(searchUrl))
            {
                return null;
            }

            var tasks = new List<Task<SearchResultView>>();

            searchEngineServices.Add(new GoogleService(engineProcess));
            searchEngineServices.Add(new BingService(engineProcess));

            foreach (var service in searchEngineServices)
            {
                tasks.Add(service.GetAllPagesAsync(searchKeywords, Uri.UnescapeDataString(searchUrl)));
            }

            return await Task.WhenAll(tasks);            
        }
    }
}
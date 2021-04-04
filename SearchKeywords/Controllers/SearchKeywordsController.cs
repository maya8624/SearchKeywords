using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SearchKeyWords.ViewModels;
using SearchKeyWords.Interface;
using SearchKeyWords.Services;

namespace SearchKeyWords.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchKeywordsController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly ISearchEngineService searchEngineService;

        public SearchKeywordsController(ISearchEngineService searchEngineService, ILogger<SearchKeywordsController> logger)
        {
            this.searchEngineService = searchEngineService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("{searchKeywords}/{searchUrl}")]
        public async Task<List<SearchResultView>> GetPages(string searchKeywords, string searchUrl)
        {            
            if (string.IsNullOrEmpty(searchKeywords) || string.IsNullOrEmpty(searchUrl))
            {
                return null;
            }

            IEnumerable<SearchResultView> results = new List<SearchResultView>();

            try
            {
                // Add search engines
                var engineServices = new SearchEngineService();
                engineServices.RegisterEngineService(new GoogleService(searchEngineService));
                engineServices.RegisterEngineService(new BingService(searchEngineService));
                                
                results = await engineServices.SearchPagesAsync(searchKeywords, Uri.UnescapeDataString(searchUrl));
               
                logger.LogInformation("Search finished successfully at: {time}", DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.LogInformation("An error occured.", ex.Message);
            }

            return results.ToList();
        }
    }
}
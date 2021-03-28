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
        private readonly ISearchEngineProcess engineProcess;

        public SearchKeywordsController(ISearchEngineProcess engineProcess, ILogger<SearchKeywordsController> logger)
        {            
            this.logger = logger;
            this.engineProcess = engineProcess;
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
                var engineServices = new SearchEngineProcess();
                engineServices.RegisterEngineService(new GoogleService(engineProcess));
                engineServices.RegisterEngineService(new BingService(engineProcess));
                                
                results = await engineServices.SearchPages(searchKeywords, Uri.UnescapeDataString(searchUrl));
               
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
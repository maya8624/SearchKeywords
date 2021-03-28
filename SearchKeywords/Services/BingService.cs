using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SearchKeywords.ViewModels;
using SearchKeyWords.Interface;

namespace SearchKeywords.Services
{
    public class BingService : ISearchEngineService
    {
        private readonly IEngineApplication engineApplication;
        
        public BingService(IEngineApplication engineApplication)
        {            
            this.engineApplication = engineApplication;
        }

        public string EngineName => "Bing";

        public async Task<SearchResultView> GetAllPagesAsync(string keywords, string url)
        {
            var result = await engineApplication.GetPageNumbersAsync(EngineName);
            
            return result;  
        }
    }
}
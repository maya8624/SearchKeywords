using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SearchKeywords.ViewModels;
using SearchKeyWords.Interface;

namespace SearchKeywords.Services
{
    public class GoogleService : ISearchEngineService
    {        
        private readonly IEngineApplication engineApplication;

        public GoogleService( IEngineApplication engineApplication)
        {            
            this.engineApplication = engineApplication;            
        }

        public string EngineName => "Google";

        public async Task<SearchResultView> GetAllPagesAsync(string keywords, string url)
        {
            var result = await engineApplication.GetPageNumbersAsync(EngineName);

            return result;
        }
    }
}
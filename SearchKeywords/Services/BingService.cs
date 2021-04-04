
using System.Threading.Tasks;
using SearchKeyWords.ViewModels;
using SearchKeyWords.Interface;

namespace SearchKeyWords.Services
{
    public class BingService : ISearchPage
    {
        private readonly ISearchEngineService searchEngineService;
        
        public BingService(ISearchEngineService searchEngineService)
        {            
            this.searchEngineService = searchEngineService;
        }

        public string EngineName => "Bing";

        public async Task<SearchResultView> GetAllPagesAsync(string searchKeywords, string searchUrl)
        {
            return await searchEngineService.GetPageNumbersAsync(EngineName, searchKeywords, searchUrl);            
        }
    }
}
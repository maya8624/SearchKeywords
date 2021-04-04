using System.Threading.Tasks;
using SearchKeyWords.ViewModels;
using SearchKeyWords.Interface;

namespace SearchKeyWords.Services
{
    public class GoogleService : ISearchPages
    {       
        private readonly ISearchEngineService searchEngineService;

        public GoogleService(ISearchEngineService searchEngineService)
        {            
            this.searchEngineService = searchEngineService;            
        }

        public string EngineName => "Google";

        public async Task<SearchResultView> GetAllPagesAsync(string searchKeywords, string searchUrl)
        {
            return await searchEngineService.GetPageNumbersAsync(EngineName, searchKeywords, searchUrl);          
        }
    }
}
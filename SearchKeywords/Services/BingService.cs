
using System.Threading.Tasks;
using SearchKeyWords.ViewModels;
using SearchKeyWords.Interface;

namespace SearchKeyWords.Services
{
    public class BingService : ISearchEngineService
    {
        private readonly ISearchEngineProcess engineProcess;
        
        public BingService(ISearchEngineProcess engineProcess)
        {            
            this.engineProcess = engineProcess;
        }

        public string EngineName => "Bing";

        public async Task<SearchResultView> GetAllPagesAsync(string searchKeywords, string searchUrl)
        {
            return await engineProcess.GetPageNumbersAsync(EngineName, searchKeywords, searchUrl);            
        }
    }
}
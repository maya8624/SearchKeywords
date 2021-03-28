using System.Threading.Tasks;
using SearchKeyWords.ViewModels;
using SearchKeyWords.Interface;

namespace SearchKeyWords.Services
{
    public class GoogleService : ISearchEngineService
    {        
        private readonly ISearchEngineProcess engineProcess;

        public GoogleService(ISearchEngineProcess engineProcess)
        {            
            this.engineProcess = engineProcess;            
        }

        public string EngineName => "Google";

        public async Task<SearchResultView> GetAllPagesAsync(string searchKeywords, string searchUrl)
        {
            return await engineProcess.GetPageNumbersAsync(EngineName, searchKeywords, searchUrl);
        }
    }
}
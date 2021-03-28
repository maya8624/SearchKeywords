using System.Threading.Tasks;
using SearchKeyWords.ViewModels;
using SearchKeyWords.Interface;

namespace SearchKeyWords.Services
{
    public class GoogleService : ISearchEngineService
    {        
        private readonly IEngineProcess engineProcess;

        public GoogleService(IEngineProcess engineProcess)
        {            
            this.engineProcess = engineProcess;            
        }

        public string EngineName => "Google";

        public async Task<SearchResultView> GetAllPagesAsync(string searchKeywords, string searchUrl)
        {
            var result = await engineProcess.GetPageNumbersAsync(EngineName, searchKeywords, searchUrl);

            return result;
        }
    }
}
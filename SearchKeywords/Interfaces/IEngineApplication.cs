using System.Collections.Generic;
using System.Threading.Tasks;
using SearchKeywords.Models;
using SearchKeywords.ViewModels;

namespace SearchKeyWords.Interface
{
    public interface IEngineApplication
    {
        string InsertCharacter(int index, string character, string original);

        Task<SearchResultView> GetPageNumbersAsync(string engineName);

        Task<string> GetResponseBodyAsync(string requestUrl);
        
        SearchEngine GetSearchEngine(string engineName);
    }
}

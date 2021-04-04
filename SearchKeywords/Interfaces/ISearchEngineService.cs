using System.Threading.Tasks;
using SearchKeyWords.Models;
using SearchKeyWords.ViewModels;

namespace SearchKeyWords.Interface
{
    public interface ISearchEngineService
    {
        string InsertCharacter(int index, string character, string original);

        Task<SearchResultView> GetPageNumbersAsync(string engineName, string searchKeywords, string searchUrl);

        Task<string> GetResponseBodyAsync(string requestUrl);

        SearchEngine GetSearchEngine(string engineName);
    }
}

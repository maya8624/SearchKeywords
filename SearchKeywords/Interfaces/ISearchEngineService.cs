using System.Threading.Tasks;
using SearchKeyWords.ViewModels;

namespace SearchKeyWords.Interface
{
    public interface ISearchEngineService
    {
        string EngineName { get; }

        Task<SearchResultView> GetAllPagesAsync(string searchKeywords, string searchUrl);
    }
}
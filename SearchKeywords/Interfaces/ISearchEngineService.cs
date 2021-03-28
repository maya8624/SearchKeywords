using System.Threading.Tasks;
using SearchKeywords.ViewModels;

namespace SearchKeyWords.Interface
{
    public interface ISearchEngineService
    {
        string EngineName { get; }

        Task<SearchResultView> GetAllPagesAsync(string keywords, string url);
    }
}
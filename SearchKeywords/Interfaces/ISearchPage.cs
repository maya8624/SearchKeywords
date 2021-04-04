using System.Threading.Tasks;
using SearchKeyWords.Models;
using SearchKeyWords.ViewModels;

namespace SearchKeyWords.Interface
{
    public interface ISearchPage
    {
        Task<SearchResultView> GetAllPagesAsync(string searchKeywords, string searchUrl);
    }
}
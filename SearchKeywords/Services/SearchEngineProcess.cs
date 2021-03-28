using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SearchKeyWords.ViewModels;
using SearchKeyWords.Interface;
using SearchKeyWords.Models;

namespace SearchKeyWords.Services
{
    public class SearchEngineProcess : ISearchEngineProcess
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private readonly IList<ISearchEngineService> _engineServices;
     
        public SearchEngineProcess()
        {
            _engineServices = new List<ISearchEngineService>();
        }

        public SearchEngineProcess(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
            this.configuration = configuration;
        }

        public string InsertCharacter(int index, string character, string original)
        {
            return original.Insert(index, character);
        }

        /// <summary>
        /// Get the response body as string
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns>response body</returns>
        public async Task<string> GetResponseBodyAsync(string requestUrl)
        {           
            var client = clientFactory.CreateClient();
            return await client.GetStringAsync(requestUrl);
        }

        /// <summary>
        /// Set request url -> get response body -> check the search url in the response body
        /// </summary>
        /// <param name="startPage"></param>
        /// <param name="engineUrl"></param>
        /// <param name="searchUrl"></param>
        /// <returns>Task string</returns>
        private async Task<string> GetPageWithBodyHasUrlAsync(int startPage, string engineUrl, string searchUrl)
        {
            string insertChar = "0";
            string page = startPage < 10 ? InsertCharacter(0, insertChar, startPage.ToString()): startPage.ToString();
            string requestUrl = $"{engineUrl}/Page{page}.html";

            var responseBody = await GetResponseBodyAsync(requestUrl);
            if (responseBody.Contains(searchUrl))
            {
                return page;
            }

            return null;
        }

        /// <summary>
        /// Get search engine details from appsettings.json and call GetPageWithBodyHasUrlAsync() to check engine pages
        /// </summary>
        /// <param name="engineName"></param>
        /// <param name="searchKeywords"></param>
        /// <param name="searchUrl"></param>
        /// <returns>search results which has the search url</returns>
        public async Task<SearchResultView> GetPageNumbersAsync(string engineName, string searchKeywords, string searchUrl)
        {
            var engine = GetSearchEngine(engineName);

            var result = new SearchResultView {
                Name = engine.Name, 
                Keywords = searchKeywords, 
                Url = searchUrl
            };

            List<Task<string>> tasks = new List<Task<string>>();
            
            while (engine.StartPage <= engine.LastPage)
            {
                tasks.Add(GetPageWithBodyHasUrlAsync(engine.StartPage, engine.Url, searchUrl));
                engine.StartPage++;
            }
                                
            IEnumerable<string> pages = await Task.WhenAll(tasks);

            pages = pages.Where(p => p != null);
            result.Pages = pages.Any() ? string.Join(", ", pages) : "no search results found.";
                              
            return result;
        }

        public SearchEngine GetSearchEngine(string engineName)
        {
            return configuration.GetSection("SearchEngines")
                                    .Get<List<SearchEngine>>()
                                    .SingleOrDefault(e => e.Name.ToLower() == engineName.ToLower());
        }

        public void RegisterEngineService(ISearchEngineService engineService)
        {
            _engineServices.Add(engineService);            
        }

        public async Task<SearchResultView[]> SearchPages(string searchKeywords, string searchUrl)
        {            
            var tasks = new List<Task<SearchResultView>>();
          
            foreach (var service in _engineServices)
            {
                tasks.Add(service.GetAllPagesAsync(searchKeywords, Uri.UnescapeDataString(searchUrl)));
            }

            return await Task.WhenAll(tasks);
        }
    }
}
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
    public class EngineProcess : IEngineProcess
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
    
        public EngineProcess(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
            this.configuration = configuration;
        }

        public string InsertCharacter(int index, string character, string original)
        {
            return original.Insert(index, character);
        }

        private async Task<string> GetResponseBodyAsync(string requestUrl)
        {            
            var client = clientFactory.CreateClient();
            return await client.GetStringAsync(requestUrl);            
        }

        private async Task<string> GetPageWithBodyHasUrlAsync(int startPage, string engineUrl, string searchUrl)
        {   
            try
            {
                string page = startPage < 10 ? InsertCharacter(0, "0", startPage.ToString()): startPage.ToString();
                string requestUrl = $"{engineUrl}/Page{page}.html";

                var responseBody = await GetResponseBodyAsync(requestUrl);
                if (responseBody.Contains(searchUrl))
                    return page;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception" + ex.Message);
            }

            return null;
        }

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
    }
}
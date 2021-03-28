using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SearchKeywords.Models;
using SearchKeywords.ViewModels;
using SearchKeyWords.Interface;

namespace SearchKeyWords.Services
{
    public class EngineApplication : IEngineApplication
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;

        public EngineApplication(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
            this.configuration = configuration;            
        }

        public string InsertCharacter(int index, string character, string original)
        {
            return original.Insert(index, character);
        }

        public async Task<string> GetResponseBodyAsync(string requestUrl)
        {            
            var client = clientFactory.CreateClient();
            return await client.GetStringAsync(requestUrl); 
        }

        private async Task<string> GetPageWithBodyHasUrlAsync(int startPage, string url)
        {   
            try
            {
                string page = startPage < 10 ? InsertCharacter(0, "0", startPage.ToString()): startPage.ToString();
                string requestUrl = $"{url}/Page{page}.html";

                var responseBody = await GetResponseBodyAsync(requestUrl);
                if (responseBody.Contains(url))
                    return page;

                return null;
            }
            catch (System.Exception)
            {                
                throw;
            }           
        }

        public async Task<SearchResultView> GetPageNumbersAsync(string engineName)
        {
            var engine = GetSearchEngine(engineName);

            var result = new SearchResultView {Name = engine.Name};
            List<Task<string>> tasks = new List<Task<string>>();

            try
            { 
                while (engine.StartPage <= engine.LastPage)
                {
                    tasks.Add(GetPageWithBodyHasUrlAsync(engine.StartPage, engine.Url));
                    engine.StartPage++;
                }

                IEnumerable<string> pages = await Task.WhenAll(tasks);
                pages = pages.Where(p => p != null);

                result.Pages = pages.Any() ? string.Join(", ", pages) : "no search results found.";
            }
            catch (AggregateException)
            {
                throw;
            }
            
            return result;
        }

        public SearchEngine GetSearchEngine(string engineName)
        {
            return configuration.GetSection("SearchEngines")
                                    .Get<List<SearchEngine>>()
                                    .SingleOrDefault(e => e.Name == engineName.ToLower());
        }
    }
}
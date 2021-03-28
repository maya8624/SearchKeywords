using Microsoft.Extensions.Configuration;
using SearchKeywords.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SearchKeywordsTests.Services
{
    public class FakeGetEngines
    {

        public SearchEngine GetSearchEngine(string engineName)
        {           
            var inMemorySettings = new Dictionary<string, string>
            {
                {"SearchEngines:Name", "Google"},
                {"SearchEngines:StartPage",  "1"},
                {"SearchEngines:LastPage", "10"},
                {"SearchEngines:Url", "https://infotrack-tests.infotrack.com.au/Google/" }
            };
          
            IConfiguration configurationBind = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

         
            var result = configurationBind.GetSection("SearchEngines").Get<List<SearchEngine>>()
                .SingleOrDefault(e => e.Name == engineName.ToLower());

            return result;
        }
    }
}

using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SearchKeyWords.Models;
using SearchKeyWords.Services;
using System.Collections.Generic;
using System.Net.Http;

namespace SearchKeyWordsTests.Services
{
    [TestFixture]
    public class SearchEngineProcessTests
    {
        private Mock<IHttpClientFactory> clientFactory;
        private Mock<IConfiguration> configuration;        
        private SearchEngineProcess engineProcess;

        [SetUp]
        public void SetUp()
        {
            clientFactory = new Mock<IHttpClientFactory>();
            configuration = new Mock<IConfiguration>();
            engineProcess = new SearchEngineProcess(configuration.Object, clientFactory.Object);
        }

        [Test]
        [TestCase(0, "0", "1", "01")]
        [TestCase(4, "e", "appl", "apple")]
        public void InsertCharacter_AddACharToOrginal_ReturnOrginalWordsWithAChar(
            int index,
            string character,
            string original,
            string expectedResult)
        {
            var result = engineProcess.InsertCharacter(index, character, original);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
                          

        [Test]
        public void GetSearchEngine_WhenCalled_ReturnSearchEngineDetails()
        {         
            //Arrange
            var inMemorySettings = new Dictionary<string, string>()
            {
                {"SearchEngines:Name", "Google"},
                {"SearchEngines:StartPage",  "1"},
                {"SearchEngines:LastPage", "10"},
                {"SearchEngines:Url", "https://infotrack-tests.infotrack.com.au/Google/" }
            };
            
            IConfiguration configurationBind = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Action
            var result = configurationBind.GetSection("SearchEngines").Get<SearchEngine>();
            
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Google").IgnoreCase);
        }
    }
}

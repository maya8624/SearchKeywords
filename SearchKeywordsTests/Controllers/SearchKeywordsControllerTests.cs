using Moq;
using NUnit.Framework;
using SearchKeyWords.Models;
using SearchKeyWords.ViewModels;
using SearchKeyWords.Controllers;
using SearchKeyWords.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SearchKeyWordsTests.Controllers
{
    [TestFixture]
    public class SearchKeywordsControllerTests
    {
        private Mock<ISearchEngineService> searchEngineService;
        private Mock<ILogger<SearchKeywordsController>> logger;        
        private SearchKeywordsController controller;

        [SetUp]
        public void SetUp()
        {
            searchEngineService = new Mock<ISearchEngineService>();
            logger = new Mock<ILogger<SearchKeywordsController>>();            
            controller = new SearchKeywordsController(searchEngineService.Object, logger.Object);
        }

        [Test]
        public async Task Get_WhenCalledWithoutParamValue_ReturnNull()
        {
            // Arrange
            string keywords = string.Empty;
            string url = "www.infotrack.com.au";

            // Action
            var result = await controller.GetPages(keywords, url);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Get_WhenCalled_ReturnSearchResultView()
        {
            // Arrange
            string keywords = "online";
            string url = "www.infotrack.com.au";

            var searchEngine = new SearchEngine
            {
                Name = "Google",
                StartPage = 1,
                LastPage = 10,
                Url = "https://www.infortack.com.au/Google"
            };

            var searchResult = new SearchResultView
            {
                Keywords = "online",
                Name = "Google",
                Pages = "1, 3, 5",
                Url = "https://www.infortack.com.au"
            };

            var engines = new List<SearchEngine>
            {
                new SearchEngine
                {
                    Name = "Google",
                    StartPage = 1,
                    LastPage = 10,
                    Url = "https://www.infortack.com.au/Google"
                },
                 new SearchEngine
                {
                    Name = "Bing",
                    StartPage = 1,
                    LastPage = 10,
                    Url = "https://www.infortack.com.au/Google"
                }
            };

            searchEngineService.Setup(e => e.GetSearchEngine("google")).Returns(searchEngine);
            searchEngineService.Setup(e => e.GetPageNumbersAsync(It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync(searchResult);

            // Action
            var result = await controller.GetPages(keywords, url);

            // Assert
            Assert.That(result.Any, Is.True);
            Assert.That(result[0].Name, Is.EqualTo("Google").IgnoreCase);
        }       
    }
}

using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SearchKeywords.Models;
using SearchKeywords.ViewModels;
using SearchKeyWords.Controllers;
using SearchKeyWords.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchKeyWordsTests.Controllers
{
    [TestFixture]
    public class SearchKeywordsControllerTests
    {
        private SearchKeywordsController _controller;
        private Mock<IEngineApplication> _engine;
        private Mock<ISearchEngineService> _service;

        [SetUp]
        public void SetUp()
        {
            _engine = new Mock<IEngineApplication>();
            _service = new Mock<ISearchEngineService>();
            _controller = new SearchKeywordsController(_engine.Object);
        }

        [Test]
        public async Task Get_WhenCalledWithoutParamValue_ReturnNull()
        {
            // Arrange
            string keywords = string.Empty;
            string url = "www.infotrack.com.au";

            // Action
            var result = await _controller.Get(keywords, url);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Get_WhenCalled_ReturnListOfSearchResult()
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

           // _engine.Setup(e => e.GetSearchEngine("google")).Returns(searchEngine);
          
            _engine.Setup(e => e.GetPageNumbersAsync(It.IsAny<string>())).ReturnsAsync(searchResult);
            _service.Setup(e => e.GetAllPagesAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(searchResult);

            // Action
            var result = await _controller.Get(keywords, url);

            // Assert
            Assert.That(result.Any, Is.True);
            Assert.That(result[0].Name, Is.EqualTo("Google").IgnoreCase);
        }
       
    }
}

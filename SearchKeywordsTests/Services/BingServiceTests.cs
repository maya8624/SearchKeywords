using Moq;
using NUnit.Framework;
using SearchKeyWords.Models;
using SearchKeyWords.Services;
using SearchKeyWords.ViewModels;
using SearchKeyWords.Interface;
using System.Threading.Tasks;

namespace SearchKeyWordsTests.Services
{
    public class BingServiceTests
    {
        private Mock<ISearchEngineService> engine;
        private BingService bing;

        [SetUp]
        public void SetUp()
        {
            engine = new Mock<ISearchEngineService>();
            bing = new BingService(engine.Object);
        }

        [Test]
        public async Task GetAllPagesAsync_WhenCalled_Return_ListOfSearchResultView()
        {
            var searchEngine = new SearchEngine
            {
                Name = "Bing",
                StartPage = 1,
                LastPage = 10,
                Url = "https://www.infortack.com.au/Bing"
            };

            var searchResult = new SearchResultView
            {
                Keywords = "online title search",
                Name = "Bing",
                Pages = "1, 3, 5",
                Url = "https://www.infortack.com.au"
            };

            engine.Setup(e => e.GetSearchEngine("bing")).Returns(searchEngine);
            engine.Setup(e => e.GetPageNumbersAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(searchResult);
                        
            var result = await bing.GetAllPagesAsync(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Bing").IgnoreCase);
        }
    }
}

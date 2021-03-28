using Moq;
using NUnit.Framework;
using SearchKeyWords.Models;
using SearchKeyWords.Services;
using SearchKeyWords.ViewModels;
using SearchKeyWords.Interface;
using System.Threading.Tasks;

namespace SearchKeyWordsTests.Services
{
    public class GogleServiceTests
    {
        private Mock<ISearchEngineProcess> engine;

        private GoogleService google;

        [SetUp]
        public void SetUp()
        {
            engine = new Mock<ISearchEngineProcess>();
            google = new GoogleService(engine.Object);
        }

        [Test]
        public async Task GetAllPagesAsync_WhenCalled_Return_ListOfSearchResultView()
        {
            var searchEngine = new SearchEngine
            {
                Name = "Google",
                StartPage = 1,
                LastPage = 10,
                Url = "https://www.infortack.com.au/Google"
            };

            var searchResult = new SearchResultView
            {
                Keywords = "online title search",
                Name = "Google",
                Pages = "1, 3, 5",
                Url = "https://www.infortack.com.au"
            };

            engine.Setup(e => e.GetSearchEngine("google")).Returns(searchEngine);
            engine.Setup(e => e.GetPageNumbersAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(searchResult);

            var result = await google.GetAllPagesAsync(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("google").IgnoreCase);
        }
    }
}

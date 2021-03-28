using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SearchKeywords.Models;
using SearchKeyWords.Interface;
using SearchKeyWords.Services;
using SearchKeywordsTests.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SearchKeyWordsTests.Services
{
    [TestFixture]
    public class EngineApplicationTests
    {
        public Mock<IHttpClientFactory> clientFactory;
        public Mock<IConfiguration> configuration;
        public Mock<IEngineApplication> iengine;
        public EngineApplication engineApplication;

        [SetUp]
        public void SetUp()
        {
            clientFactory = new Mock<IHttpClientFactory>();
            configuration = new Mock<IConfiguration>();
            iengine = new Mock<IEngineApplication>();
            engineApplication = new EngineApplication(configuration.Object, clientFactory.Object);
        }

        [Test]
        [TestCase(0, "0", "1", "01")]
        [TestCase(4, "e", "appl", "apple")]
        public void InsertCharacter_AddACharToOrginal_ReturnOrginalWordsWithAChar(int index,
                                                                                  string character,
                                                                                  string original,
                                                                                  string expectedResult)
        {
            var result = engineApplication.InsertCharacter(index, character, original);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetPageNumbersAsync_WhenCalled_ReturnSearchResult()
        {
            //string engineName, int startPage, int lastPage, string url
            

        }

        [Test]
        public async Task GetResponseBody_WhenCalled_ReturnResponseBodyAsString()
        {
            iengine.Setup(e => e.GetResponseBodyAsync(It.IsAny<string>()))
                .ReturnsAsync("Lorem Ipsum is simply dummy text of the printing and typesetting industry");

          
          
            var client = new HttpClient();
            
             clientFactory.Setup(c => c.CreateClient(It.IsAny<string>())).Returns(client).Verifiable();

           // var client2 = new Mock<HttpClient>();
            //client2.Setup(c => c.GetStringAsync("https://www.infortack.com.au/Google/page01.html")).ReturnsAsync("afasf");

            

            var result = await engineApplication.GetResponseBodyAsync("https://www.infortack.com.au/Google/page01.html");


        }

        [Test]
        public void GetSearchEngine_WhenCalled_ReturnSearchEngineDetails()
        {
            // Arrange
            string engineName = "Google";
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
                    Name = "Google",
                    StartPage = 1,
                    LastPage = 10,
                    Url = "https://www.infortack.com.au/Google"
                }
            };

            var engine = new SearchEngine
            {
                Name = "Google",
                StartPage = 1,
                LastPage = 10,
                Url = "https://www.infortack.com.au/Google"
            };


            // Action
            iengine.Setup(e => e.GetSearchEngine(It.IsAny<string>())).Returns(engine);

            iengine.Verify();
            //var mockIConfigurationSections = new Mock<List<IConfigurationSection>>();




            //configuration.Setup(c => c.GetSection("SearchEngines")
            //                        .Get<List<SearchEngine>>()
            //                        .Where(e => e.Name == engineName.ToLower()).ToList()).Returns(engine);

            //var configuration2 = new Mock<IConfiguration>();
            //configuration2.Setup(c => c.GetSection("SearchEngines")).Returns(mockChildrenSection.Object);

            //Arrange
            //var inMemorySettings = new Dictionary<string, string>
            //{
            //    {"Name", "Google"},
            //    {"StartPage",  "1"},
            //    {"LastPage", "10"},
            //    {"Url", "https://infotrack-tests.infotrack.com.au/Google/" }
            //};

            //IConfiguration configurationBind = new ConfigurationBuilder()
            //    .AddInMemoryCollection(inMemorySettings)
            //    .Build();


            //var test = new FakeGetEngines();
            //var result = test.GetSearchEngine("google");

            // configuration.Setup(c => c.GetSection(It.IsAny<string>())).Returns(engines);

            //// Action act = () => configurationBind.GetSection("SearchEngines").Get<List<SearchEngine>>();


           // var result = engineApplication.GetSearchEngine(engineName);

            // Assert
           // Assert.That(result, Is.Not.Null);
            //Assert.That(result.Name, Is.EqualTo("Google").IgnoreCase);
        }
    }
}

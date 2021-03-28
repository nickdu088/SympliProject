using FrontEnd.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FrontEnd.UnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test_Index_Url_Keywords_NotNullEmpty_Returns_Result_In_ViewData ()
        {
            var mockLoggerObj = new Mock<ILogger<HomeController>>();

            string html = "{ \"keywords\":\"e-settlements\",\"url\":\"www.sympli.com\",\"result\":\"3,4,\"}";
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(html),
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);
            var httpClient = new HttpClient(handlerMock.Object);

            var homeController = new HomeController(mockLoggerObj.Object, httpClient);
            ViewResult result = (ViewResult)await homeController.Index("e-settlements", "www.sympli.com");

            ////mock httpclient getAsync returns string
            ViewDataDictionary viewData = result.ViewData;
            Assert.AreEqual("3,4,", viewData["result"]);
            Assert.AreEqual("Index", result.ViewName);
        }

        [Test]
        public async Task Test_Index_Url_Keywords_NullEmpty_Returns_Result_In_ViewData()
        {
            var mockLoggerObj = new Mock<ILogger<HomeController>>();
            HttpClient httpClient = new HttpClient();
            var homeController = new HomeController(mockLoggerObj.Object, httpClient);
            ViewResult result = (ViewResult)await homeController.Index("", "");

            ////mock httpclient getAsync returns string
            ViewDataDictionary viewData = result.ViewData;
            Assert.AreEqual("keywords or url cannot be empty!", viewData["result"]);
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}
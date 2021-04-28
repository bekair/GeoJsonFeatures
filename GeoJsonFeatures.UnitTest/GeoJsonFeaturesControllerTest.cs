using GeoJsonFeatures.UnitTest.Constants;
using GeoJsonFeatures.UnitTest.Helpers;
using GeoJsonFeatures.WebAPI.Controllers;
using GeoJsonFeatures.WebAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GeoJsonFeatures.UnitTest
{
    [TestClass]
    public class GeoJsonFeaturesControllerTest
    {
        private readonly GeoJsonFeaturesController _geoJsonFeaturesControllerTest;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

        public GeoJsonFeaturesControllerTest()
        {
            _httpClientFactoryMock = new();
            _configMock = new();
            _httpMessageHandlerMock = new(MockBehavior.Strict);
            _geoJsonFeaturesControllerTest = new GeoJsonFeaturesController(_httpClientFactoryMock.Object, _configMock.Object);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetErrorCaseParameters), DynamicDataSourceType.Property)]
        public async Task GetOsmDataByBboxErrorCaseTest(double minLon, double minLat, double maxLon,
                                                        double maxLat, string expectedErrorMessage, HttpStatusCode httpStatusCode)
        {
            //Arrange
            MockCommonProcess(minLon, minLat, maxLon, maxLat, expectedErrorMessage, httpStatusCode);
            BoundingBox boundingBox = new(minLon, minLat, maxLon, maxLat);

            //Act
            var response = await _geoJsonFeaturesControllerTest.GetOsmDataByBbox(boundingBox);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(expectedErrorMessage, response.Message);
            Assert.AreEqual(httpStatusCode, response.StatusCode);
            Assert.AreEqual(null, response.Result);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetSuccessCaseParameters), DynamicDataSourceType.Property)]
        public async Task GetOsmDataByBboxSuccessCaseTest(double minLon, double minLat, double maxLon,
                                                          double maxLat, string expectedContent, HttpStatusCode httpStatusCode)
        {
            //Arrange
            MockCommonProcess(minLon, minLat, maxLon, maxLat, expectedContent, httpStatusCode);
            BoundingBox boundingBox = new(minLon, minLat, maxLon, maxLat);
            XDocument expectedContentXml = XDocument.Parse(expectedContent);

            //Act
            var response = await _geoJsonFeaturesControllerTest.GetOsmDataByBbox(boundingBox);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(expectedContentXml.ToString(), response.Result.ToString());
        }

        private void MockCommonProcess(double minLon, double minLat, double maxLon, double maxLat,
                                       string expectedContent, HttpStatusCode httpStatusCode)
        {
            string baseAddressUrl = TestConstants.ApiBaseAddressUrl;
            string url = $"{baseAddressUrl}/map?bbox={minLon},{minLat},{maxLon},{maxLat}";
            HttpResponseMessage httpResponseMessage = new()
            {
                StatusCode = httpStatusCode,
                Content = new StringContent(expectedContent)
            };

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri(baseAddressUrl)
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            _httpClientFactoryMock.Setup(x => x.CreateClient(_configMock.Object["MapApiName"]))
                                  .Returns(httpClient);

            _httpMessageHandlerMock.Protected()
                                   .Setup<Task<HttpResponseMessage>>
                                   (
                                        "SendAsync",
                                        ItExpr.IsAny<HttpRequestMessage>(),
                                        ItExpr.IsAny<CancellationToken>()
                                   )
                                   .ReturnsAsync(httpResponseMessage)
                                   .Verifiable();
        }

        public static IEnumerable<object[]> GetErrorCaseParameters
        {
            get
            {
                yield return new object[]
                {
                    10, 10, 10, 120,
                    "The latitudes must be between -90 and 90, longitudes between -180 and 180 and the minima must be less than the maxima.",
                    HttpStatusCode.BadRequest
                };
                yield return new object[]
                {
                    10, 40, 12, 50,
                    "The maximum bbox size is 0.25, and your request was too large. Either request a smaller area, or use planet.osm",
                    HttpStatusCode.BadRequest
                };
                yield return new object[]
                {
                    10, 63, 10.2, 63.7,
                    "You requested too many nodes (limit is 50000). Either request a smaller area, or use planet.osm",
                    HttpStatusCode.BadRequest
                };
            }
        }

        public static IEnumerable<object[]> GetSuccessCaseParameters
        {
            get
            {
                yield return new object[]
                {
                    10, 10, 10, 90,
                    TestHelper.GetStringContentFromFile($"{TestConstants.XmlTestFilesBaseUrl}/{TestConstants.OsmSuccessResponsePath1}"),
                    HttpStatusCode.OK
                };
                yield return new object[]
                {
                    88.01, 33.01, 88.02, 33.02,
                    TestHelper.GetStringContentFromFile($"{TestConstants.XmlTestFilesBaseUrl}/{TestConstants.OsmSuccessResponsePath2}"),
                    HttpStatusCode.OK
                };
            }
        }
    }
}

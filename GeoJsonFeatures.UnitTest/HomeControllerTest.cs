using GeoJsonFeatures.UnitTest.Constants;
using GeoJsonFeatures.UnitTest.Helpers;
using GeoJsonFeatures.Web.Controllers;
using GeoJsonFeatures.Web.Models;
using GeoJsonFeatures.Web.Models.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
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
    public class HomeControllerTest
    {
        private readonly HomeController _homeControllerTest;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

        public HomeControllerTest()
        {
            _httpClientFactoryMock = new();
            _httpMessageHandlerMock = new(MockBehavior.Strict);
            _configMock = new();
            _homeControllerTest = new HomeController(_httpClientFactoryMock.Object, _configMock.Object);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetFeaturesByCoordinatesModelValidErrorTestCaseParameters), DynamicDataSourceType.Property)]
        public async Task SearchGeoJsonFeaturesByCoordinates_Model_Valid_ErrorTestAsync(double minLon, double minLat, double maxLon, double maxLat,
                                                                                        string expectedContent, HttpStatusCode expectedStatusCode)
        {
            //Arrange
            string baseAddressUrl = TestConstants.ApiBaseAddressUrl;
            string url = $"{baseAddressUrl}/map?bbox={minLon},{minLat},{maxLon},{maxLat}";
            HttpResponseMessage httpResponseMessage = new()
            {
                StatusCode = expectedStatusCode,
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

            _homeControllerTest.ControllerContext.HttpContext = new DefaultHttpContext();

            OpenStreetMapViewModel openStreetMapViewModel = new()
            {
                MinimumLongitude = minLon,
                MaximumLatitude = maxLat,
                MinimumLatitude = minLat,
                MaximumLongitude = maxLon
            };

            //Act
            JsonResult result = await _homeControllerTest.SearchGeoJsonFeaturesByCoordinates(openStreetMapViewModel);
            OsmApiResponseModel actualOsmResponseModel = (OsmApiResponseModel)result.Value;
            OsmApiResponseModel expectedOsmResponseModel = JsonConvert.DeserializeObject<OsmApiResponseModel>(expectedContent);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedOsmResponseModel.Message, actualOsmResponseModel.Message);
            Assert.AreEqual(expectedOsmResponseModel.Result, actualOsmResponseModel.Result);
            Assert.AreEqual(expectedOsmResponseModel.StatusCode, actualOsmResponseModel.StatusCode);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetFeaturesByCoordinatesModelInvalidErrorTestCaseParameters), DynamicDataSourceType.Property)]
        public async Task SearchGeoJsonFeaturesByCoordinates_Model_InValid_ErrorTestAsync(double? minLon, double? minLat, double? maxLon, double? maxLat,
                                                                                          string expectedContent, HttpStatusCode expectedStatusCode, string[] modelStateErrorArray)
        {
            //Arrange
            string baseAddressUrl = TestConstants.ApiBaseAddressUrl;
            string url = $"{baseAddressUrl}/map?bbox={minLon},{minLat},{maxLon},{maxLat}";
            HttpResponseMessage httpResponseMessage = new()
            {
                StatusCode = expectedStatusCode,
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

            _homeControllerTest.ControllerContext.HttpContext = new DefaultHttpContext();
            foreach (var modelStateError in modelStateErrorArray)
            {
                _homeControllerTest.ModelState.AddModelError("", modelStateError);
            }

            OpenStreetMapViewModel openStreetMapViewModel = new()
            {
                MinimumLongitude = minLon,
                MaximumLatitude = maxLat,
                MinimumLatitude = minLat,
                MaximumLongitude = maxLon
            };

            //Act
            JsonResult result = await _homeControllerTest.SearchGeoJsonFeaturesByCoordinates(openStreetMapViewModel);
            OsmApiResponseModel actualOsmResponseModel = (OsmApiResponseModel)result.Value;
            string expectedOsmResponseMessage = expectedContent;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedOsmResponseMessage, actualOsmResponseModel.Message);
            Assert.AreEqual(null, actualOsmResponseModel.Result);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetFeaturesByCoordinatesSuccessTestCaseParameters), DynamicDataSourceType.Property)]
        public async Task SearchGeoJsonFeaturesByCoordinates_SuccessTestAsync(double minLon, double minLat, double maxLon, double maxLat,
                                                                              string expectedContent, HttpStatusCode expectedStatusCode)
        {
            //Arrange
            string baseAddressUrl = TestConstants.ApiBaseAddressUrl;
            string url = $"{baseAddressUrl}/map?bbox={minLon},{minLat},{maxLon},{maxLat}";
            HttpResponseMessage httpResponseMessage = new()
            {
                StatusCode = expectedStatusCode,
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

            OpenStreetMapViewModel openStreetMapViewModel = new()
            {
                MinimumLongitude = minLon,
                MaximumLatitude = maxLat,
                MinimumLatitude = minLat,
                MaximumLongitude = maxLon
            };

            //Act
            JsonResult result = await _homeControllerTest.SearchGeoJsonFeaturesByCoordinates(openStreetMapViewModel);
            string actualOsmResponse = result.Value.ToString();
            OsmApiResponseModel expectedOsmResponseModel = JsonConvert.DeserializeObject<OsmApiResponseModel>(expectedContent);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedOsmResponseModel.Result.ToString(), actualOsmResponse);
        }

        public static IEnumerable<object[]> GetFeaturesByCoordinatesModelValidErrorTestCaseParameters
        {
            get
            {
                yield return new object[]
                {
                    10, 40, 12, 50,
                    @"{""result"":null,""statusCode"":400,""message"":""The maximum bbox size is 0.25, and your request was too large. Either request a smaller area, or use planet.osm""}",
                    HttpStatusCode.BadRequest
                };
                yield return new object[]
                {
                    10, 63, 10.2, 63.7,
                    @"{""result"":null,""statusCode"":400,""message"":""You requested too many nodes (limit is 50000). Either request a smaller area, or use planet.osm""}",
                    HttpStatusCode.BadRequest
                };
            }
        }

        public static IEnumerable<object[]> GetFeaturesByCoordinatesSuccessTestCaseParameters
        {
            get
            {
                yield return new object[]
                {
                    88.01, 33.01, 88.02, 33.02,
                    TestHelper.GetStringContentFromFile($"{TestConstants.JsonTestFilesBaseUrl}/{TestConstants.SearchByCoordinateResponsePath}"),
                    HttpStatusCode.OK
                };
            }
        }

        public static IEnumerable<object[]> GetFeaturesByCoordinatesModelInvalidErrorTestCaseParameters
        {
            get
            {
                yield return new object[]
                {
                    null, null, null, null,
                    TestHelper.ConcatMessagesBySeparator("<br/>", NullParameterMessages),
                    HttpStatusCode.BadRequest,
                    NullParameterMessages
                };
                yield return new object[]
                {
                    200D, 200D, 200D, 200D,
                    TestHelper.ConcatMessagesBySeparator("<br/>", RangeExceedParameterMessages),
                    HttpStatusCode.BadRequest,
                    RangeExceedParameterMessages
                };
            }
        }

        public static string[] NullParameterMessages
        {
            get
            {
                return new string[]
                {
                    TestConstants.MinLonRequiredMessage,
                    TestConstants.MinLatRequiredMessage,
                    TestConstants.MaxLonRequiredMessage,
                    TestConstants.MaxLatRequiredMessage
                };
            }
        }

        public static string[] RangeExceedParameterMessages
        {
            get
            {
                return new string[]
                {
                    TestConstants.MinLonRangeExceededMessage,
                    TestConstants.MinLatRangeExceededMessage,
                    TestConstants.MaxLonRangeExceededMessage,
                    TestConstants.MaxLatRangeExceededMessage
                };
            }
        }

    }
}

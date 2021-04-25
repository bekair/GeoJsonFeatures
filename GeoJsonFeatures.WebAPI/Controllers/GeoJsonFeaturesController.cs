using GeoJsonFeatures.WebAPI.Models;
using GeoJsonFeatures.WebAPI.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GeoJsonFeatures.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoJsonFeaturesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public GeoJsonFeaturesController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        [HttpGet("GetOsmDataByBbox")]
        public async Task<ApiResponseModel<XDocument>> GetOsmDataByBbox([FromBody] BoundingBox boundingBox)
        {
            HttpClient client = _httpClientFactory.CreateClient(_config["MapApiName"]);
            string url = $"{client.BaseAddress}map?bbox={boundingBox.MinimumLongitude},{boundingBox.MinimumLatitude},{boundingBox.MaximumLongitude},{boundingBox.MaximumLatitude}";

            using HttpResponseMessage responseMessage = await client.GetAsync(url);

            ContentResult result = Content(await responseMessage.Content.ReadAsStringAsync(), "text/xml");
            ApiResponseModel<XDocument> response = new()
            {
                IsSuccessfull = true,
                StatusCode = responseMessage.StatusCode,
                Result = XDocument.Parse(result.Content),
                ContentType = result.ContentType
            };

            if (!responseMessage.IsSuccessStatusCode)
            {
                response.IsSuccessfull = false;
                response.Message = result.Content;
                response.Result = null;
            }

            return response;
        }
    }
}

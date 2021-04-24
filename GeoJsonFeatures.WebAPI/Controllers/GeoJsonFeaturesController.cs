using GeoJsonFeatures.WebAPI.Models;
using GeoJsonFeatures.WebAPI.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

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
        public async Task<ApiResponseModel<string>> GetOsmDataByBbox([FromBody] BoundingBox boundingBox)
        {
            ApiResponseModel<string> response = new();

            HttpClient client = _httpClientFactory.CreateClient(_config["MapApiName"]);
            string url = $"{client.BaseAddress}map?bbox={boundingBox.MinimumLongitude},{boundingBox.MinimumLatitude},{boundingBox.MaximumLongitude},{boundingBox.MaximumLatitude}";

            using HttpResponseMessage responseMessage = await client.GetAsync(url);

            ContentResult result = Content(await responseMessage.Content.ReadAsStringAsync(), "text/xml");
            response.StatusCode = responseMessage.StatusCode;
            response.Result = result.Content;
            response.ContentType = result.ContentType;
            if (!responseMessage.IsSuccessStatusCode)
            {
                response.IsSuccessfull = false;
                response.Message = response.Result;
                response.Result = null;
            }

            return response;
        }
    }
}

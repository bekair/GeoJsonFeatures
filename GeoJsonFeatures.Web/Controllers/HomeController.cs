using GeoJsonFeatures.Web.Models;
using GeoJsonFeatures.Web.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GeoJsonFeatures.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public HomeController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new OpenStreetMapViewModel());
        }

        [HttpPost]
        public async Task<JsonResult> SearchGeoJsonFeaturesByCoordinates(OpenStreetMapViewModel openStreetMapViewModel)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = _httpClientFactory.CreateClient(_config["MapApiName"]);
                string url = $"{client.BaseAddress}GeoJsonFeatures/GetOsmDataByBbox";
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                    Content = new StringContent(JsonConvert.SerializeObject(openStreetMapViewModel), Encoding.UTF8, "application/json")
                };

                using HttpResponseMessage response = await client.SendAsync(request);
                ContentResult responseResult = Content(await response.Content.ReadAsStringAsync(), "text/xml");

                openStreetMapViewModel.OsmApiResponseModel = JsonConvert.DeserializeObject<OsmApiResponseModel>(responseResult.Content);

                if (openStreetMapViewModel.OsmApiResponseModel.StatusCode != HttpStatusCode.OK)
                {
                    openStreetMapViewModel.ToastrViewModel.ErrorMessage = openStreetMapViewModel.OsmApiResponseModel.Message;
                    HttpContext.Response.StatusCode = (int)openStreetMapViewModel.OsmApiResponseModel.StatusCode;
                }
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                openStreetMapViewModel.ToastrViewModel.ErrorMessage = string.Join("<br/>", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList());
            }

            return Json(openStreetMapViewModel);
        }
    }
}

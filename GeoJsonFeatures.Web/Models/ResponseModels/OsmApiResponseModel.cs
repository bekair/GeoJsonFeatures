using System.Net;

namespace GeoJsonFeatures.Web.Models.ResponseModels
{
    public class OsmApiResponseModel
    {
        public string Result { get; set; }

        public bool IsSuccessfull { get; set; }

        public string Message { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}

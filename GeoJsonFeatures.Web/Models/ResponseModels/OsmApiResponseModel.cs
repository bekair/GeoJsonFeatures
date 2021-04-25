using System.Net;
using System.Xml.Linq;

namespace GeoJsonFeatures.Web.Models.ResponseModels
{
    public class OsmApiResponseModel
    {
        public XDocument Result { get; set; }

        public string Message { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}

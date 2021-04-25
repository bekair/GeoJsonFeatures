using System.Net;
using System.Runtime.Serialization;

namespace GeoJsonFeatures.WebAPI.Models.ResponseModels
{
    [DataContract]
    public class ApiResponseModel<T>
    {
        //For serialization
        public ApiResponseModel()
        {
        }

        [DataMember]
        public T Result { get; set; }

        [DataMember]
        public HttpStatusCode StatusCode { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}

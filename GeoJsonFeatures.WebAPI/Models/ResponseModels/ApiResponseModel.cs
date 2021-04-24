using System.Net;
using System.Runtime.Serialization;

namespace GeoJsonFeatures.WebAPI.Models.ResponseModels
{
    [DataContract]
    public class ApiResponseModel<T>
    {
        public ApiResponseModel()
        {
            IsSuccessfull = true;
        }

        [DataMember]
        public T Result { get; set; }

        [DataMember]
        public HttpStatusCode StatusCode { get; set; }

        [DataMember]
        public string ContentType { get; set; }

        [DataMember]
        public bool IsSuccessfull { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}

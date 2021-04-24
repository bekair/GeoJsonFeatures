using GeoJsonFeatures.Web.Models.ResponseModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GeoJsonFeatures.Web.Models
{
    public class OpenStreetMapViewModel
    {
        public OpenStreetMapViewModel()
        {
            ToastrViewModel = new() { ErrorMessage = "" };
        }

        [Required(ErrorMessage = "{0} field is required.")]
        [Range(-180, 180, ErrorMessage = "{0} field must be between {1} and {2}.")]
        [DisplayName("Min. Longitude (westernmost)")]
        public double? MinimumLongitude { get; set; }

        [Required(ErrorMessage = "{0} field is required.")]
        [Range(-90, 90, ErrorMessage = "{0} field must be between {1} and {2}.")]
        [DisplayName("Min. Latitude (southernmost)")]
        public double? MinimumLatitude { get; set; }

        [Required(ErrorMessage = "{0} field is required.")]
        [Range(-180, 180, ErrorMessage = "{0} field must be between {1} and {2}.")]
        [DisplayName("Max. Longitude (easternmost)")]
        public double? MaximumLongitude { get; set; }

        [Required(ErrorMessage = "{0} field is required.")]
        [Range(-90, 90, ErrorMessage = "{0} field must be between {1} and {2}.")]
        [DisplayName("Max. Longitude (northernmost)")]
        public double? MaximumLatitude { get; set; }

        public ToastrViewModel ToastrViewModel { get; set; }

        public OsmApiResponseModel OsmApiResponseModel { get; set; }
    }
}

namespace GeoJsonFeatures.UnitTest.Constants
{
    public static class TestConstants
    {
        public static readonly string OsmSuccessResponsePath1 = "ApiOsmDataResponse_10,001_12,001_10,002_12,002.xml";
        public static readonly string OsmSuccessResponsePath2 = "ApiOsmDataResponse_88,01_33,01_88,02_33,02.xml";
        public static readonly string ApiBaseAddressUrl = "https://localhost:44305/api";
        public static readonly string XmlTestFilesBaseUrl = "./XmlTestFiles";
        public static readonly string JsonTestFilesBaseUrl = "./JsonTestFiles";
        public static readonly string SearchByCoordinateResponsePath = "SearchByCoordinateResponse_33,01_88,01_33,02_88,02.json";
        public static readonly string MinLonRequiredMessage = "Min. Longitude (westernmost) field is required.";
        public static readonly string MinLatRequiredMessage = "Min. Latitude (southernmost) field is required.";
        public static readonly string MaxLonRequiredMessage = "Max. Longitude (easternmost) field is required.";
        public static readonly string MaxLatRequiredMessage = "Max. Latitude (northernmost) field is required.";
        public static readonly string MinLonRangeExceededMessage = "Min. Longitude (westernmost) field must be between -180 and 180.";
        public static readonly string MinLatRangeExceededMessage = "Min. Latitude (southernmost) field must be between -90 and 90.";
        public static readonly string MaxLonRangeExceededMessage = "Max. Longitude (easternmost) field must be between -180 and 180.";
        public static readonly string MaxLatRangeExceededMessage = "Max. Latitude (northernmost) field must be between -90 and 90.";
    }
}

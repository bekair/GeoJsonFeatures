namespace GeoJsonFeatures.WebAPI.Models
{
    public class BoundingBox
    {
        /// <summary>
        /// The longitude of the left (westernmost) side of the bounding box.
        /// </summary>
        public double MinimumLongitude { get; set; }

        /// <summary>
        /// The latitude of the bottom (southernmost) side of the bounding box.
        /// </summary>
        public double MinimumLatitude { get; set; }

        /// <summary>
        /// The longitude of the right (easternmost) side of the bounding box.
        /// </summary>
        public double MaximumLongitude { get; set; }

        /// <summary>
        /// The latitude of the top (northernmost) side of the bounding box.
        /// </summary>
        public double MaximumLatitude { get; set; }

        public BoundingBox(double minimumLongitude, double minimumLatitude, double maximumLongitude, double maximumLatitude)
        {
            MinimumLongitude = minimumLongitude;
            MinimumLatitude = minimumLatitude;
            MaximumLongitude = maximumLongitude;
            MaximumLatitude = maximumLatitude;
        }
    }
}

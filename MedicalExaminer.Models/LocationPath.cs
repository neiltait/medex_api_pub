namespace MedicalExaminer.Models
{
    public class LocationPath : ILocationPath
    {
        public string NationalLocationId { get; set; }

        public string RegionLocationId { get; set; }

        public string TrustLocationId { get; set; }

        public string SiteLocationId { get; set; }
    }
}

using MedicalExaminer.Models.Enums;
using Newtonsoft.Json;

namespace MedicalExaminer.Models
{
    public class Location : ILocationPath
    {
        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string LocationId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "parentId")]
        public string ParentId { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "type")]
        public LocationType Type { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "national_location_id")]
        public string NationalLocationId { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "region_location_id")]
        public string RegionLocationId { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "trust_location_id")]
        public string TrustLocationId { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "site_location_id")]
        public string SiteLocationId { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "is_me_office")]
        public bool IsMeOffice { get; set; } = false;
    }
}
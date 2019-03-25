namespace MedicalExaminer.Common.Queries.Location
{
    public class LocationRetrievalByIdQuery : IQuery<Models.Location>
    {
        public readonly string LocationId;

        public LocationRetrievalByIdQuery(string locationId)
        {
            LocationId = locationId;
        }
    }
}
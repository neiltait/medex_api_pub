namespace MedicalExaminer.Common.Queries.Location
{
    public class LocationRetrievalByIdQuery : IQuery<Models.Location>
    {
        public readonly string Id;

        public LocationRetrievalByIdQuery(string id)
        {
            Id = id;
        }
    }
}
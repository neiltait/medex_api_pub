namespace MedicalExaminer.Common.Queries.Location
{
    public class LocationRetrivalByIdQuery : IQuery<Models.Location>
    {
        public readonly string Id;

        public LocationRetrivalByIdQuery(string id)
        {
            Id = id;
        }
    }
}

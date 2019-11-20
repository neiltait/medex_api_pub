namespace MedicalExaminer.Common.Queries.Location
{
    /// <summary>
    /// Update examination urgency sort query.
    /// </summary>
    public class UpdateLocationIsMeOfficeQuery : IQuery<Models.Location>
    {
        /// <summary>
        /// Location to update.
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        /// Value of Is Me Office to set.
        /// </summary>
        public bool IsMeOffice { get; set; }
    }
}

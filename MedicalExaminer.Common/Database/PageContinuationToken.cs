namespace MedicalExaminer.Common.Database
{
    public class PageContinuationToken
    {
        public string PaginationToken { get; }
        public int PageNumber { get; }

        public PageContinuationToken(string paginationToken, int pageNumber)
        {
            PaginationToken = paginationToken;
            PageNumber = pageNumber;
        }
    }
}

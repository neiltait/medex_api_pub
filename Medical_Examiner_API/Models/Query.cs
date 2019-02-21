namespace Medical_Examiner_API.Models
{
    public abstract class Query
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }
        
        public Query(int page = 0, int pagesize = 30, string filter = "", string sort = "")
        {
        }
    }
}
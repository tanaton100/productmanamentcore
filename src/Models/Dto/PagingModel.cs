namespace ProductmanagementCore.Models.Dto
{
    public class PagingModel
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int TotalPage { get; set; }
        public int TotalRecord { get; set; }
    }
}

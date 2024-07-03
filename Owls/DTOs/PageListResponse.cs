namespace Owls.DTOs
{
    public class PageListResponse<T>
    {
        public List<T> Data { get; set; }
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages =>
            (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);

    }
}

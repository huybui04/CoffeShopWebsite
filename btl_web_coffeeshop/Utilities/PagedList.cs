namespace btl_web_coffeeshop.Utilities
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public PagedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }

}

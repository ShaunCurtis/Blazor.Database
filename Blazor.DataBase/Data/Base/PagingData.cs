
namespace Blazor.Database.Data
{
    public struct PagingData
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public PagingData(int page, int pageSize)
        {
            this.Page = page;
            this.PageSize = pageSize;
        }
    }
}

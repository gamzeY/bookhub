using System.Collections.Generic;

namespace backend.Dtos;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }

    public PagedResult(IEnumerable<T> items, int page, int pageSize, int totalCount, int totalPages)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = totalPages;
    }
}

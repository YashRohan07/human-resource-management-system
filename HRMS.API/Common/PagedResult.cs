namespace HRMS.API.Common;

// Used for paginated API responses
public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];

    public PaginationMeta Meta { get; set; } = new();
}
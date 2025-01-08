namespace ProfileRepositories.Pagination;

public class PaginationParams
{
    private const int defaultPageSize = 10;
    private const int defaultPageIndex = 1;
    private int _pageSize = defaultPageSize;
    private int _pageIndex = defaultPageIndex;

    public int PageSize 
    { 
        get => _pageSize;
        set => _pageSize = value > 0 ? value : defaultPageSize;
    }

    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = value > 0 ? value : defaultPageIndex;
    }
}

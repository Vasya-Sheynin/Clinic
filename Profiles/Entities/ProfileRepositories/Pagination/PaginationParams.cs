namespace ProfileRepositories.Pagination;

public class PaginationParams
{
    private int _pageSize = 10;
    private int _pageIndex = 1;

    public int PageSize 
    { 
        get
        {
            return _pageSize;
        }
        set 
        { 
            if (value > 0) 
                _pageSize = value;
        } 
    }

    public int PageIndex
    {
        get
        {
            return _pageIndex;
        }
        set
        {
            if (value > 0)
                _pageIndex = value;
        }
    }
}

namespace FloraFauna_GO_Shared;

public class Pagination<T>
{
    public long TotalCount { get; set; }

    public int PageIndex { get; set; }

    public int CountPerPage { get; set; }

    public IEnumerable<T> Items { get; set; } = null!;
}
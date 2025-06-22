namespace FloraFauna_GO_Shared;

public class Pagination<T>
{
    public long Total { get; set; }

    public int Index { get; set; }

    public int Count { get; set; }

    public IEnumerable<T> Items { get; set; } = null!;
}
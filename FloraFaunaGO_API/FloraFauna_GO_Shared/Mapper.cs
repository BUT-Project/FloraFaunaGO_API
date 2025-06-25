namespace FloraFauna_GO_Shared;

public class Mapper<T, U>
    where T : class
    where U : class
{
    private readonly HashSet<Tuple<T, U>> mapper = new();

    public void Reset()
    {
        mapper.Clear();
    }

    public T? GetT(U u)
    {
        var result = mapper.Where(tuple => ReferenceEquals(tuple.Item2, u));
        if (result.Count() != 1) return null;
        return result.First().Item1;
    }

    public U? GetU(T t)
    {
        var result = mapper.Where(tuple => ReferenceEquals(tuple.Item1, t));
        if (result.Count() != 1) return null;
        return result.First().Item2;
    }

    public bool AddMapping(T t, U u)
    {
        var mapping = new Tuple<T, U>(t, u);
        if (mapper.Contains(mapping)) return false;
        mapper.Add(mapping);
        return true;
    }
}
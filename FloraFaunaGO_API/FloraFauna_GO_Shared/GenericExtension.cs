namespace FloraFauna_GO_Shared;

public static class GenericExtension
{
    public static U ToU<T, U>(this T t, Mapper<T, U>? mapper, Func<T, U> creator, Action<T, U>? linker = null) where T : class where U : class
    {
        var result = mapper?.GetU(t);

        if (result != null) return result;

        U u = creator(t);

        mapper?.AddMapping(t, u);

        if (linker != null) linker(t, u);

        return u;
    }

    public static T ToT<T, U>(this U u, Mapper<T, U>? mapper, Func<U, T> creator, Action<U, T>? linker = null) where T : class where U : class
    {
        var result = mapper?.GetT(u);

        if (result != null) return result;

        T t = creator(u);

        mapper?.AddMapping(t, u);

        if (linker != null) linker(u, t);

        return t;
    }
}
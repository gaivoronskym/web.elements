namespace Point.Utils;

public sealed class RsTaskWrap : IRsTaskWrap
{
    private readonly Func<IResponse> func;

    public RsTaskWrap(IResponse res)
        : this(() => res)
    {
    }

    public RsTaskWrap(Func<IResponse> func)
    {
        this.func = func;
    }

    public Task<IResponse> Task()
    {
        return new Task<IResponse>(func);
    }
}
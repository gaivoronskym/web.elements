namespace Point.Rq
{
    public interface IRqAuth : IRequest
    {
        IIdentity Identity();
    }
}

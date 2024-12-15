namespace Point.Rq.Interfaces
{
    public interface IRqAuth : IRequest
    {
        IIdentity Identity();
    }
}

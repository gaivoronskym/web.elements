namespace Point.Rs;

public interface IResponse : IHead, IBody
{
    public sealed class Smart
    {
        private readonly Func<IResponse> func;

        public Smart(IResponse res)
            : this(() => res)
        {
        }
        
        public Smart(Func<IResponse> func)
        {
            this.func = func;
        }

        public Task<IResponse> AsTask()
        {
            return Task.FromResult(func());
        }
    }
}
using Point.Authentication.Rq;
using Point.Fk;
using Point.Pt;
using Point.Rq.Interfaces;

namespace Point.Authentication.Fk
{
    public class FkAuthenticated : IFork
    {
        private readonly IPoint _point;
        private readonly string _header;

        public FkAuthenticated(IPoint point, string header)
        {
            _point = point;
            _header = header;
        }

        public async Task<IOpt<IResponse>> Route(IRequest req)
        {
            var identity = new RqAuth(req, _header).Identity();
            if(identity is not Anonymous)
            {
                var res = await _point.Act(req);
                return new Opt<IResponse>(res);
            }

            return new IOpt<IResponse>.Empty();
        }
    }
}

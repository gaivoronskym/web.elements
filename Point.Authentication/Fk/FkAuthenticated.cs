using Point.Authentication.Interfaces;
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

        public async Task<IResponse?> Route(IRequest req)
        {
            IIdentity identity = new RqAuth(req, _header).Identity();
            if(identity is not Anonymous)
            {
                return await _point.Act(req);
            }

            return default;
        }
    }
}

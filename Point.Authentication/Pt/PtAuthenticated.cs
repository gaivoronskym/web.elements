using Point.Authentication.Interfaces;
using Point.Authentication.Rq;
using Point.Pt;
using Point.Rq.Interfaces;
using Point.Rs;
using System.Net;

namespace Point.Authentication.Pt
{
    public class PtAuthenticated : IPoint
    {
        private readonly IPoint _origin;
        private readonly string _header;

        public PtAuthenticated(IPoint origin, string header)
        {
            _origin = origin;
            _header = header;
        }

        public async Task<IResponse> Act(IRequest req)
        {
            IIdentity identity = new RqAuth(req, _header).Identity();
            if (identity is not Anonymous)
            {
                return await _origin.Act(req);
            }

            return new RsWithStatus(HttpStatusCode.Unauthorized);
        }
    }
}

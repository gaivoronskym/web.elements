using System.Net;
using Point.Exceptions;
using Point.Pt;
using Point.Rq;
using Point.Rs;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Point.Fk
{
    public class FkAuthenticated : IFork
    {
        private readonly IScalar<IPoint> point;
        private readonly string header;

        public FkAuthenticated(IPoint point)
            : this(point, nameof(PtAuth))
        {
        }

        public FkAuthenticated(IPoint point, string header)
            : this(new Live<IPoint>(() => point), header)
        {
        }

        public FkAuthenticated(IScalar<IPoint> point)
            : this(point, nameof(PtAuth))
        {
        }
        
        public FkAuthenticated(IScalar<IPoint> point, string header)
        {
            this.point = point;
            this.header = header;
        }

        public async Task<IOpt<IResponse>> Route(IRequest req)
        {
            var identity = new RqAuth(req, header).Identity();
            if(!string.IsNullOrEmpty(identity.Identifier()))
            {
                var res = await point.Value().Act(req);
                return new Opt<IResponse>(res);
            }

            throw new HttpException(HttpStatusCode.Unauthorized);
        }
    }
}

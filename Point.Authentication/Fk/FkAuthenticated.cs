﻿using System.Net;
using Point.Authentication.Pt;
using Point.Authentication.Rq;
using Point.Fk;
using Point.Pt;
using Point.Rq.Interfaces;
using Point.Rs;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Point.Authentication.Fk
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
            : this(new ScalarOf<IPoint>(() => point), header)
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

            return new Opt<IResponse>(
                new RsWithStatus(
                    HttpStatusCode.Unauthorized
                )
            );
        }
    }
}

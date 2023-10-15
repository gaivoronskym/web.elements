using Point.Authentication.Fk;
using Point.Fk;
using Point.Pt;

namespace Point.Authentication.Pt
{
    public class PtAuthenticated : PtWrap
    {
        public PtAuthenticated(IPoint origin, string header)
            : base(
                    new PtFork(
                        new FkAuthenticated(origin, header)
                    )
                  )
        {

        }
    }
}

using Point.Authentication.Interfaces;
using Point.Authentication.Ps;
using Point.Backend;
using Point.Branch;
using Point.Pt;

namespace CustomServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IPass pass = new PsBearer(
                "Server",
                "https://localhost",
                "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"
            );
            
            await new TcpBackend(
                new PtBranch(
                    new BranchRoute("/auth/login", new PtMethod("POST", new PtLogin())),
                    new BookPoints(pass)
                ),
                5436).StartAsync();
        }
    }
}
using Point.Authentication.Interfaces;
using Point.Authentication.Ps;
using Point.Backend;
using Point.Fork;
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
            
            await new Backend(
                new PtFork(
                    new FkRoute("/auth/login", new PtMethod("POST", new PtLogin())),
                    new BookPoints(pass),
                    new FkRoute("/files/data.txt", new PtFiles("./data.txt"))
                ),
                5436).StartAsync();
        }
    }
}
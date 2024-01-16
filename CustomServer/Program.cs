using Point.Authentication.Codec;
using Point.Authentication.Interfaces;
using Point.Authentication.Ps;
using Point.Authentication.Pt;
using Point.Backend;
using Point.Fk;
using Point.Pt;

namespace CustomServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // IPass pass = new PsBearer(
            //     "Server",
            //     "https://localhost",
            //     "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"
            // );
            IPass pass = new PsCookie(new CcBase64(new CcPlain()), "Identity", 1);

            await new Backend(
                new PtAuth(
                    new PtFork(
                        new FkRoute("/auth/login",
                            new PtMethod(
                                "POST",
                                new PtLogin(
                                    "Server",
                                    "https://localhost",
                                    4460,
                                    "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"
                                )
                            )
                        ),
                        new FkBooks(),
                        new FkRoute("/files/data.txt", new PtFiles("./data.txt"))
                    ),
                    pass,
                    "Authorization"
                ),
                5436).StartAsync();
        }
    }
}
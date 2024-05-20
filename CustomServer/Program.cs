using System.Net;
using CustomServer.Doc;
using CustomServer.Doc.Routes;
using Point.Authentication.Codec;
using Point.Authentication.Interfaces;
using Point.Authentication.Ps;
using Point.Authentication.Pt;
using Point.Backend;
using Point.Fk;
using Point.Pt;
using Point.RestDoc;
using Point.RestDoc.Pt;

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
            // "Server",
            // "https://localhost",
            // 4460,
            // "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"
            //IPass pass = new PsCookie(new CcBase64(new CcPlain()), "Identity", 1);

            // ICodec codec = new CcSafe(
            //     new CcHex(
            //         new CcXor(
            //             new CcPlain(),
            //             "secret-code"
            //         )
            //     )
            // );
            //
            // IPass pass = new PsCookie(
            //     new CcSafe(
            //         new CcHex(
            //             new CcXor(
            //                 new CcPlain(),
            //                 "secret-code"
            //             )
            //         )
            //     ),
            //     "Identity",
            //     1
            // );

            // await new Backend(
            //     new PtAuth(
            //         new PtFork(
            //             new FkRoute(
            //                 "/auth/login",
            //                 new PtMethod(
            //                     "POST",
            //                     new PtLogin(
            //                         codec,
            //                         1
            //                     )
            //                 )
            //             ),
            //             new FkBooks(),
            //             new FkRoute("/files/data.txt", new PtFiles("./data.txt"))
            //         ),
            //         pass,
            //         "Authorization"
            //     ),
            //     5436).StartAsync();

            await new Backend(
                new PtFork(
                    new FkRoute(
                        "/doc/index.html",
                        new PtMethod(
                            "GET",
                            new PtFiles("./index.html")
                        )
                    ),
                    new FkRoute(
                        "/v1/rest-doc.json",
                        new PtMethod(
                            "GET",
                            new PtDoc(
                                new DocSegment(
                                    new BooksRoute()
                                )
                            )
                        )
                    ),
                    new FkRoute(
                        "/books",
                        new PtMethod(
                            "GET",
                            new PtBooks()
                        )
                    ),
                    new FkRoute(
                        "/books",
                        new PtMethod(
                            "POST",
                            new PtPostBook()
                        )
                    )
                ),
                5436
            ).StartAsync();
        }
    }
}
using System.Security.Cryptography;
using Point.Authentication;
using Point.Authentication.Ps;
using Point.Authentication.Pt;
using Point.Backend;
using Point.Fk;
using Point.Pt;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Text;

namespace CustomServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // IPass pass = new
            // (
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

            // new FkRoute(
            //     "/doc/index.html",
            //     new PtMethod(
            //         "GET",
            //         new PtFiles("./index.html")
            //     )
            // ),
            // new FkRoute(
            //     "/v1/rest-doc.json",
            //     new PtMethod(
            //         "GET",
            //         new PtDoc(
            //             new DocSegment(
            //                 new BooksRoute()
            //             )
            //         )
            //     )
            // ),

            await new Back(
                new PtAuth(
                    new PtFork(
                        new FkRegex(
                            "auth/login",
                            new PtLogin(
                                new TokenFactory(
                                    "Server",
                                    "https://localhost",
                                    4460,
                                    new HMACSHA256(
                                        new BytesOf(
                                            new TextOf(
                                                "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"
                                            )
                                        ).AsBytes()
                                    )
                                )
                            )
                        ),
                        new FkRegex(
                            "/api/items/(?<id>\\d+)",
                            new PtAuthenticated(
                                new PtMethod(
                                    "GET",
                                    new PtBookPages()
                                )
                            )
                        )
                        // new FkRoute(
                        //     "/books/{bookId:\\d+}/pages",
                        //     new PtMethod(
                        //         "GET",
                        //         new PtBookPages()
                        //     )
                        // ),
                        // new FkRoute(
                        //     "/books",
                        //     new PtMethod(
                        //         "POST",
                        //         new PtPostBook()
                        //     )
                        // ),
                        // new FkRoute("/files/data.txt", new PtFiles("./data.txt"))
                    ),
                    new PsBearer(
                        new HMACSHA256(
                            new BytesOf(
                                new TextOf(
                                    "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"
                                )
                            ).AsBytes()
                        )
                    ),
                    "Authorization"
                ),
                5000
            ).StartAsync();
        }
    }
}
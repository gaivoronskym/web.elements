﻿using Point.Authentication.Interfaces;
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
            
            await new Backend(
                new PtBranch(
                    new BranchRoute("/auth/login", new PtMethod("POST", new PtLogin())),
                    new BookPoints(pass),
                    new BranchRoute("/files/data.txt", new PtFiles("./data.txt"))
                ),
                5436).StartAsync();
        }
    }
}
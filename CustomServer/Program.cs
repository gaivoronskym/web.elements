using Point;

namespace CustomServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var httpServer = new HttpServer(5436);

            httpServer.Start();

            while (true)
            {
                
            }
        }
    }
}
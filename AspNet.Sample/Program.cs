namespace AspNet.Sample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello, world!");
            });
        }
    }
}
using Point.Razor;

namespace CustomServer.Layouts;

public class MainLayout : LayoutView
{
    public MainLayout(IRazorView origin) : base(origin, "MainLayout")
    {
    }
}
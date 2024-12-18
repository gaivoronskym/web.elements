namespace Web.Elements.Rq;

public interface IRqRequestLine : IRequest
{
    string Header();

    string Method();

    string Uri();

    string Version();
}
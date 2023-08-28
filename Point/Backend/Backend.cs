using System.Net;
using Point.Pt;
using Point.Rq;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Func;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;
using JoinedText = Yaapii.Atoms.Collection.Joined<string>;

namespace Point.Backend;

public class HttpServer
{
    private readonly HttpListener _listener;

    public HttpServer(int port)
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add("http://127.0.0.1:" + port + "/");
    }

    public void Start()
    {
        
        _listener.Start();
        Receive();
    }

    public void Stop()
    {
        _listener.Stop();
    }

    private void Receive()
    {
        _listener.BeginGetContext(ListenerCallback, _listener);
    }

    private void ListenerCallback(IAsyncResult result)
    {
        if (_listener.IsListening)
        {
            var context = _listener.EndGetContext(result);
            var request = context.Request;
            
            Console.WriteLine($"{request.Url}");

            IPoint ptText = new PtWithHeader(
                new PtTest(),
                "UtcOffSet",
                "-4"
            );

            var res = ptText.Act(new RequestOf(
                    new JoinedText(
                        request.Headers.Cast<string>().Select(x => x).ToList(),
                        new ListOf<string>($"Uri: {request.Url}")
                        ),
                    new InputStreamOf(string.Empty)
                )
            );

            var statusHead = new ItemAt<string>(
                new Yaapii.Atoms.Enumerable.Filtered<string>(
                    (item) => new StartsWith(
                        new TextOf(item),
                        "HTTP/").Value(),
                    res.Head()
                )
            ).Value();
            
            var response = context.Response;
            response.StatusCode = int.Parse(statusHead.Split(" ")[1]);
            // response.ContentType = "text/plain";

            new Each<string>(
                (item) => response.Headers.Add(item),
                new Yaapii.Atoms.Enumerable.Filtered<string>(
                    (item) => new Not(
                        new StartsWith(
                            new TextOf(item),
                            "HTTP/")
                    ).Value(),
                    res.Head()
                )
            ).Invoke();

            response.OutputStream.Write(new BytesOf(
                    new InputOf(res.Body)
                ).AsBytes()
            );
            response.OutputStream.Close();
            
            Receive();
        }
    }
}
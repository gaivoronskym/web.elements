using System.Diagnostics;
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

public class Backend : IBackend
{
    private readonly HttpListener _listener;
    private readonly IPoint _point;

    public Backend(IPoint point, int port)
    {
        _point = point;
        _listener = new HttpListener();
        _listener.Prefixes.Add("http://127.0.0.1:" + port + "/");
    }

    public int Start()
    {
        try
        {

            var waitEvent = new ManualResetEvent(false);

            AppDomain.CurrentDomain.ProcessExit += (_, __) => { waitEvent.Set(); };

            _listener.Start();
            Receive();

            try
            {
                waitEvent.WaitOne();
            }
            finally
            {
                Stop();
            }

            return 0;
        }
        catch (Exception e)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }

            Console.WriteLine(e);

            return -1;
        }

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
        var context = _listener.EndGetContext(result);
        
        try
        {
            
            if (_listener.IsListening)
            {
                var request = context.Request;

                var res = _point.Act(AddHeaders(request));

                var statusHead = new ItemAt<string>(
                    new Filtered<string>(
                        (item) => new StartsWith(
                            new TextOf(item),
                            "HTTP/").Value(),
                        res.Head()
                    )
                ).Value();

                context.Response.StatusCode = int.Parse(statusHead.Split(" ")[1]);

                new Each<string>(
                    (item) => context.Response.Headers.Add(item),
                    new Filtered<string>(
                        (item) => new Not(
                            new StartsWith(
                                new TextOf(item),
                                "HTTP/")
                        ).Value(),
                        res.Head()
                    )
                ).Invoke();

                context.Response.OutputStream.Write(new BytesOf(
                        new InputOf(res.Body)
                    ).AsBytes()
                );
                context.Response.OutputStream.Close();

                Receive();
            }
        }
        catch (HttpRequestException e)
        {
            context.Response.StatusCode = (int)e.StatusCode!;
            context.Response.OutputStream.Write(new BytesOf(
                    new TextOf(e.Message)
                ).AsBytes()
            );
            
            context.Response.OutputStream.Close();
            
            Receive();
        }
    }

    private IRequest AddHeaders(HttpListenerRequest httpRequest)
    {
        return new RequestOf(
            new JoinedText(
                httpRequest.Headers.Cast<string>().Select(x => x).ToList(),
                new ListOf<string>(httpRequest.Url!.ToString())
            ),
            new InputStreamOf(string.Empty)
        );
    }
}
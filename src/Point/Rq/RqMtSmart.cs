using System.Net;
using Point.Exceptions;
using Point.Rq.Interfaces;
using Yaapii.Atoms.Enumerable;

namespace Point.Rq;

public sealed class RqMtSmart : IRqMultipart
{
    private readonly IRqMultipart origin;

    public RqMtSmart(IRequest origin)
        : this(new RqMtBase(origin))
    {
    }

    public RqMtSmart(IRqMultipart origin)
    {
        this.origin = origin;
    }

    public IRequest Single(string name)
    {
        IEnumerable<IRequest> parts = this.Part(name).ToList();

        if (!parts.Any())
        {
            throw new HttpException(
                HttpStatusCode.BadRequest,
                $"form param \"{name}\" is mandatory."
            );
        }
        
        return new ItemAt<IRequest>(parts).Value();
    }

    public IEnumerable<string> Head()
    {
        return this.origin.Head();
    }

    public Stream Body()
    {
        return this.origin.Body();
    }

    public IEnumerable<IRequest> Part(string name)
    {
        return this.origin.Part(name);
    }

    public IEnumerable<string> Names()
    {
        return this.origin.Names();
    }
}
﻿using Point.Rq.Interfaces;

namespace Point.Rq;

public sealed class RequestOf : IRequest
{
    private readonly IHead _head;
    private readonly IBody _body;

    public RequestOf(Func<IEnumerable<string>> headFunc, Func<Stream> bodyFunc)
        : this(headFunc(), bodyFunc())
    {

    }

    public RequestOf(IHead head, IBody body)
    {
        _head = head;
        _body = body;
    }

    public RequestOf(IEnumerable<string> head, Stream body)
        : this(new HeadOf(head), new BodyOf(body))
    {
        
    }
    
    public IEnumerable<string> Head()
    {
        return _head.Head();
    }

    public Stream Body()
    {
        return _body.Body();
    }
}
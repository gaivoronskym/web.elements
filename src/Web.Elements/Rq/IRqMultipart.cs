﻿namespace Web.Elements.Rq;

public interface IRqMultipart : IRequest
{
    IEnumerable<IRequest> Part(string name);
    
    IEnumerable<string> Names();
}
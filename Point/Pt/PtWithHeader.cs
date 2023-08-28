﻿using Point.Rs;

namespace Point.Pt;

public class PtWithHeader : PtWrap
{
    private readonly string _header;
    
    public PtWithHeader(IPoint origin, string header)
        : base(origin)
    {
        _header = header;
    }

    public PtWithHeader(IPoint origin, string name, string value)
        : base(origin)
    {
        _header = $"{name}: {value}";
    }

    public override IResponse Act(IRequest req)
    {
        return new RsWithHeader(Origin.Act(req), _header);
    }
}
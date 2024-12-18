using System.Text.RegularExpressions;

namespace Web.Elements.Rq;

public interface IRqRegex : IRequest
{
    Match Match();
}
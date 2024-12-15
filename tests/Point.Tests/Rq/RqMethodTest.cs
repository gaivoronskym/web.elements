using Point.Rq;
using Yaapii.Atoms.Enumerable;

namespace Point.Tests.Rq;

public class RqMethodTest
{
    [Fact]
    public void ReturnsMethodPost()
    {
        foreach (var method in new ManyOf<string>("POST", "GET", "PUT", "PATCH"))
        {
            Assert.Equal(
                actual: new RqMethod(
                    new RqFake(method)
                ).Method(),
                expected: method
            );   
        }
    }
}
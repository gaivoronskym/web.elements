using Point.Fk;

namespace Point.Tests;

public class MediaTypeTest
{
    [Fact]
    public void MatchesToTypes()
    {
        Assert.True(new MediaType("*/*").Matches(new MediaType("application/pdf")));
        Assert.True(new MediaType("application/xml").Matches(new MediaType("*/*")));
        Assert.True(new MediaType("text/html").Matches(new MediaType("text/*")));
        Assert.True(new MediaType("image/*").Matches(new MediaType("image/png")));
        Assert.False(new MediaType("application/json").Matches(new MediaType("text")));
    }

    [Fact]
    public void CompareTwoTypes()
    {
        Assert.NotEqual(0, new MediaType("text/b").CompareTo(new MediaType("text/a")));
    }
}
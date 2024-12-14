namespace Point.Tests;

public class MediaTypesTest
{
    [Fact]
    public void MatchesTwoTypes()
    {
        Assert.True(new MediaTypes("*/*").Contains(new MediaTypes("application/xml")));
        Assert.True(new MediaTypes("application/pdf").Contains(new MediaTypes("application/*")));
        Assert.True(new MediaTypes("text/html;q=0.2,*/*").Contains(new MediaTypes("text/plain")));
        Assert.False(new MediaTypes("text/html;q=1.0,text/json").Contains(new MediaTypes("text/p")));
        Assert.False(new MediaTypes("text/*;q=1.0").Contains(new MediaTypes("application/x-file")));
    }

    [Fact]
    public void MatchesTwoCompositeTypes()
    {
        Assert.True(new MediaTypes("text/xml,text/json").Contains(new MediaTypes("text/json")));
        Assert.True(new MediaTypes("text/x-json").Contains(new MediaTypes("text/plain,text/x-json")));
    }
}
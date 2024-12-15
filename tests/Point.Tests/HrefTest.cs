namespace Point.Tests;

public class HrefTest
{
    [Fact]
    public void BuildsUri()
    {
        var res = new Href("https://example.com?id=1&name=User")
            .Without("name")
            .With("age", "30")
            .ToString();
        
        Assert.Equal("https://example.com/?id=1&age=30", res);
    }

    [Fact]
    public void BuildsUriFromEmpty()
    {
        var res = new Href()
            .Path("items")
            .With("offset", "10")
            .With("limit", "10")
            .ToString();
        
        Assert.Equal("about:blank/items?offset=10&limit=10", res);
    }

    [Fact]
    public void ExtractsQueryParams()
    {
        var res = new Href("https://example.com?id=1&name=User")
            .Param("name")[0];
        
        Assert.Equal("User", res);
    }
    
    [Fact]
    public void ExtractsManyQueryParams()
    {
        var res = new Href("https://example.com?ids=1&ids=2&ids=3&ids=4&ids=5")
            .Param("ids");
        
        Assert.Equal(5, res.Count);
    }
}
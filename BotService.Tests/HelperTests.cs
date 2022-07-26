namespace BotService.Tests;

public class HelperTests
{
    private Helper _helper;

    public HelperTests()
    {
        _helper = new Helper();
    }

    [Theory]
    [InlineData("/stock=AAPL.US", "https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv")]
    [InlineData("/stock=ACB.US", "https://stooq.com/q/l/?s=acb.us&f=sd2t2ohlcv&h&e=csv")]
    [InlineData("/stock=AIM.US", "https://stooq.com/q/l/?s=aim.us&f=sd2t2ohlcv&h&e=csv")]
    [InlineData("/stock=AMBO.US", "https://stooq.com/q/l/?s=ambo.us&f=sd2t2ohlcv&h&e=csv")]
    public void GenerateUrl_ShouldReturnUrl(string input, string expectedUrl)
    {
        var expected = new Uri(expectedUrl);

        var actual = _helper.GenerateUrl(input);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("/stock=AAPL.US", "aapl.us")]
    [InlineData("/stOck=AapL.US", "aapl.us")]
    [InlineData("/STOCK=U.US", "u.us")]
    [InlineData("/stock=AAP\nL.US", "aap\nl.us")]
    [InlineData("/stock=", "")]
    [InlineData("/stock=AAPL.U S", "aapl.u s")]
    [InlineData("/stock=AA!#!PL.US", "aa!#!pl.us")]
    [InlineData("/st", "")]
    [InlineData("asdasdasd", "")]
    [InlineData("\n", "")]
    [InlineData("\"", "")]
    [InlineData("AAPL.US", "")]
    [InlineData("", "")]
    public void GetInputStockCode_ShouldReturnStockCode(string input, string expected)
    {
        var helper = new Helper();

        var actual = _helper.GetInputStockCode(input);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ParseResponse_ShouldReturnListOfValues()
    {
        string apiResponse = "Symbol,Date,Time,Open,High,Low,Close,Volume\r\nACB.US,2022-07-25,17:38:33,1.41,1.42,1.35,1.41,4221454\r\n";

        List<string> expected = new List<string> {
            "ACB.US", "2022-07-25", "17:38:33", "1.41", "1.42", "1.35", "1.41", "4221454"
        };

        var actual = _helper.ParseResponse(apiResponse);

        Assert.True(actual.Count == 8);

        for (var i = 0; i < 8; i++)
        {
            Assert.Equal(expected[i], actual[i]);
        }
    }

    [Fact]
    public void CreateMessage_ShouldReturnMessage()
    {
        var expected1 = "ACB.US quote is $1.41 per share";
        var expected2 = " quote is $ per share";
        var expected3 = " quote is $ per share";
        var expected4 = "Symbol quote is $Close per share";

        var model1 = new ResponseModel
        {
            Symbol = "ACB.US",
            Date = "2022-07-25",
            Time = "17:38:33",
            Open = "1.41",
            High = "1.42",
            Low = "1.35",
            Close = "1.41",
            Volume = "4221454"
        };
        var model2 = new ResponseModel
        {
            Symbol = "",
            Date = "",
            Time = "",
            Open = "",
            High = "",
            Low = "",
            Close = "",
            Volume = ""
        };
        var model3 = new ResponseModel
        {
            Symbol = null,
            Date = null,
            Time = null,
            Open = null,
            High = null,
            Low = null,
            Close = null,
            Volume = null
        };
        var model4 = new ResponseModel
        {
            Symbol = "Symbol",
            Date = null,
            Time = null,
            Open = null,
            High = null,
            Low = null,
            Close = "Close",
            Volume = null
        };

        var actual1 = _helper.CreateMessage(model1);
        var actual2 = _helper.CreateMessage(model2);
        var actual3 = _helper.CreateMessage(model3);
        var actual4 = _helper.CreateMessage(model4);

        Assert.Equal(expected1, actual1);
        Assert.Equal(expected2, actual2);
        Assert.Equal(expected3, actual3);
        Assert.Equal(expected4, actual4);
    }
}

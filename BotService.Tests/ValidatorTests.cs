namespace BotService.Tests;

public class ValidatorTests
{
    private Validator _validator = new Validator();

    [Theory]
    [InlineData("/stock=", "/stock=AAPL.US", true)]
    [InlineData("/stock=", "/stock=VHAQ-R.US", true)]
    [InlineData("/stock=", "/stock=YCBD_A.US", true)]
    [InlineData("/stock=", "/stock=asdsad", true)]
    [InlineData("/stock=", "/stock=!#!", true)]
    [InlineData("/stock=", "/stock=asd[*][*", true)]

    [InlineData("/stock=", "/stock=", false)]
    [InlineData("/stock=", "stock=AAPL.US", false)]
    [InlineData("/stock=", "VHAQ-R.US", false)]
    [InlineData("/stock=", "asd[*][*", false)]
    [InlineData("/stock=", "/stock AAPL.US", false)]
    public void IsValidInput_ShouldValidateCommand(string command, string input, bool expected)
    {
        var actual = _validator.IsValidInput(command, input);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("AAPL.US", true)]
    [InlineData("ACB.US", true)]
    [InlineData("VHAQ-R.US", true)]
    [InlineData("YCBD_A.US", true)]
    [InlineData("AAsadasdPL.UasdasdasS", true)]
    [InlineData("AapL.us", true)]

    [InlineData("AAa.sd.asdPL.Ua.sdS", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("AAPL.US!!#", false)]
    [InlineData("=2?AAPL.US", false)]
    [InlineData("=AAPL.US", false)]
    public void IsValidStockCode_ShouldValidateOnlyLettersOrDot(string stockCode, bool expected)
    {
        var actual = _validator.IsValidStockCode(stockCode);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("Symbol,Date,Time,Open,High,Low,Close,Volume\r\nACB.US,2022-07-25,17:38:33,1.41,1.42,1.35,1.41,4221454\r\n", true)]
    [InlineData("Symbol,Date,Time,Open,High,Low,Close,Volume\r\nPCG_C.US,2022-07-22,16:02:24,18.25,18.25,18.25,18.25,120\r\n", true)]

    [InlineData("Symbol,Date,Time,Open,High,Low,Close,VolumeACB.US,2022-7-25,17:38:33,1.41,1.42,1.35,1.41,4221454\r\n", false)]
    [InlineData("Symbol,Date,Time,Open,High,Low,Close,Volume\r\nACB.US,2022-07-25,17,1.a41,1.42,1.35,1.41,4221454\r\n", false)]
    [InlineData("Symbol,Date,Time,Open,High,Low,Close,Volume\r\nACB.US,2022-07-25,17:31.41,N/A,1.35,1.41,4221454\r\n", false)]
    [InlineData("Symbol,Date,Time,Open,High,Low,Close,Volume\r\nACB.US,2022-07-25,17:38:33,1.41,1.42,1.35,1.41,422x1454\r\n", false)]
    [InlineData("Symbol,Date,Time,Open,High,Low,Close,Volume\r\nAASDASDASDCB.US,N/D,N/D,N/D,N/D,N/D,N/D,N/D\r\n", false)]
    public void IsValidApiResponse_ShouldValidateResponse(string response, bool expected)
    {
        var actual = _validator.IsValidApiResponse(response);

        Assert.Equal(expected, actual);
    }
}

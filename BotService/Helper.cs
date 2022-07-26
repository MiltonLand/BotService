namespace BotService;

internal class Helper
{
    internal Uri GenerateUrl(string input)
    {
        var validator = new Validator();

        if (!validator.IsValidInput("/stock=", input))
        {
            throw new InvalidDataException("Invalid input");
        }

        var stockCode = GetInputStockCode(input);

        if (!validator.IsValidStockCode(stockCode))
        {
            throw new InvalidDataException("Invalid Stock Code");
        }

        var url = new Uri($"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv");

        return url;
    }

    internal string GetInputStockCode(string input)
    {
        input = input.ToLower();

        if (!input.StartsWith("/stock="))
        {
            return string.Empty;
        }

        var code = input.Split("/stock=").Last();

        return code;
    }

    internal List<string> ParseResponse(string content)
    {
        var validator = new Validator();

        if (!validator.IsValidApiResponse(content))
        {
            throw new InvalidDataException("Invalid API response");
        }

        var list = content.Split("\r\n");

        var values = list[1].Split(',').ToList();

        return values;
    }

    internal ResponseModel CreateResponseModel(List<string> values)
    {
        var model = new ResponseModel
        {
            Symbol = values.ElementAt(0),
            Date = values.ElementAt(1),
            Time = values.ElementAt(2),
            Open = values.ElementAt(3),
            High = values.ElementAt(4),
            Low = values.ElementAt(5),
            Close = values.ElementAt(6),
            Volume = values.ElementAt(7)
        };

        return model;
    }

    internal string CreateMessage(ResponseModel responseModel)
    {
        var stockCode = responseModel.Symbol;
        var stockPrice = responseModel.Close;

        return $"{stockCode} quote is ${stockPrice} per share";
    }
}

using System.Text.RegularExpressions;

namespace BotService;

internal class Validator
{
    public bool IsValidInput(string command, string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        bool startsWithStock = input.ToLower().StartsWith(command);
        bool hasArgument = input.Length > command.Length;

        return startsWithStock && hasArgument;
    }

    public bool IsValidStockCode(string stockCode)
    {
        if (string.IsNullOrEmpty(stockCode))
        {
            return false;
        }

        bool hasOnlyLettersAndOneDot = Regex.IsMatch(stockCode, @"^[A-Za-z-_]+\.+[A-Za-z-_]+$");

        return hasOnlyLettersAndOneDot;
    }

    public bool IsValidApiResponse(string response)
    {
        if (string.IsNullOrEmpty(response))
        {
            return false;
        }

        var header = @"Symbol,Date,Time,Open,High,Low,Close,Volume";
        var lineBreak = @"\r\n";
        var stockCode = @"[A-Za-z-_]+\.+[A-Za-z-_]";
        var date = @"\d{4}-\d{2}-\d{2}";
        var time = @"\d{2}:\d{2}:\d{2}";
        var decimalNumber = @"[\d\.?]";
        var formmatedNumber = @",+" + decimalNumber + "+";

        var values = stockCode + "+," + date + "," + time + 
            formmatedNumber + formmatedNumber + formmatedNumber + 
            formmatedNumber + formmatedNumber;

        var regExpression = @"^" + header + lineBreak + "+" + values + lineBreak + "+$";
        var isMatch = Regex.IsMatch(response, regExpression);

        return isMatch;
    }
}

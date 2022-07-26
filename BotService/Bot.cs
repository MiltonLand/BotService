using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("BotService.Tests")]

namespace BotService;

public class Bot : IBot
{
    private readonly string _rabbitMQConnection;
    private readonly string _queueName;

    public Bot(string rabbitMQConnection, string queueName)
    {
        _rabbitMQConnection = rabbitMQConnection;
        _queueName = queueName;
    }

    public async Task Send(string input)
    {
        var message = await GetResponseMessage(input);

        SendResponse(message);
    }

    internal async Task<string> GetResponseMessage(string input)
    {
        var helper = new Helper();

        var url = helper.GenerateUrl(input);

        var response = await CallAPI(url);

        var responseValueList = helper.ParseResponse(response);

        ResponseModel responseModel = helper.CreateResponseModel(responseValueList);

        var message = helper.CreateMessage(responseModel);

        return message;
    }

    internal void SendResponse(string text)
    {
        var factory = new ConnectionFactory { Uri = new Uri(_rabbitMQConnection) };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();
        channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var message = new
        {
            TimeStamp = DateTime.Now,
            Username = "Bot",
            Text = text
        };

        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

        channel.BasicPublish("", _queueName, null, body);
    }

    internal async Task<string> CallAPI(Uri url)
    {
        using var httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await httpClient.SendAsync(request);

        using var reader = new StreamReader(response.Content.ReadAsStream());
        var responseBody = reader.ReadToEnd();

        return responseBody;
    }
}
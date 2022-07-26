namespace BotService
{
    public interface IBot
    {
        Task Send(string input);
    }
}
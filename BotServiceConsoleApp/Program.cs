using BotService;

var bot = new Bot("amqp://guest:guest@localhost:5672", "financial-chat-queue");

await bot.Send("/stock=U.US");
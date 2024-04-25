namespace InstagramSearcherBot
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            const string token = "6665847358:AAEVEcOuoZwyEKYfXrl6cIK_ABbSjDy7fPg";
            TelegramBotHandler handler = new TelegramBotHandler(token);

            try
            {
               await handler.BotHandler();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

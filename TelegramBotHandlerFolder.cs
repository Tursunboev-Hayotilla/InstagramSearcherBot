
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace InstagramSearcherBot
{
    public class TelegramBotHandler
    {
        public string Token { get; set; }
        bool Subscribe = false;
        public TelegramBotHandler(string token)
        {
            Token = token;
        }

        public async Task BotHandler()
        {
            var botClient = new TelegramBotClient($"{Token}");
            using CancellationTokenSource cts = new CancellationTokenSource();

            ReceiverOptions receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            botClient.StartReceiving(
                 updateHandler: HandleUpdateAsync,
                 pollingErrorHandler: HandlePollingErrorAsync,
                 receiverOptions: receiverOptions,
                 cancellationToken: cts.Token
             );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            cts.Cancel();
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var id = update.Message.From.Id;
            var chatId = update.Message.Chat.Id;
            var getchatmember = await botClient.GetChatMemberAsync("@Ieltszoneuzswluchat", id);

            var message = update.Message;
            Console.WriteLine(id);

            if (message.Text == "/start")
            {
                ChatMember membership = await botClient.GetChatMemberAsync("@Ieltszoneuzswluchat", userId: message.Chat.Id);

                if (membership != null && membership.Status != ChatMemberStatus.Member && membership.Status != ChatMemberStatus.Administrator && membership.Status != ChatMemberStatus.Creator)
                {
                    await UserIsSubscriber(botClient, id, cancellationToken);
                }
                else
                {
                    Subscribe = true;
                    await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Send me instagram link",
                        cancellationToken: cancellationToken);
                }
            }
            else if (Subscribe)
            {
                string replasemessage = update.Message.Text.Replace("www.", "dd");
                if (update.Message.Text.StartsWith("https://www.instagram.com"))
                {
                    await botClient.SendPhotoAsync(
                   chatId: chatId,
                   photo: $"{replasemessage}",
                   cancellationToken: cancellationToken);
                }
            }


        }
        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        async Task UserIsSubscriber(ITelegramBotClient botClient, long id, CancellationToken cancellationToken)
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
                  {
                    new []
                    {
                        InlineKeyboardButton.WithUrl(text: "Canale 1", url: "https://t.me/Ieltszoneuzswluchat"),
                        

                    },
                }) ;

            await botClient.SendTextMessageAsync(
            chatId: id,
            text: "Follow before starting using bot",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
        }
    }
}
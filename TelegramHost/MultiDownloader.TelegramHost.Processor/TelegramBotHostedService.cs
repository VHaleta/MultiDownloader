using Microsoft.Extensions.Hosting;
using MultiDownloader.TelegramHost.TgBotProcessor.Services;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MultiDownloader.TelegramHost.Processor
{
    public class TelegramBotHostedService : IHostedService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger _logger;
        private CancellationTokenSource _cts;
        private readonly UserService _userService;

        public TelegramBotHostedService(ITelegramBotClient botClient, ILogger logger, UserService userService)
        {
            _botClient = botClient;
            _logger = logger;
            _userService = userService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = new CancellationTokenSource();

            ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };
            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                _cts.Token
            );

            _logger.Information("Telegram Bot started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            _logger.Information("Telegram Bot stopped");
            return Task.CompletedTask;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Models.User user = _userService.RetrieveUserFromUpdate(update);
            await _userService.EnsureUserExistsAndIsUpdatedAsync(user);

            Task handle;
            switch (update.Type)
            {
                case UpdateType.Message:
                    _logger.Information($"Reseived update {update.Type}: {update.Message.Text} from {update.Message.Chat.Id}");
                    handle = HandleMessageAsync(botClient, update, user);
                    break;
                default:
                    _logger.Information($"Reseived unhandled update {update.Type} from {update.Message.Chat.Id}");
                    return;
            }

            await handle;
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}] {apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.Error("Error occurred: {ErrorMessage}", errorMessage);
            return Task.CompletedTask;
        }

        private async Task HandleMessageAsync(ITelegramBotClient botClient, Update update, Models.User user)
        {
            await botClient.SendMessage(user.ChatId, "1");
        }
    }
}

using Microsoft.Extensions.Hosting;
using MultiDownloader.TelegramHost.TgBotProcessor.Repositories;
using Serilog;
using System.Text.Json;
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
        private readonly IUserRepository _userRepository;
        private CancellationTokenSource _cts;

        public TelegramBotHostedService(ITelegramBotClient botClient, ILogger logger, IUserRepository userRepository)
        {
            _botClient = botClient;
            _logger = logger;
            _userRepository = userRepository;
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
            if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            _logger.Information("Received a message from {ChatId}: {MessageText}", chatId, messageText);

            string responce = JsonSerializer.Serialize(await _userRepository.GetAllUsersAsync());
            
            await botClient.SendMessage(chatId, responce, cancellationToken: cancellationToken);
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
    }
}

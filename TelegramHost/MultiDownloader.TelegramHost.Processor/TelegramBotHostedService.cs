using Microsoft.Extensions.Hosting;
using MultiDownloader.TelegramHost.Models.Extensions;
using MultiDownloader.TelegramHost.TgBotProcessor.Handlers;
using MultiDownloader.TelegramHost.TgBotProcessor.Services;
using Serilog;
using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MultiDownloader.TelegramHost.Processor
{
    public class TelegramBotHostedService :
        CallbackQueryHandler,
        IHostedService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger _logger;
        private readonly UserService _userService;
        private readonly DownloaderService _downloaderService;
        private readonly TgMessageService _tgMessageService;

        public TelegramBotHostedService(
            ITelegramBotClient botClient,
            ILogger logger,
            UserService userService,
            DownloaderService downloaderService,
            TgMessageService tgMessageService)
            : base(downloaderService, logger)
        {
            _botClient = botClient;
            _logger = logger;
            _userService = userService;
            _downloaderService = downloaderService;
            _tgMessageService = tgMessageService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };
            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );

            _logger.Information("Telegram Bot started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
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
                    _logger.Information($"Reseived update {update.Type}: {update.Message.Text} from {user.ChatId}");
                    handle = HandleMessageAsync(botClient, update, user);
                    break;
                case UpdateType.CallbackQuery:
                    _logger.Information($"Reseived update {update.Type}: {update.CallbackQuery.Data} from {user.ChatId}");
                    handle = HandleCallbackQueryAsync(botClient, update, user);
                    break;
                default:
                    _logger.Information($"Reseived unhandled update {update.Type} from {user.ChatId}");
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
            if(Uri.TryCreate(update.Message.Text, UriKind.RelativeOrAbsolute, out Uri? uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                //var clearUri = uriResult.ClearUri();
                var formats = await _downloaderService.GetAvailableFormats(uriResult);
                await _tgMessageService.SendFormatsMessage(botClient, user, uriResult, formats);
            }
        }
    }
}

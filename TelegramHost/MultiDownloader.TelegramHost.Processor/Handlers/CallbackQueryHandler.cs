using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using MultiDownloader.TelegramHost.Models.Enums;
using MultiDownloader.TelegramHost.Models.HttpModels;
using MultiDownloader.TelegramHost.TgBotProcessor.Services;
using Serilog;

namespace MultiDownloader.TelegramHost.TgBotProcessor.Handlers
{
    public class CallbackQueryHandler
    {
        private readonly DownloaderService _downloaderService;
        private readonly ILogger _logger;

        public CallbackQueryHandler(DownloaderService downloaderService, ILogger logger)
        {
            _downloaderService = downloaderService;
            _logger = logger;
        }

        public async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, Update update, Models.User user)
        {
            string data = update.CallbackQuery.Data;
            string[] dataSplit = data.Split('|');
            var action = dataSplit[0] switch
            {
                CallbackConst.DownloadAudio => OnDownloadingAudioRequest(botClient, update, user, dataSplit),
                CallbackConst.DownloadVideo => OnDownloadingVideoRequest(botClient, update, user, dataSplit),
                _ => throw new NotSupportedException()
            };

            await action;
        }

        private async Task OnDownloadingAudioRequest(ITelegramBotClient botClient, Update update, Models.User user, string[] dataSplit)
        {
            string filePath = dataSplit[2] switch
            {
                "audio" => await _downloaderService.DownloadAudioFile(new Uri(dataSplit[1]))
            };

            switch (dataSplit[2])
            {
                case "audio":
                    using (var stream = File.OpenRead(filePath))
                    {
                        await botClient.SendAudio(
                            chatId: user.ChatId,
                            audio: InputFile.FromStream(stream)
                        );
                    }
                    break;
            }
        }

        private async Task OnDownloadingVideoRequest(ITelegramBotClient botClient, Update update, Models.User user, string[] dataSplit)
        {
            string filePath =
                await _downloaderService.DownloadVideoFile(new Uri(dataSplit[1]), dataSplit[2]);

            _logger.Information(filePath ?? "is null");
            using (var stream = File.OpenRead(filePath))
            {
                await botClient.SendVideo(
                    chatId: user.ChatId,
                    video: InputFile.FromStream(stream)
                );
            }
        }
    }
}

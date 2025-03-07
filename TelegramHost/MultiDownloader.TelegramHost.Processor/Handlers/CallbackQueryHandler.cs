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

namespace MultiDownloader.TelegramHost.TgBotProcessor.Handlers
{
    public class CallbackQueryHandler
    {
        private readonly DownloaderService _downloaderService;

        public CallbackQueryHandler(DownloaderService downloaderService)
        {
            _downloaderService = downloaderService;
        }

        public async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, Update update, Models.User user)
        {
            string data = update.CallbackQuery.Data;
            string[] dataSplit = data.Split('|');
            var action = dataSplit[0] switch
            {
                CallbackConst.RequestDownload => OnDownloadingRequest(botClient, update, user, dataSplit),
                _ => throw new NotSupportedException()
            };

            await action;
        }

        private async Task OnDownloadingRequest(ITelegramBotClient botClient, Update update, Models.User user, string[] dataSplit)
        {
            FileData fileData = dataSplit[2] switch
            {
                "audio" => await _downloaderService.DownloadAudioFile(new Uri(dataSplit[1]))
            };

            switch (dataSplit[2])
            {
                case "audio":
                    using (var stream = File.OpenRead(fileData.FilePath))
                    {
                        await botClient.SendAudio(
                            chatId: user.ChatId,
                            audio: InputFile.FromStream(stream, fileData.FileName)
                        );
                    }
                    break;
            }
        }
    }
}

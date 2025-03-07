using MultiDownloader.TelegramHost.Models.Enums;
using MultiDownloader.TelegramHost.Models.HttpModels;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace MultiDownloader.TelegramHost.TgBotProcessor.Services
{
    public class TgMessageService
    {
        public async Task SendFormatsMessage(ITelegramBotClient botClient, Models.User user, Uri uri, List<FormatInfo> formats)
        {
            var keyboard = new List<List<InlineKeyboardButton>>();
            var videosList = formats.Where(x => x.Resolution != "audio" && x.Proto == "https")
                .DistinctBy(x => x.Resolution).ToList();

            int videoCount = videosList.Count;
            int rowsCount = videoCount / 3;

            int generalCount = 0;
            for (int row = 0; row < rowsCount; row++)
            {
                keyboard.Add(new List<InlineKeyboardButton>());
                for (int j = 0; j < 3; j++)
                {
                    var format = videosList[generalCount++];
                    keyboard[keyboard.Count - 1].Add(new InlineKeyboardButton(format.Resolution)
                    {
                        CallbackData = String.Format("{0}|{1}|{2}", CallbackConst.RequestDownload, uri, format.Resolution)
                    });
                }
            }
            if(videoCount % 3 != 0)
            {
                keyboard.Add(new List<InlineKeyboardButton>());
                for (int i = 0; i < videoCount % 3; i++)
                {
                    var format = videosList[generalCount++];
                    keyboard[keyboard.Count - 1].Add(new InlineKeyboardButton(format.Resolution)
                    {
                        CallbackData = String.Format("{0}|{1}|{2}", CallbackConst.RequestDownload, uri, format.Resolution)
                    });
                }
            }
            if(formats.Any(x => x.Resolution == "audio"))
            {
                keyboard.Add(new List<InlineKeyboardButton>());
                keyboard[keyboard.Count - 1].Add(new InlineKeyboardButton("audio")
                {
                    CallbackData = String.Format("{0}|{1}|audio", CallbackConst.RequestDownload, uri)
                });
            }

            await botClient.SendMessage(user.ChatId, "formats", replyMarkup: new InlineKeyboardMarkup(keyboard));
        }
    }
}

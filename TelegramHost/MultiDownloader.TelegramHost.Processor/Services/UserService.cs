using MultiDownloader.TelegramHost.Models;
using MultiDownloader.TelegramHost.Models.Extensions;
using MultiDownloader.TelegramHost.TgBotProcessor.Repositories;
using Serilog;
using Telegram.Bot.Types.Enums;

namespace MultiDownloader.TelegramHost.TgBotProcessor.Services
{
    public class UserService
    {
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;

        public UserService(ILogger logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task EnsureUserExistsAndIsUpdatedAsync(User user)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(user.ChatId);

            if (existingUser == null)
            {
                await _userRepository.AddUserAsync(user);
                return;
            }

            if (existingUser.IsChanged(user))
            {
                await _userRepository.UpdateUserAsync(existingUser);
            }
        }

        public User RetrieveUserFromUpdate(Telegram.Bot.Types.Update update)
        {
            Telegram.Bot.Types.User? telegramUser = update.Type switch
            {
                // Message-related updates
                UpdateType.Message => update.Message?.From,
                UpdateType.EditedMessage => update.EditedMessage?.From,
                UpdateType.ChannelPost => update.ChannelPost?.From,
                UpdateType.EditedChannelPost => update.EditedChannelPost?.From,

                // Interaction-related updates
                UpdateType.CallbackQuery => update.CallbackQuery?.From,
                UpdateType.InlineQuery => update.InlineQuery?.From,
                UpdateType.ChosenInlineResult => update.ChosenInlineResult?.From,

                // Payment-related updates
                UpdateType.ShippingQuery => update.ShippingQuery?.From,
                UpdateType.PreCheckoutQuery => update.PreCheckoutQuery?.From,

                // Membership updates
                UpdateType.MyChatMember => update.MyChatMember?.From,
                UpdateType.ChatMember => update.ChatMember?.From,
                UpdateType.ChatJoinRequest => update.ChatJoinRequest?.From,

                // Poll answer
                UpdateType.PollAnswer => update.PollAnswer?.User,

                _ => null
            };

            if (telegramUser == null)
                throw new InvalidOperationException("No user information found in update");

            return new User()
            {
                ChatId = telegramUser.Id,
                FirstName = telegramUser.FirstName,
                LastName = telegramUser.LastName,
                LastActivityDate = DateTime.UtcNow.Date
            };
        }
    }
}

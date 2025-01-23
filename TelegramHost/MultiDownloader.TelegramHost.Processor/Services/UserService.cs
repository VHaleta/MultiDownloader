using MultiDownloader.TelegramHost.Models;
using MultiDownloader.TelegramHost.Models.Extensions;
using MultiDownloader.TelegramHost.TgBotProcessor.Repositories;
using Serilog;

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

        public async Task<User> EnsureUserExistsAndIsUpdatedAsync(long chatId, string username, string firstName, string lastName)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(chatId);
            var newUser = new User
            {
                ChatId = chatId,
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                LastActivityDate = DateTime.Today
            };

            if (existingUser == null)
            {
                await _userRepository.AddUserAsync(newUser);
                return newUser;
            }

            if (existingUser.IsChanged(newUser))
            {
                await _userRepository.UpdateUserAsync(existingUser);
            }

            return newUser;
        }
    }
}

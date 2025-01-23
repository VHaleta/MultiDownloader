using MultiDownloader.TelegramHost.Models;

namespace MultiDownloader.TelegramHost.TgBotProcessor.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(long chatId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(long chatId);
    }
}

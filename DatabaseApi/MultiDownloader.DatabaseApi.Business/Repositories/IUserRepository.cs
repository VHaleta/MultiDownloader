using MultiDownloader.DatabaseApi.Models;

namespace MultiDownloader.DatabaseApi.Business.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(long chatId);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(long chatId);
    }
}

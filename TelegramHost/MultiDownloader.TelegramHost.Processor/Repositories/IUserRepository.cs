using MultiDownloader.TelegramHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDownloader.TelegramHost.TgBotProcessor.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(long chatId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<string> AddUserAsync(User user);
        Task<string> UpdateUserAsync(User user);
        Task<string> DeleteUserAsync(long chatId);
    }
}

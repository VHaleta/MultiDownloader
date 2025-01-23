using Microsoft.EntityFrameworkCore;
using MultiDownloader.TelegramHost.Models;
using MultiDownloader.TelegramHost.TgBotProcessor.Repositories;

namespace MultiDownloader.TelegramHost.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MultiDownloaderContext _context;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public UserRepository(MultiDownloaderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync() =>
            await _context
            .Users
            .ToListAsync();

        public async Task<User?> GetUserByIdAsync(long chatId) =>
            await _context
            .Users
            .FirstOrDefaultAsync(u => u.ChatId == chatId);

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(long chatId)
        {
            var user = await GetUserByIdAsync(chatId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}

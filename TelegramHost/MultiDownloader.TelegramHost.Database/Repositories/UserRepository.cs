using Microsoft.EntityFrameworkCore;
using MultiDownloader.TelegramHost.Models;
using MultiDownloader.TelegramHost.TgBotProcessor.Repositories;
using Serilog;

namespace MultiDownloader.TelegramHost.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MultiDownloaderContext _context;
        private readonly ILogger _logger;

        public UserRepository(MultiDownloaderContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
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
            _logger.Information("New user {0} {1} ({3}) has been added", user.FirstName, user.LastName, user.Username);
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

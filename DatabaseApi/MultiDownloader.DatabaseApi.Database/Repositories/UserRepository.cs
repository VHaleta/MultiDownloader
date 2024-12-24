using Microsoft.EntityFrameworkCore;
using MultiDownloader.DatabaseApi.Business.Repositories;
using MultiDownloader.DatabaseApi.Models;

namespace MultiDownloader.DatabaseApi.Database.Repositories
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
            .Include(user => user.Jobs)
            .ToListAsync();

        public async Task<User?> GetUserByIdAsync(long chatId) =>
            await _context
            .Users
            .Include(user => user.Jobs)
            .FirstOrDefaultAsync(u => u.ChatId == chatId);

        public async Task AddUserAsync(User user)
        {
            await _semaphore.WaitAsync();
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            await _semaphore.WaitAsync();
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            finally 
            {
                _semaphore.Release();
            }
        }

        public async Task DeleteUserAsync(long chatId)
        {
            await _semaphore.WaitAsync();
            try
            {
                var user = await GetUserByIdAsync(chatId);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}

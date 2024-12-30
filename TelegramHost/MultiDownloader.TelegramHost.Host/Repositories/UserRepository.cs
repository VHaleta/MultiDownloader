using MultiDownloader.DatabaseApi.GrpcHost.Users;
using MultiDownloader.TelegramHost.Models;
using MultiDownloader.TelegramHost.TgBotProcessor.Repositories;

namespace MultiDownloader.TelegramHost.Host.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserService.UserServiceClient _userServiceClient;

        public UserRepository(UserService.UserServiceClient userServiceClient)
        {
            _userServiceClient = userServiceClient;
        }

        public async Task<User> GetUserAsync(long chatId)
        {
            var request = new UserRequest { ChatId = chatId };
            var response = await _userServiceClient.GetUserAsync(request);

            return MapToLocalUser(response);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var request = new EmptyRequest();
            var response = await _userServiceClient.GetAllUsersAsync(request);

            return response.Users.Select(MapToLocalUser);
        }

        public async Task<string> AddUserAsync(AddUserRequest user)
        {
            var response = await _userServiceClient.AddUserAsync(user);
            return response.Message;
        }

        public async Task<string> UpdateUserAsync(UpdateUserRequest user)
        {
            var response = await _userServiceClient.UpdateUserAsync(user);
            return response.Message;
        }

        public async Task<string> DeleteUserAsync(long chatId)
        {
            var request = new UserRequest { ChatId = chatId };
            var response = await _userServiceClient.DeleteUserAsync(request);
            return response.Message;
        }

        private User MapToLocalUser(UserResponse response)
        {
            return new User
            {
                ChatId = response.ChatId,
                Username = response.Username,
                FirstName = response.FirstName,
                LastName = response.LastName,
                JobIds = response.Jobs.Select(job => job.JobId).ToList()
            };
        }

        public Task<string> AddUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}

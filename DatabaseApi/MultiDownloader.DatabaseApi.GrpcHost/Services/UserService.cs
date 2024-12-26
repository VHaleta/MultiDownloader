using Grpc.Core;
using MultiDownloader.DatabaseApi.Business.Repositories;
using MultiDownloader.DatabaseApi.GrpcHost.Users;
using MultiDownloader.DatabaseApi.Models;

namespace MultiDownloader.DatabaseApi.GrpcHost.Services
{
    public class UserService : Users.UserService.UserServiceBase
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public override async Task<UserResponse> GetUser(UserRequest request, ServerCallContext context)
        {
            var user = await _userRepository.GetUserByIdAsync(request.ChatId);
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }

            return MapUserToResponse(user);
        }

        public override async Task<UsersResponse> GetAllUsers(EmptyRequest request, ServerCallContext context)
        {
            var users = await _userRepository.GetAllUsersAsync();

            var response = new UsersResponse();
            response.Users.AddRange(users.Select(MapUserToResponse));
            return response;
        }

        public override async Task<ResultMessage> AddUser(AddUserRequest request, ServerCallContext context)
        {
            var newUser = new User
            {
                ChatId = request.ChatId,
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            await _userRepository.AddUserAsync(newUser);

            return new ResultMessage
            {
                Message = "User successfully added."
            };
        }

        public override async Task<ResultMessage> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            var user = await _userRepository.GetUserByIdAsync(request.ChatId);
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }

            user.Username = request.Username;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            await _userRepository.UpdateUserAsync(user);

            return new ResultMessage
            {
                Message = "User successfully updated."
            };
        }

        public override async Task<ResultMessage> DeleteUser(UserRequest request, ServerCallContext context)
        {
            var user = await _userRepository.GetUserByIdAsync(request.ChatId);
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }

            await _userRepository.DeleteUserAsync(user.ChatId);

            return new ResultMessage
            {
                Message = "User successfully deleted."
            };
        }

        private UserResponse MapUserToResponse(User user)
        {
            return new UserResponse
            {
                ChatId = user.ChatId,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Jobs =
            {
                user.Jobs.Select(job => new Users.Job
                {
                    JobId = job.JobId,
                    Title = job.Title,
                    Url = job.URL,
                    FileType = job.FileType,
                    Source = job.Sourse,
                    ResultStatus = job.ResultStatus
                })
            }
            };
        }
    }
}

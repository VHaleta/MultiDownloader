using Microsoft.AspNetCore.Mvc;
using MultiDownloader.DatabaseApi.Business.Repositories;
using MultiDownloader.DatabaseApi.Models;

namespace MultiDownloader.DatabaseApi.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetUser(long id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null) // TODO: Add user validator
            {
                return BadRequest(new { Message = "Invalid user data" });
            }

            await _userRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.ChatId }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] User user)
        {
            if (user == null || id != user.ChatId)
            {
                return BadRequest(new { Message = "Invalid user data or ID mismatch" });
            }

            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            await _userRepository.UpdateUserAsync(user);
            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            await _userRepository.DeleteUserAsync(id);
            return NoContent();
        }
    }
}

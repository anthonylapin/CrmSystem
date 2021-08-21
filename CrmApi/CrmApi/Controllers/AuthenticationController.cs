using System.Threading.Tasks;
using CrmApi.Contracts;
using CrmApi.DataTransferObjects;
using CrmApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CrmApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationManager _authManager;
        private readonly ILogger<AuthenticationController> _logger;
       

        public AuthenticationController(UserManager<User> userManager, IAuthenticationManager authManager,
            ILogger<AuthenticationController> logger)
        {
            _userManager = userManager;
            _authManager = authManager;
            _logger = logger;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto userRegisterDto)
        {
            var user = new User() { Email = userRegisterDto.Email, UserName = userRegisterDto.Email };
            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

            if (result.Succeeded) return StatusCode(201);

            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserDto user)
        {
            _logger.LogDebug("Authenticate was called!");

            if (await _authManager.ValidateUser(user))
                return Ok(new {Token = await _authManager.CreateToken()});

            _logger.LogWarning($"{nameof(Authenticate)}: Authentication failed for user {user.Email}. Wrong credentials");
            return Unauthorized();

        }

        [Authorize]
        [Route("test")]
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            return Ok(currentUser);
        }
    }
}
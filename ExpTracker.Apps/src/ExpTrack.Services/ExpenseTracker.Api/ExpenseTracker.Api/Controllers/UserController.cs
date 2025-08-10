using AutoMapper;
using ExpTrack.AppBal.Contracts;
using ExpTrack.AppModels.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userrepo;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userrepo,
            ILogger<UserController> logger,
            IMapper mapper
            )
        {
            this._userrepo = userrepo;
            this._logger = logger;
            this._mapper = mapper;
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is null.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _userrepo.CreateUserAsync(userDto);
                if (user == null)
                {
                    return BadRequest("Failed to create user.");
                }
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user in {MethodName}", nameof(CreateUser));
                return Problem("An error occurred while creating the user.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(long id)
        {
            try
            {
                var user = await _userrepo.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with ID {Id} in {MethodName}", id, nameof(GetUserById));
                return Problem("An error occurred while retrieving the user.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}

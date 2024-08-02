using Application;
using Application.User.Dtos;
using Application.User.Ports;
using Application.User.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserManager _userManager;

        public UserController(ILogger<UserController> logger, IUserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Post(UserDto user)
        {
            var request = new CreateUserRequest
            {
                Data = user
            };

            var res = await _userManager.CreateUser(request);

            if (res.Success) return Created("", res.Data);

            if (res.ErrorCode == ErrorCodes.USER_NOT_FOUND)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.MISSING_REQUIRED_INFORMATION)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.INVALID_EMAIL)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.COULD_NOT_STORE_DATA)
            {
                return BadRequest(res);
            }

            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }

        [Authorize("Bearer")]
        [HttpGet]
        public async Task<ActionResult<UserDto>> Get(int userId)
        {
            var res = await _userManager.GetUser(userId);
            
            if (res.Success) return Created("", res.Data);
            
            return NotFound(res);
        }

        [Authorize("Bearer")]
        [HttpPut]
        public async Task<ActionResult<UserDto>> Put(UserDto user)
        {
            var request = new UpdateUserRequest
            {
                Data = user
            };

            var res = await _userManager.UpdateUser(request);

            if (res.ErrorCode == ErrorCodes.MISSING_REQUIRED_INFORMATION) return BadRequest(res);

            if (res.ErrorCode == ErrorCodes.COULD_NOT_STORE_DATA) return BadRequest(res);

            if (res.Success) return Ok(res.Data);

            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }

        [Authorize("Bearer")]
        [HttpDelete]
        public async Task<ActionResult<UserDto>> Delete(int userId)
        {
            var res = await _userManager.DeleteUser(userId);

            if(res.Success) return Ok(res.Data);

            return NotFound(res);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using TechX.API.Models.DTOs;
using TechX.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                return Ok(user);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving user" });
            }
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserDTO>> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                return Ok(user);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving user" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] CreateUserDTO createDto)
        {
            try
            {
                var user = await _userService.CreateUserAsync(createDto);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while creating user" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> UpdateUser(int id, [FromBody] UpdateUserDTO updateDto)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(id, updateDto);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                return Ok(user);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while updating user" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "User not found" });
                }
                return Ok(new { message = "User deleted successfully" });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while deleting user" });
            }
        }

        [HttpPost("{id}/verify-email")]
        public async Task<ActionResult> VerifyEmail(int id)
        {
            try
            {
                var result = await _userService.VerifyEmailAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "User not found" });
                }
                return Ok(new { message = "Email verified successfully" });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while verifying email" });
            }
        }

        [HttpPost("{id}/verify-phone")]
        public async Task<ActionResult> VerifyPhone(int id)
        {
            try
            {
                var result = await _userService.VerifyPhoneAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "User not found" });
                }
                return Ok(new { message = "Phone verified successfully" });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while verifying phone" });
            }
        }
    }
} 
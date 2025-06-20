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
    public class LoyaltyController : ControllerBase
    {
        private readonly ILoyaltyService _loyaltyService;

        public LoyaltyController(ILoyaltyService loyaltyService)
        {
            _loyaltyService = loyaltyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoyaltyPointsDTO>>> GetAllLoyaltyPoints()
        {
            try
            {
                var loyaltyPoints = await _loyaltyService.GetUserLoyaltyPointsAsync(0); // This should be filtered by user
                return Ok(loyaltyPoints);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving loyalty points" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoyaltyPointsDTO>> GetLoyaltyPointsById(int id)
        {
            try
            {
                var loyaltyPoints = await _loyaltyService.GetLoyaltyPointsByIdAsync(id);
                return Ok(loyaltyPoints);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving loyalty points" });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<LoyaltyPointsDTO>>> GetUserLoyaltyPoints(int userId)
        {
            try
            {
                var loyaltyPoints = await _loyaltyService.GetUserLoyaltyPointsAsync(userId);
                return Ok(loyaltyPoints);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving user loyalty points" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<LoyaltyPointsDTO>> CreateLoyaltyPoints([FromBody] CreateLoyaltyPointsDTO createDto)
        {
            try
            {
                var loyaltyPoints = await _loyaltyService.CreateLoyaltyPointsAsync(createDto);
                return CreatedAtAction(nameof(GetLoyaltyPointsById), new { id = loyaltyPoints.Id }, loyaltyPoints);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while creating loyalty points" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LoyaltyPointsDTO>> UpdateLoyaltyPoints(int id, [FromBody] UpdateLoyaltyPointsDTO updateDto)
        {
            try
            {
                var loyaltyPoints = await _loyaltyService.UpdateLoyaltyPointsAsync(id, updateDto);
                return Ok(loyaltyPoints);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while updating loyalty points" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLoyaltyPoints(int id)
        {
            try
            {
                var result = await _loyaltyService.DeleteLoyaltyPointsAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "Loyalty points not found" });
                }
                return Ok(new { message = "Loyalty points deleted successfully" });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while deleting loyalty points" });
            }
        }

        [HttpGet("user/{userId}/total")]
        public async Task<ActionResult<decimal>> GetUserTotalLoyaltyPoints(int userId)
        {
            try
            {
                var totalLoyaltyPoints = await _loyaltyService.GetUserTotalLoyaltyPointsAsync(userId);
                return Ok(new { totalLoyaltyPoints });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while calculating total loyalty points" });
            }
        }
    }
} 
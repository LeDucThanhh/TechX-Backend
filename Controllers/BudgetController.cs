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
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetDTO>>> GetAllBudgets()
        {
            try
            {
                var budgets = await _budgetService.GetAllBudgetsAsync();
                return Ok(budgets);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving budgets" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetDTO>> GetBudgetById(int id)
        {
            try
            {
                var budget = await _budgetService.GetBudgetByIdAsync(id);
                return Ok(budget);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving budget" });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<BudgetDTO>>> GetUserBudgets(int userId)
        {
            try
            {
                var budgets = await _budgetService.GetUserBudgetsAsync(userId);
                return Ok(budgets);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving user budgets" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BudgetDTO>> CreateBudget([FromBody] CreateBudgetDTO createDto)
        {
            try
            {
                var budget = await _budgetService.CreateBudgetAsync(createDto);
                return CreatedAtAction(nameof(GetBudgetById), new { id = budget.Id }, budget);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while creating budget" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BudgetDTO>> UpdateBudget(int id, [FromBody] UpdateBudgetDTO updateDto)
        {
            try
            {
                var budget = await _budgetService.UpdateBudgetAsync(id, updateDto);
                return Ok(budget);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while updating budget" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBudget(int id)
        {
            try
            {
                var result = await _budgetService.DeleteBudgetAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "Budget not found" });
                }
                return Ok(new { message = "Budget deleted successfully" });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while deleting budget" });
            }
        }

        [HttpPut("{id}/spent-amount")]
        public async Task<ActionResult<BudgetDTO>> UpdateBudgetSpentAmount(int id, [FromBody] UpdateSpentAmountDTO updateDto)
        {
            try
            {
                var budget = await _budgetService.UpdateBudgetSpentAmountAsync(id, updateDto.SpentAmount);
                return Ok(budget);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while updating budget spent amount" });
            }
        }
    }

    public class UpdateSpentAmountDTO
    {
        public decimal SpentAmount { get; set; }
    }
} 
using ExpTrack.AppBal.Contracts;
using ExpTrack.AppModels.DTOs;
using AutoMapper;
using ExpTrack.DbAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseRepository _expenserepo;
        private readonly ILogger<ExpenseController> _logger;
        private readonly IMapper _mapper;

        public ExpenseController(IExpenseRepository expenserepo,
            ILogger<ExpenseController> logger,
            IMapper mapper)
        {
            this._expenserepo = expenserepo;
            this._logger = logger;
            this._mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllExpense/{id}")]
        public async Task<IActionResult> GetExpenseByUserId(int id)
        {
            try
            {
                var expenses = await _expenserepo.GetExpensesByUserIdAsync(id);
                if (expenses == null || !expenses.Any())
                {
                    return NotFound($"No expenses found for user with ID {id}.");
                }
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving expenses for user ID {id} in {nameof(GetExpenseByUserId)}");
                return Problem("An error occurred while retrieving expenses.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetExpenseByCategoryId/{id}")]
        public async Task<IActionResult> GetExpenseByCategoryId(int id)
        {
            try
            {
                var expenses = await _expenserepo.GetExpensesByCategoryIdAsync(id);
                if (expenses == null || !expenses.Any())
                {
                    return NotFound($"No expenses found for category with ID {id}.");
                }
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving expenses for category ID {id} in {nameof(GetExpenseByCategoryId)}");
                return Problem("An error occurred while retrieving expenses.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("AddExpense")]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseCreateDto expense)
        {
            if (expense == null)
            {
                return BadRequest("Expense data is null.");
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdExpense = await _expenserepo.CreateExpenseAsync(_mapper.Map<ExpenseCreateDto, Expense>(expense));
                return CreatedAtAction(nameof(GetExpenseByUserId), new { id = expense.UserId }, createdExpense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding expense for User id: {expense.UserId} in {nameof(AddExpense)}");
                return Problem("An error occurred while adding the expense.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("DeleteExpense/{id}")]
        public async Task<IActionResult> DeleteExpense(long id)
        {
            try
            {
                var isDeleted = await _expenserepo.DeleteExpenseAsync(id);
                if (!isDeleted)
                {
                    return NotFound($"Expense with ID {id} not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting expense with ID {id} in {nameof(DeleteExpense)}");
                return Problem("An error occurred while deleting the expense.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut]
        [Route("UpdateExpense/{id}")]
        public async Task<IActionResult> UpdateExpense(long id, [FromBody] ExpenseCreateDto expense)
        {
            if (expense == null)
            {
                return BadRequest("Expense data is null.");
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var updatedExpense = await _expenserepo.UpdateExpenseAsync(id, _mapper.Map<ExpenseCreateDto, Expense>(expense));
                if (updatedExpense == null)
                {
                    return NotFound($"Expense with ID {id} not found.");
                }
                return Ok(updatedExpense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating expense with ID {id} in {nameof(UpdateExpense)}");
                return Problem("An error occurred while updating the expense.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}

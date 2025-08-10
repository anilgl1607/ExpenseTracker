using AppBal.Contracts;
using AppModels.DTOs;
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
        [Route("GetAllExpenses")]
        public async Task<IActionResult> GetAllExpenses()
        {
            try
            {
                var expenses = await _expenserepo.GetAllExpensesAsync();
                if (expenses == null || !expenses.Any())
                {
                    return NotFound("No expenses found.");
                }
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving expenses in{nameof(GetAllExpenses)}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet]
        [Route("GetExpenseByUserId/{id}")]
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
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
            try
            {
                var createdExpense = await _expenserepo.CreateExpenseAsync(_mapper.Map<ExpenseCreateDto,Expense>(expense));
                return CreatedAtAction(nameof(GetExpenseByUserId), new { id = expense.UserId }, createdExpense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding expense in {nameof(AddExpense)}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}

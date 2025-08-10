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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _caterepo;
        private readonly ILogger<CategoryController> _logger;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository caterepo,
            ILogger<CategoryController> logger
            , IMapper mapper)
        {
            this._caterepo = caterepo;
            this._logger = logger;
            this._mapper = mapper;
        }
        [HttpGet]
        [Route("GetAllCategories/{userid}")]
        public async Task<IActionResult> GetCategoriesByUserId(int userid)
        {
            try
            {
                var categories = await _caterepo.GetCategoriesByUserIdAsync(userid);
                if (categories == null || !categories.Any())
                {
                    return NotFound($"No categories found for user with ID {userid}.");
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving categories for user ID {userid} in {nameof(GetCategoriesByUserId)}");
                return Problem("An error occurred while retrieving categories.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetCategoryById/{categoryid}")]
        public IActionResult GetCategory(int categoryid)
        {
            try
            {
                var category = _caterepo.GetCategoryByIdAsync(categoryid).Result;
                if (category == null)
                {
                    return NotFound($"Category with ID {categoryid} not found.");
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving category with ID {categoryid} in {nameof(GetCategory)}");
                return Problem("An error occurred while retrieving the category.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto category)
        {
            if (category == null)
            {
                return BadRequest("Category data is required.");
            }
            try
            {
                var result = await _caterepo.CreateCategoryAsync(_mapper.Map<CategoryCreateDto, Category>(category));
                if (result)
                {
                    return CreatedAtAction(nameof(GetCategoriesByUserId), new { categoryid = category.UserId }, category);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create category.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating category in {nameof(CreateCategory)}");
                return Problem("An error occurred while creating the category.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(long id, [FromBody] CategoryCreateDto category)
        {
            if (category == null)
            {
                return BadRequest("Category data is required.");
            }
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedCategory = await _caterepo.UpdateCategoryAsync(id, _mapper.Map<CategoryCreateDto, Category>(category));
                if (updatedCategory == null)
                {
                    return NotFound($"Category with ID {id} not found.");
                }
                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating category with ID {id} in {nameof(UpdateCategory)}");
                return Problem("An error occurred while updating the category.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            try
            {
                var result = await _caterepo.DeleteCategoryAsync(id);
                if (result)
                {
                    return NoContent(); // 204 No Content
                }
                return NotFound($"Category with ID {id} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting category with ID {id} in {nameof(DeleteCategory)}");
                return Problem("An error occurred while deleting the category.", statusCode: StatusCodes.Status500InternalServerError);
            }

        }
    }
}

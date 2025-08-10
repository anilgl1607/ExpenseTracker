using ExpTrack.DbAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTrack.AppBal.Contracts
{
    public interface ICategoryRepository
    {
        Task<bool> CreateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(long id);
        Task<List<Category>> GetCategoriesByUserIdAsync(long userId);
        Task<Category?> GetCategoryByIdAsync(long id);
        Task<Category> UpdateCategoryAsync(long id, Category category);
    }
}

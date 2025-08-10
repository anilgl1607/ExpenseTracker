using ExpTrack.AppBal.Contracts;
using ExpTrack.DbAccess.Contracts;
using ExpTrack.DbAccess.Entities;
using ExpTrack.EfCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTrack.AppBal.Adapters
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext context;

        public CategoryRepository(IConfigurationConnectionString configuration,
            ILoggerFactory loggerFactory)
        {
            context = new AppDbContext(configuration.GetConnectionString("ExpTrackDb"), loggerFactory);
        }
        public async Task<bool> CreateCategoryAsync(Category category)
        {
            try
            {
                await context.Categories.AddAsync(category).ConfigureAwait(false);
                return await context.SaveChangesAsync().ConfigureAwait(false) > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create expense.", ex);
            }
        }

        public async Task<bool> DeleteCategoryAsync(long id)
        {
            try
            {
                var catory = await context.Categories.FindAsync(id);
                if (catory == null)
                    throw new KeyNotFoundException($"Category with ID {id} not found.");
                context.Categories.Remove(catory);
                return await context.SaveChangesAsync().ConfigureAwait(false) > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to delete category.",ex);
            }
        }

        public async Task<List<Category>> GetCategoriesByUserIdAsync(long userId)
        {
            try
            {
                return await context.Categories
                    .AsNoTracking()
                    .Where(c => c.UserId == userId)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("Failed to retrive Categories by User Id",ex);
            }
        }

        public async Task<Category?> GetCategoryByIdAsync(long id)
        {
            try
            {
                return await context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve category with ID {id}.", ex);
            }
        }

        public Task<Category> UpdateCategoryAsync(long id, Category category)
        {
            try
            {
                var existingCategory = context.Categories.Find(id);
                if (existingCategory == null)
                    throw new KeyNotFoundException($"Category with ID {id} not found.");
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
                context.Categories.Update(existingCategory);
                return context.SaveChangesAsync().ContinueWith(t => existingCategory);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to update category with ID {id}.", ex);
            }
        }
    }
}

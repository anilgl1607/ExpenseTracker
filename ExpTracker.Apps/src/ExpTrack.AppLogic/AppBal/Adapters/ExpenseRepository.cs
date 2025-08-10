using ExpTrack.AppBal.Contracts;
using ExpTrack.AppModels.DTOs;
using ExpTrack.DbAccess.Contracts;
using ExpTrack.DbAccess.Entities;
using ExpTrack.EfCore.Contexts;
using ExpTrack.EfCore.Repositories.Adapters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExpTrack.AppBal.Adapters
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext context;

        public ExpenseRepository(IConfigurationConnectionString configuration,
            ILoggerFactory loggerFactory)
        {
            context = new AppDbContext(configuration.GetConnectionString("ExpTrackDb"), loggerFactory);
        }

        public async Task<bool> CreateExpenseAsync(Expense expense)
        {
            try
            {
                await context.Expenses.AddAsync(expense).ConfigureAwait(false);
                return await context.SaveChangesAsync().ConfigureAwait(false) > 0;
            }
            catch (Exception ex)
            {
                // Log exception or handle as needed
                throw new InvalidOperationException("Failed to create expense.", ex);
            }
        }

        public async Task<bool> DeleteExpenseAsync(long id)
        {
            try
            {
                var expense = await context.Expenses.FindAsync(id).ConfigureAwait(false);
                if (expense == null)
                    throw new KeyNotFoundException($"Expense with ID {id} not found.");

                context.Expenses.Remove(expense);
                return await context.SaveChangesAsync().ConfigureAwait(false) > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to delete expense.", ex);
            }
        }

        public async Task<List<Expense>> GetExpenseByDateAsync(DateTime fromdate, DateTime Todate)
        {
            try
            {
                // Use AsNoTracking for read-only queries to improve performance
                return await context.Expenses
                    .AsNoTracking()
                    .Where(e => (e.CreatedAt >= fromdate && e.CreatedAt <= Todate) ||
                                (e.ModifiedAt >= fromdate && e.ModifiedAt <= Todate))
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to retrieve expenses by date.", ex);
            }
        }

        public async Task<List<Expense>> GetExpensesByCategoryIdAsync(long categoryId)
        {
            try
            {
                return await context.Expenses
                    .AsNoTracking()
                    .Where(e => e.CategoryId == categoryId)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to retrieve expenses by category.", ex);
            }
        }

        public async Task<List<Expense>> GetExpensesByUserIdAsync(long userId)
        {
            try
            {
                return await context.Expenses
                    .AsNoTracking()
                    .Where(e => e.UserId == userId)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to retrieve expenses by user.", ex);
            }
        }

        public async Task<Expense> UpdateExpenseAsync(long id, Expense expense)
        {
            try
            {
                var existingExpense = await context.Expenses.FindAsync(id).ConfigureAwait(false);
                if (existingExpense == null)
                    throw new KeyNotFoundException($"Expense with ID {id} not found.");

                // Only update changed fields for efficiency
                existingExpense.Amount = expense.Amount;
                existingExpense.Description = expense.Description;
                existingExpense.CategoryId = expense.CategoryId;
                existingExpense.UserId = expense.UserId;
                existingExpense.ModifiedAt = DateTime.UtcNow;

                context.Expenses.Update(existingExpense);
                await context.SaveChangesAsync().ConfigureAwait(false);

                // Return the tracked entity directly
                return existingExpense;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to update expense.", ex);
            }
        }
    }
}

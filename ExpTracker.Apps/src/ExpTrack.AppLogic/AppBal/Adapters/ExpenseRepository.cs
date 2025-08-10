using AppBal.Contracts;
using AppModels.DTOs;
using ExpTrack.DbAccess.Contracts;
using ExpTrack.DbAccess.Entities;
using ExpTrack.EfCore.Contexts;
using ExpTrack.EfCore.Repositories.Adapters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppBal.Adapters
{
    public class ExpenseRepository : AppContextFactory<AppDbContext>, IExpenseRepository
    {
        private readonly IConfigurationConnectionString _configuration;
        private readonly ILoggerFactory _loggerFactory;

        public ExpenseRepository(IConfigurationConnectionString configuration,
            ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
            this._configuration = configuration;
            this._loggerFactory = loggerFactory;
        }

        protected override AppDbContext CreateDbContext()
        {
            var connectionString = _configuration.GetConnectionString("ExpTrackDb");
            return new AppDbContext(connectionString, _loggerFactory);
        }
        public async Task<bool> CreateExpenseAsync(Expense expense)
        {
            using (var context = CreateDbContext())
            {
                await context.Expenses.AddAsync(expense);
                return (await context.SaveChangesAsync() > 0 ? true : false);
            }
        }

        public Task<bool> DeleteExpenseAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Expense>> GetAllExpensesAsync()
        {
            using (var context = CreateDbContext())
            {
                return context.Expenses.ToListAsync();
            }
        }

        public async Task<List<Expense>> GetExpenseByIdAsync(DateTime fromdate, DateTime Todate)
        {
            using (var context = CreateDbContext())
            {
                return await context.Expenses
                    .Where(e => (e.CreatedAt >= fromdate && e.CreatedAt <= Todate) ||
                    (e.ModifiedAt >= fromdate && e.ModifiedAt <= Todate))
                    .ToListAsync();
            }
        }

        public async Task<List<Expense>> GetExpensesByCategoryIdAsync(long categoryId)
        {
            using (var context = CreateDbContext())
            {
                return await context.Expenses
                    .Where(e => e.CategoryId == categoryId)
                    .ToListAsync();
            }
        }

        public async Task<List<Expense>> GetExpensesByUserIdAsync(long userId)
        {
            using (var context = CreateDbContext())
            {
                return await context.Expenses
                    .Where(e => e.UserId == userId)
                    .ToListAsync();
            }
        }

        public async Task<Expense> UpdateExpenseAsync(long id, Expense expense)
        {
            using (var context = CreateDbContext())
            {
                var existingExpense = context.Expenses.Find(id);
                if (existingExpense == null)
                {
                    throw new KeyNotFoundException($"Expense with ID {id} not found.");
                }
                existingExpense.Amount = expense.Amount;
                existingExpense.Description = expense.Description;
                existingExpense.CategoryId = expense.CategoryId;
                existingExpense.UserId = expense.UserId;
                context.Expenses.Update(existingExpense);
                await context.SaveChangesAsync();
                return new Expense
                {
                    Id = existingExpense.Id,
                    Amount = existingExpense.Amount,
                    Description = existingExpense.Description,
                    CategoryId = existingExpense.CategoryId,
                    UserId = existingExpense.UserId
                };
            }
        }
    }
}

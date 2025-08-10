using AppModels.DTOs;
using ExpTrack.DbAccess.Entities;

namespace AppBal.Contracts
{
    public interface IExpenseRepository
    {
        Task<bool> CreateExpenseAsync(Expense expense);
        Task<bool> DeleteExpenseAsync(long id);
        Task<List<Expense>> GetAllExpensesAsync();
        Task<List<Expense>> GetExpenseByIdAsync(DateTime fromdate, DateTime Todate);
        Task<List<Expense>> GetExpensesByCategoryIdAsync(long categoryId);
        Task<List<Expense>> GetExpensesByUserIdAsync(long userId);
        Task<Expense> UpdateExpenseAsync(long id, Expense expense);
    }
}

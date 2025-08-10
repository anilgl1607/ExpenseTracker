using ExpTrack.AppModels.DTOs;
using ExpTrack.DbAccess.Entities;

namespace ExpTrack.AppBal.Contracts
{
    public interface IExpenseRepository
    {
        Task<bool> CreateExpenseAsync(Expense expense);
        Task<bool> DeleteExpenseAsync(long id);
        Task<List<Expense>> GetExpenseByDateAsync(DateTime fromdate, DateTime Todate);
        Task<List<Expense>> GetExpensesByCategoryIdAsync(long categoryId);
        Task<List<Expense>> GetExpensesByUserIdAsync(long userId);
        Task<Expense> UpdateExpenseAsync(long id, Expense expense);
    }
}

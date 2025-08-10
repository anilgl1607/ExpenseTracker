using ExpTrack.AppModels.DTOs;
using ExpTrack.DbAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTrack.AppBal.Contracts
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(UserCreateDto dto);
        Task<User?> GetUserByIdAsync(long id);
        Task<bool> VerifyPasswordAsync(string username, string password);
    }
}

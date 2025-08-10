using ExpTrack.AppBal.Contracts;
using ExpTrack.AppModels.DTOs;
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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(IConfigurationConnectionString connectionconfig,
            ILoggerFactory factory)
        {
            _context= new AppDbContext(connectionconfig.GetConnectionString("ExpTrackDb"), factory);
        }
        public async Task<User> CreateUserAsync(UserCreateDto dto)
        {
            try
            {
                CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var user = new User
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    PasswordHash = Encoding.UTF8.GetString(passwordHash),
                    PasswordSalt = Encoding.UTF8.GetString(passwordSalt),
                    CreatedAt = dto.CreatedAt == default ? DateTime.UtcNow : dto.CreatedAt
                };

                await _context.Users.AddAsync(user).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return user;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create user.", ex);
            }
        }

        public async Task<User?> GetUserByIdAsync(long id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return null; // User not found
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve user with ID {id}.", ex);
            }
        }

        public async Task<bool> VerifyPasswordAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username).ConfigureAwait(false);
            if (user == null) return false;

            return VerifyPasswordHash(password, Encoding.UTF8.GetBytes(user.PasswordHash), Encoding.UTF8.GetBytes(user.PasswordSalt));

        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }
    }
}

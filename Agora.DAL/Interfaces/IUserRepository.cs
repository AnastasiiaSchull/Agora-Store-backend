using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace Agora.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<IQueryable<User>> GetAll();
        Task<User> GetByEmail(string email);
        Task<User> Get(int id);
        Task<ICollection<Address>> GetAddressesByUserId(int userId);
        Task Create(User user);  
        void Update(User user);
        Task Delete(int id);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        Task SavePasswordResetTokenAsync(string email, string token, DateTime expiresAt);
        Task<PasswordResetToken?> GetValidResetTokenAsync(string token);
        Task<User> GetAddresses(int id);
    }
}

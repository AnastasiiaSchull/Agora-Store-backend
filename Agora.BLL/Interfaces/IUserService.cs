using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;
using Agora.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace Agora.BLL.Interfaces
{
    public interface IUserService
    {
        Task<IQueryable<UserDTO>> GetAll();
        Task<UserDTO> Get(int id);
        Task<bool> CheckEmailExists(string email);
        Task<int> CreateReturnId(UserDTO userDTO);
        Task<UserDTO> GetByEmail(string email);
        Task<UserDTO> GetByCheckEmail(string email);
        Task<RoleDTO> GetRoleByUserId(int id);
        Task Create(UserDTO userDTO);
        Task Update(UserDTO userDTO);
        Task Delete(int id);
        Task<bool> CreateGoogle(UserDTO userDTO);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<bool> ResetUserPasswordAsync(string email, string token, string newPassword);
        Task<string> GetGeneratePasswordResetTokenAsync(ApplicationUser user);
        Task<string> CreatePasswordResetTokenAsync(string email);
        Task<bool> ValidateResetTokenAsync(string token);
        Task<UserDTO> GetById(int id);
        Task UpdateSellerEmailAsync(int userId, string newEmail);
        Task UpdateSellerPhoneNumberAsync(int userId, string newPhoneNumber);
    }
}

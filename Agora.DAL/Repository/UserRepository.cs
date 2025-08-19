using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Agora.DAL.Repository
{
    public class UserRepository: IUserRepository
    {
        private AgoraContext db;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(AgoraContext context, UserManager<ApplicationUser> userManager)
        {
            this.db = context;
            this._userManager = userManager;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task SavePasswordResetTokenAsync(string email, string token, DateTime expiresAt)
        {
            var resetToken = new PasswordResetToken
            {
                Email = email,
                Token = token,
                ExpirationDate = expiresAt
            };
            db.PasswordResetTokens.Add(resetToken);
            await db.SaveChangesAsync();
        }

        public async Task<PasswordResetToken?> GetValidResetTokenAsync(string token)
        {
            return await db.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.Token == token && t.ExpirationDate > DateTime.UtcNow);
        }

        public async Task<IQueryable<User>> GetAll()
        {
            return db.Users;
        }

        public async Task<User> Get(int id)
        {
            return await db.Users.FindAsync(id);
        }

        public async Task<User> GetAddresses(int id)
        {
            return await db.Users
                .Include(u => u.AddressUsers)
                    .ThenInclude(au => au.Address)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByEmail(string email)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            return user;
        }

        public async Task<ICollection<Address>> GetAddressesByUserId(int userId)
        {
            var user = await db.Users
                .Include(u => u.AddressUsers)
                    .ThenInclude(au => au.Address)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.AddressUsers?
                .Select(au => au.Address)
                .ToList() ?? new List<Address>();
        }

        public async Task Create(User user)
        {
            await db.Users.AddAsync(user);
        }

        public void Update(User user)
        {
            db.Entry(user).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            User? user = await db.Users.FindAsync(id);
            if (user != null)
                db.Users.Remove(user);
        }
    }
}

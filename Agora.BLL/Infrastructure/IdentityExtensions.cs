using Agora.DAL.EF;
using Agora.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Agora.BLL.Infrastructure
{
    public static class IdentityExtensions
    {
        public static void AddIdentityServices(this IServiceCollection services)
        {
            services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<AgoraContext>()
                .AddDefaultTokenProviders();
        }
    }
}

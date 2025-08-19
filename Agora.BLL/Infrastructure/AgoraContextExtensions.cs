using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Agora.DAL.EF;

namespace Agora.BLL.Infrastructure
{
    public static class AgoraContextExtensions
    {
        public static void AddAgoraContext(this IServiceCollection services, string connection)
        {
             services.AddDbContext<AgoraContext>(options => options.UseLazyLoadingProxies().UseMySql(connection, ServerVersion.AutoDetect(connection)));
        }
    }
}

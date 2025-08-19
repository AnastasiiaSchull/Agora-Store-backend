using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Interfaces;
using Agora.DAL.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Agora.BLL.Infrastructure
{
    public static class UnitOfWorkServiceExtensions
    {
        public static void AddUnitOfWorkService(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
        }
    }
}

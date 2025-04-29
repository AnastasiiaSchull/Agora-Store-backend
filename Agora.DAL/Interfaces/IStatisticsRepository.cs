using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IStatisticsRepository
    {
        Task<IQueryable<object>> GetWeeksStatisticsBySales(int storeId);
        Task<IQueryable<object>> GetWeeksStatisticsBySalesGeneral(int sellerId);
        Task<IQueryable<object>> GetTop10BestProducts(int storeId);
        Task<IQueryable<object>> GetPreviousMonthRevenue(int storeId);
        Task<IQueryable<object>> GetPrePreviousMonthRevenue(int storeId);
        Task<IQueryable<object>> GetGeneralInfoAbtStore(int sellerId);
        Task<IQueryable<object>> GetRawStoreTotalStatistics(int storeId);

        Task<IQueryable<object>> GetPreviousMonthRevenueGeneral(int sellerId);
        Task<IQueryable<object>> GetPrePreviousMonthRevenueGeneral(int sellerId);
        Task<IQueryable<object>> GetSalesByCategoriesGeneral(int sellerId);

        Task<IQueryable<object>> GetRawStoreTotalStatisticsGeneral(int sellerId);


    }
}

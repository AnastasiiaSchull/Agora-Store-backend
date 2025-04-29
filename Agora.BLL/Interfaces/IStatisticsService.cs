using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IStatisticsService
    {
        Task<List<WeeklyStatisticsDTO>> GetWeeksStatisticsBySales(int storeId);
        Task<List<DailyRevenueDTO>> GetPrePreviousMonthRevenue(int storeId);
        Task<List<WeeklyStatisticsDTO>> GetWeeksStatisticsBySalesGeneral(int sellerId);
        Task<List<DailyRevenueDTO>> GetPreviousMonthRevenue(int storeId);
        Task<List<TopProductDTO>> GetTop10BestProducts(int storeId);
        Task<List<GeneralInfoAbtStoreDTO>> GetGeneralIngoAbtStore(int sellerId);
        Task<List<DailyRevenueDTO>> GetPrePreviousMonthRevenueGeneral(int sellerId);
        Task<List<DailyRevenueDTO>> GetPreviousMonthRevenueGeneral(int sellerId);

        Task<StoreTotalStatisticsDTO> GetStoreTotalStatistics(int storeId);
        Task<List<CategorySalesDTO>> GetSalesByCategoriesGeneral(int sellerId);
        Task<SellerTotalStatisticsDTO> GetRawStoreTotalStatisticsGeneral(int sellerId);
    }
}

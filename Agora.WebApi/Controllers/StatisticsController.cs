using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Agora.Controllers
{
    
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IRedisSafeExecutor _redisSafe;
        private readonly IStatisticsService _statisticsService;
        private readonly ISellerService _sellerService;

        public StatisticsController(IRedisSafeExecutor redisSafe, IStatisticsService statisticsService, ISellerService sellerService)
        {
            _redisSafe = redisSafe;
            _statisticsService = statisticsService;
            _sellerService = sellerService;
        }

        [HttpGet("weekly-by-sales/{storeId}")]
        public async Task<IActionResult> GetWeekly(int storeId)
        {
            var key = $"weekly_stats:{storeId}";
            var data = await _redisSafe.GetOrSetAsync(
                key,
                () => _statisticsService.GetWeeksStatisticsBySales(storeId),
                TimeSpan.FromDays(7)
            );
            return data == null ? NotFound() : Ok(data);
        }

        [HttpGet("revenue/monthly/previous/{storeId}")]
        public async Task<IActionResult> GetPreviousMonthRevenue(int storeId)
        {
            var date = DateTime.Now.AddMonths(-1);
            var key = $"monthly_revenue:{date:yyyy-MM}:{storeId}";
            var ttl = TimeSpan.FromDays(DateTime.DaysInMonth(date.Year, date.Month));

            var data = await _redisSafe.GetOrSetAsync(
                key,
                () => _statisticsService.GetPreviousMonthRevenue(storeId),
                ttl
            );
            return data == null ? NotFound() : Ok(data);
        }

        [HttpGet("revenue/monthly/before-previous/{storeId}")]
        public async Task<IActionResult> GetBeforePreviousMonthRevenue(int storeId)
        {
            var date = DateTime.Now.AddMonths(-2);
            var key = $"monthly_revenue:{date:yyyy-MM}:{storeId}";
            var ttl = TimeSpan.FromDays(DateTime.DaysInMonth(date.Year, date.Month));

            var data = await _redisSafe.GetOrSetAsync(
                key,
                () => _statisticsService.GetPrePreviousMonthRevenue(storeId),
                ttl
            );
            return data == null ? NotFound() : Ok(data);
        }

        [HttpGet("weekly-by-sales-general/{sellerId}")]
        public async Task<IActionResult> GetWeeklyGeneral(int sellerId)
        {
            var key = $"weekly_general_stats:{sellerId}";
            var data = await _redisSafe.GetOrSetAsync(
                key,
                () => _statisticsService.GetWeeksStatisticsBySalesGeneral(sellerId),
                TimeSpan.FromDays(7)
            );
            return data == null ? NotFound() : Ok(data);            
        }

        [HttpGet("top-products/{storeId}")]
        public async Task<IActionResult> GetTopProducts(int storeId)
        {
            var key = $"top_products:{storeId}";
            var data = await _redisSafe.GetOrSetAsync(
                key,
                () => _statisticsService.GetTop10BestProducts(storeId),
                TimeSpan.FromDays(7)
            );
            return data == null ? NotFound() : Ok(data);
        }

        [HttpGet("total-statistics/{storeId}")]
        public async Task<IActionResult> GetStoreTotals(int storeId)
        {
            var key = $"store_total_stats:{storeId}";
            var data = await _redisSafe.GetOrSetAsync(
                key,
                () => _statisticsService.GetStoreTotalStatistics(storeId),
                TimeSpan.FromDays(7)
            );
            return data == null ? NotFound() : Ok(data);
        }

        [HttpGet("revenue/general-monthly/{sellerId}")]
        public async Task<IActionResult> GetMonthGeneral(int sellerId)
        {
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            var oneMonthAgo = DateTime.Now.AddMonths(-1);

            var thisKey = $"monthly_revenue_general:{oneMonthAgo:yyyy-MM}:{sellerId}";
            var lastKey = $"monthly_revenue_general:{twoMonthsAgo:yyyy-MM}:{sellerId}";

            var thisData = await _redisSafe.GetOrSetAsync(
                thisKey,
                () => _statisticsService.GetPreviousMonthRevenueGeneral(sellerId),
                TimeSpan.FromDays(DateTime.DaysInMonth(oneMonthAgo.Year, oneMonthAgo.Month))
            );

            var lastData = await _redisSafe.GetOrSetAsync(
                lastKey,
                () => _statisticsService.GetPrePreviousMonthRevenueGeneral(sellerId),
                TimeSpan.FromDays(DateTime.DaysInMonth(twoMonthsAgo.Year, twoMonthsAgo.Month))
            );

            if (thisData == null || lastData == null)
                return NotFound();

            var result = thisData.Join(lastData, a => a.Date.Day, b => b.Date.Day, (a, b) =>
                new MonthlyRevenueDTO
                {
                    Date = a.Date.ToString("dd-MM"),
                    ThisMonth = a.Revenue,
                    LastMonth = b.Revenue
                }).ToList();

            return Ok(result);
        }

        [HttpGet("revenue/general-monthly/store/{storeId}")]
        public async Task<IActionResult> GetMonthGeneralStore(int storeId)
        {
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            var oneMonthAgo = DateTime.Now.AddMonths(-1);

            var thisKey = $"monthly_revenue:{oneMonthAgo:yyyy-MM}:{storeId}";
            var lastKey = $"monthly_revenue:{twoMonthsAgo:yyyy-MM}:{storeId}";

            var thisData = await _redisSafe.GetOrSetAsync(
                thisKey,
                () => _statisticsService.GetPreviousMonthRevenue(storeId)
            );

            var lastData = await _redisSafe.GetOrSetAsync(
                lastKey,
                () => _statisticsService.GetPrePreviousMonthRevenue(storeId)
            );

            if (thisData == null || lastData == null)
                return NotFound();

            var result = thisData.Join(lastData, a => a.Date.Day, b => b.Date.Day, (a, b) =>
                new MonthlyRevenueDTO
                {
                    Date = a.Date.ToString("dd-MM"),
                    ThisMonth = a.Revenue,
                    LastMonth = b.Revenue
                }).ToList();

            return Ok(result);
        }

        [HttpGet("category-sales/{sellerId}")]
        public async Task<IActionResult> GetSalesByCategory(int sellerId)
        {
            var key = $"sales_by_category_stats:{sellerId}";
            var data = await _redisSafe.GetOrSetAsync(
                key,
                () => _statisticsService.GetSalesByCategoriesGeneral(sellerId),
                TimeSpan.FromDays(7)
            );
            return data == null ? NotFound() : Ok(data);
        }

        [HttpGet("total-statistics-general/{sellerId}")]
        public async Task<IActionResult> GetSellerTotalStatistics(int sellerId)
        {
            var totalStats = await _statisticsService.GetRawStoreTotalStatisticsGeneral(sellerId);
            if (totalStats == null)
                return StatusCode(500, new { message = "Server error!" });

            var seller = await _sellerService.Get(sellerId);
            totalStats.Rating = seller.Rating;

            return Ok(totalStats);
        }

        [HttpGet("info-abt-stores/{sellerId}")]
        public async Task<ActionResult<List<GeneralInfoAbtStoreDTO>>> GetInfoAbtStores(int sellerId)
        {
            var list = await _statisticsService.GetGeneralIngoAbtStore(sellerId);
            return list;
        }
    }

}

using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.BLL.Services;
using Agora.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StackExchange.Redis;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Agora.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IStatisticsService _statisticsService;
        private readonly ISellerService _sellerService;

        public StatisticsController(IConnectionMultiplexer redis, IStatisticsService statisticsService, ISellerService sellerService)
        {
            _redis = redis;
            _statisticsService = statisticsService;
            _sellerService = sellerService;
        }

        [HttpGet("weekly-by-sales/{storeId}")]
        public async Task<IActionResult> GetWeekly(int storeId)
        {
            var db = _redis.GetDatabase();
            var json = await db.StringGetAsync($"weekly_stats:{storeId}");

            if (json.IsNullOrEmpty)
                return NotFound("Statistics was not found or has not yet been calculated.");

            var data = JsonSerializer.Deserialize<List<WeeklyStatisticsDTO>>(json);
            return Ok(data);
        }

        [HttpGet("revenue/monthly/previous/{storeId}")]
        public async Task<IActionResult> GetPreviousMonthRevenue(int storeId)
        {
            var db = _redis.GetDatabase();
            var date = DateTime.Now.AddMonths(-1);
            var key = $"monthly_revenue:{date.Year}-{date.Month:D2}:{storeId}";

            var json = await db.StringGetAsync(key);
            if (json.IsNullOrEmpty)
                return NotFound("Revenue data for the previous month was not found or has not yet been calculated.");

            var data = JsonSerializer.Deserialize<List<DailyRevenueDTO>>(json);
            return Ok(data);
        }

        [HttpGet("revenue/monthly/before-previous/{storeId}")]
        public async Task<IActionResult> GetBeforePreviousMonthRevenue(int storeId)
        {
            var db = _redis.GetDatabase();
            var date = DateTime.Now.AddMonths(-2);
            var key = $"monthly_revenue:{date.Year}-{date.Month:D2}:{storeId}";

            var json = await db.StringGetAsync(key);
            if (json.IsNullOrEmpty)
                return NotFound("Revenue data for the preprevious month was not found or has not yet been calculated.");

            var data = JsonSerializer.Deserialize<List<DailyRevenueDTO>>(json);
            return Ok(data);
        }

        [HttpGet("info-abt-stores/{sellerId}")]
        public async Task<ActionResult<List<GeneralInfoAbtStoreDTO>>> GetInfoAbtStores(int sellerId)
        {
            List<GeneralInfoAbtStoreDTO> list = await _statisticsService.GetGeneralIngoAbtStore(sellerId);
            return list;
        }


        [HttpGet("weekly-by-sales-general/{sellerId}")]
        public async Task<IActionResult> GetWeeklyGeneral(int sellerId)
        {
            var db = _redis.GetDatabase();
            var json = await db.StringGetAsync($"weekly_general_stats:{sellerId}");

            if (json.IsNullOrEmpty)
                return NotFound("Statistics was not found or has not yet been calculated.");

            var data = JsonSerializer.Deserialize<List<WeeklyStatisticsDTO>>(json);
            return Ok(data);
        }

        [HttpGet("top-products/{storeId}")]
        public async Task<IActionResult> GetTopProducts(int storeId)
        {
            var db = _redis.GetDatabase();
            var json = await db.StringGetAsync($"top_products:{storeId}");

            if (json.IsNullOrEmpty)
                return NotFound("Top products were not found or have not yet been calculated.");

            var data = JsonSerializer.Deserialize<List<TopProductDTO>>(json);
            return Ok(data);
        }

        [HttpGet("total-statistics/{storeId}")]
        public async Task<IActionResult> GetStoreTotals(int storeId)
        {
            var db = _redis.GetDatabase();
            var json = await db.StringGetAsync($"store_total_stats:{storeId}");

            if (json.IsNullOrEmpty)
                return NotFound("Store totals data not found or not yet cached.");

            var data = JsonSerializer.Deserialize<StoreTotalStatisticsDTO>(json);
            return Ok(data);
        }


        [HttpGet("revenue/general-monthly/{sellerId}")]
        public async Task<IActionResult> GetMonthGeneral(int sellerId)
        {
            var db = _redis.GetDatabase();
            var twoMonthAgo = DateTime.Now.AddMonths(-2);
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var thisMonthKey = $"monthly_revenue_general:{oneMonthAgo.Year}-{oneMonthAgo.Month:D2}:{sellerId}";
            var lastMonthKey = $"monthly_revenue_general:{twoMonthAgo.Year}-{twoMonthAgo.Month:D2}:{sellerId}";
            var thisMonthJson = await db.StringGetAsync(thisMonthKey);
            var lastMonthJson = await db.StringGetAsync(lastMonthKey);
            var thisMonthData = JsonSerializer.Deserialize<List<DailyRevenueDTO>>(thisMonthJson);
            var lastMonthData = JsonSerializer.Deserialize<List<DailyRevenueDTO>>(lastMonthJson);
            List<MonthlyRevenueDTO> list = new List<MonthlyRevenueDTO>();
            foreach (var thisMonth in thisMonthData)
            {
                foreach(var lastMonth in lastMonthData)
                {
                    if (thisMonth.Date.Day.Equals(lastMonth.Date.Day))
                    {
                        MonthlyRevenueDTO revenueDTO = new MonthlyRevenueDTO();
                        var date = thisMonth.Date;
                        revenueDTO.Date = date.ToString("dd-MM");
                        revenueDTO.ThisMonth = thisMonth.Revenue;
                        revenueDTO.LastMonth = lastMonth.Revenue;
                        list.Add(revenueDTO);
                    }

                }
            }
                return Ok(list);


        }
        [HttpGet("revenue/general-monthly/store/{storeId}")]
        public async Task<IActionResult> GetMonthGeneralStore(int storeId)
        {
            var db = _redis.GetDatabase();
            var twoMonthAgo = DateTime.Now.AddMonths(-2);
            var oneMonthAgo = DateTime.Now.AddMonths(-1);

            var thisMonthKey = $"monthly_revenue:{oneMonthAgo.Year}-{oneMonthAgo.Month:D2}:{storeId}";
            var lastMonthKey = $"monthly_revenue:{twoMonthAgo.Year}-{twoMonthAgo.Month:D2}:{storeId}";

            var thisMonthJson = await db.StringGetAsync(thisMonthKey);
            var lastMonthJson = await db.StringGetAsync(lastMonthKey);

            if (thisMonthJson.IsNullOrEmpty || lastMonthJson.IsNullOrEmpty)
            {
                return NotFound("Revenue data not found for one or both months.");
            }

            var thisMonthData = JsonSerializer.Deserialize<List<DailyRevenueDTO>>(thisMonthJson);
            var lastMonthData = JsonSerializer.Deserialize<List<DailyRevenueDTO>>(lastMonthJson);

            List<MonthlyRevenueDTO> list = new List<MonthlyRevenueDTO>();

            foreach (var thisDay in thisMonthData)
            {
                var matchingDay = lastMonthData.FirstOrDefault(x => x.Date.Day == thisDay.Date.Day);
                if (matchingDay != null)
                {
                    MonthlyRevenueDTO revenueDTO = new MonthlyRevenueDTO
                    {
                        Date = thisDay.Date.ToString("dd-MM"),
                        ThisMonth = thisDay.Revenue,
                        LastMonth = matchingDay.Revenue
                    };
                    list.Add(revenueDTO);
                }
            }

            return Ok(list);
        }



        [HttpGet("category-sales/{sellerId}")]
        public async Task<IActionResult> GetSalesByCategory(int sellerId)
        {
            var db = _redis.GetDatabase();
            var json = await db.StringGetAsync($"sales_by_category_stats:{sellerId}");

            if (json.IsNullOrEmpty)
                return NotFound("Sales By Catogory were not found or have not yet been calculated.");

            var data = JsonSerializer.Deserialize<List<CategorySalesDTO>>(json);
            return Ok(data);
        }

        [HttpGet("total-statistics-general/{sellerId}")]
        public async Task<IActionResult> GetSellerTotalStatistics(int sellerId  )
        {
            SellerTotalStatisticsDTO  totalStatisticsGeneral = await _statisticsService.GetRawStoreTotalStatisticsGeneral(sellerId);
            if(totalStatisticsGeneral == null)
                return new JsonResult(new { message = "Server error!" }) { StatusCode = 500 };
            SellerDTO seller = await _sellerService.Get(sellerId);
            totalStatisticsGeneral.Rating = seller.Rating;
            return Ok(totalStatisticsGeneral);
        }
    }
}

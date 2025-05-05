using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System.Text.Json;


namespace Agora.BLL.Services
{
    public class StatisticsCacheService : IHostedService, IDisposable, IStatisticsInitializer
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConnectionMultiplexer _redis;

        public StatisticsCacheService(IServiceScopeFactory scopeFactory, IConnectionMultiplexer redis)
        {
            _scopeFactory = scopeFactory;
            _redis = redis;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateRedisCache, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }
        
        private static bool _firstRun = true;

        private async void UpdateRedisCache(object state)
        {
            Console.WriteLine("UpdateRedisCache started...");
            using var scope = _scopeFactory.CreateScope();
            var statsService = scope.ServiceProvider.GetRequiredService<IStatisticsService>();
            var db = _redis.GetDatabase();
            var today = DateOnly.FromDateTime(DateTime.Now);
            var storeService = scope.ServiceProvider.GetRequiredService<IStoreService>();
            var storeIds = await storeService.GetAllStoreIds();
            
            // set cache for all stores in db:
            foreach (var storeId in storeIds)
            {
                
                // UpdateRedisCache for first run or 1 time for week
                if (_firstRun || today.DayOfWeek == DayOfWeek.Monday)
                {
                    await CacheWeeklySalesAsync(db, statsService, storeId);
                    await CacheTop10BestSellersAsync(db, statsService, storeId);
                }

                // UpdateRedisCache for first run or 1 time for month
                if (_firstRun || today.Day == 1)
                {
                    await CachePreviousMonthRevenueAsync(db, statsService, storeId);
                    await CachePrePreviousMonthRevenueAsync(db, statsService, storeId);
                }
                // UpdateRedisCache 1 time for day
                await CacheStoreTotalStatisticsAsync(db, statsService, storeId);
            }

            if (_firstRun || today.DayOfWeek == DayOfWeek.Monday)
            {                
                var sellerService = scope.ServiceProvider.GetRequiredService<ISellerService>();
                var sellerIds = await sellerService.GetAllSellerIds();
                foreach(var sellerId in sellerIds)
                {
                    await CacheWeeklySalesGeneralAsync(db, statsService, sellerId);
                    await CacheSalesByCategory(db, statsService, sellerId);
                }
            }
            if (_firstRun || today.Day == 1)
            {
                var sellerService = scope.ServiceProvider.GetRequiredService<ISellerService>();
                var sellerIds = await sellerService.GetAllSellerIds();
                foreach (var sellerId in sellerIds)
                {
                    await CachePreviousMonthRevenueGeneralAsync(db, statsService, sellerId);
                    await CachePrePreviousMonthRevenueGeneralAsync(db, statsService, sellerId);
                }
                    
            }

            _firstRun = false;
        }

        private async Task CachePreviousMonthRevenueAsync(IDatabase db, IStatisticsService statsService, int storeId)
        {
            var date = DateTime.Now.AddMonths(-1);
            var key = $"monthly_revenue:{date.Year}-{date.Month:D2}:{storeId}";

            var revenue = await statsService.GetPreviousMonthRevenue(storeId);
            var json = JsonSerializer.Serialize(revenue);

            await db.StringSetAsync(key, json, GetCacheLifetimeForMonth(date));
        }
        private async Task CachePrePreviousMonthRevenueAsync(IDatabase db, IStatisticsService statsService, int storeId)
        {
            var date = DateTime.Now.AddMonths(-2);
            var key = $"monthly_revenue:{date.Year}-{date.Month:D2}:{storeId}";

            var revenue = await statsService.GetPrePreviousMonthRevenue(storeId);
            var json = JsonSerializer.Serialize(revenue);
            // GetCacheLifetimeForMonth(date) - lifetime of cache in Redis (28, 29, 30 or 31 days) 
            await db.StringSetAsync(key, json, GetCacheLifetimeForMonth(date));
        }
        private async Task CacheWeeklySalesAsync(IDatabase db, IStatisticsService statsService, int storeId)
        {
            var weeklyStats = await statsService.GetWeeksStatisticsBySales(storeId);
            var json = JsonSerializer.Serialize(weeklyStats);
            // 7 - lifetime of cache in Redis for  weekly stetistics
            await db.StringSetAsync($"weekly_stats:{storeId}", json, TimeSpan.FromDays(7));
        }
        private async Task CacheWeeklySalesGeneralAsync(IDatabase db, IStatisticsService statsService, int sellerId)
        {
            var weeklyStatsGeneral = await statsService.GetWeeksStatisticsBySalesGeneral(sellerId);
            var json = JsonSerializer.Serialize(weeklyStatsGeneral);
            // 7 - lifetime of cache in Redis for  weekly stetistics
            await db.StringSetAsync($"weekly_general_stats:{sellerId}", json, TimeSpan.FromDays(7));
        }

        private async Task CachePreviousMonthRevenueGeneralAsync(IDatabase db, IStatisticsService statsService, int sellerId)
        {
            var date = DateTime.Now.AddMonths(-1);
            var key = $"monthly_revenue_general:{date.Year}-{date.Month:D2}:{sellerId}";

            var revenue = await statsService.GetPreviousMonthRevenueGeneral(sellerId);
            var json = JsonSerializer.Serialize(revenue);

            await db.StringSetAsync(key, json, GetCacheLifetimeForMonth(date));
        }
        private async Task CachePrePreviousMonthRevenueGeneralAsync(IDatabase db, IStatisticsService statsService, int sellerId)
        {
            var date = DateTime.Now.AddMonths(-2);
            var key = $"monthly_revenue_general:{date.Year}-{date.Month:D2}:{sellerId}";
            var revenue = await statsService.GetPrePreviousMonthRevenueGeneral(sellerId);
            var json = JsonSerializer.Serialize(revenue);
            // GetCacheLifetimeForMonth(date) - lifetime of cache in Redis (28, 29, 30 or 31 days) 
            await db.StringSetAsync(key, json, GetCacheLifetimeForMonth(date));
        }
        private async Task CacheTop10BestSellersAsync(IDatabase db, IStatisticsService statsService, int storeId)
        {
            var topProducts = await statsService.GetTop10BestProducts(storeId);
            var json = JsonSerializer.Serialize(topProducts);
            await db.StringSetAsync($"top_products:{storeId}", json, TimeSpan.FromDays(7));
        }

        private async Task CacheStoreTotalStatisticsAsync(IDatabase db, IStatisticsService statsService, int storeId)
        {
            var stats = await statsService.GetStoreTotalStatistics(storeId);
            var json = JsonSerializer.Serialize(stats);
            await db.StringSetAsync($"store_total_stats:{storeId}", json, TimeSpan.FromDays(7));
        }

        private async Task CacheSalesByCategory(IDatabase db, IStatisticsService statsService, int sellerId)
        {
            var sales = await statsService.GetSalesByCategoriesGeneral(sellerId);
            var json = JsonSerializer.Serialize(sales);
            // 7 - lifetime of cache in Redis for  weekly stetistics
            await db.StringSetAsync($"sales_by_category_stats:{sellerId}", json, TimeSpan.FromDays(7));
        }
        private TimeSpan GetCacheLifetimeForMonth(DateTime date)
        {
            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            return TimeSpan.FromDays(daysInMonth);
        }

        public async Task InitializeEmptyStatsForStore(int storeId)
        {
            var db = _redis.GetDatabase();

            await db.StringSetAsync($"weekly_stats:{storeId}", "[]");
            await db.StringSetAsync($"top_products:{storeId}", "[]");
            await db.StringSetAsync($"store_total_stats:{storeId}", JsonSerializer.Serialize(new StoreTotalStatisticsDTO()));

            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            var oneMonthAgo = DateTime.Now.AddMonths(-1);

            var lastMonthKey = $"monthly_revenue:{twoMonthsAgo.Year}-{twoMonthsAgo.Month:D2}:{storeId}";
            var thisMonthKey = $"monthly_revenue:{oneMonthAgo.Year}-{oneMonthAgo.Month:D2}:{storeId}";

            var emptyListJson = JsonSerializer.Serialize(new List<DailyRevenueDTO>());

            await db.StringSetAsync(lastMonthKey, emptyListJson);
            await db.StringSetAsync(thisMonthKey, emptyListJson);
        }
        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose() => _timer?.Dispose();
    }
}

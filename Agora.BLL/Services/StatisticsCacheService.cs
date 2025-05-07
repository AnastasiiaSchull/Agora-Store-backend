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
        private readonly IRedisSafeExecutor _redisSafe;

        public StatisticsCacheService(IServiceScopeFactory scopeFactory, IRedisSafeExecutor redisSafe)
        {
            _scopeFactory = scopeFactory;
            _redisSafe = redisSafe;
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
            var db = _redisSafe.TryGetDatabase();
            if (db == null)
            {
                Console.WriteLine("Redis not available. Caching skipped..");
                return;
            }

            var today = DateOnly.FromDateTime(DateTime.Now);
            var storeService = scope.ServiceProvider.GetRequiredService<IStoreService>();
            var storeIds = await storeService.GetAllStoreIds();

            foreach (var storeId in storeIds)
            {
                if (_firstRun || today.DayOfWeek == DayOfWeek.Monday)
                {
                    await CacheWeeklySalesAsync(statsService, storeId);
                    await CacheTop10BestSellersAsync(statsService, storeId);
                }

                if (_firstRun || today.Day == 1)
                {
                    await CachePreviousMonthRevenueAsync(statsService, storeId);
                    await CachePrePreviousMonthRevenueAsync(statsService, storeId);
                }

                await CacheStoreTotalStatisticsAsync(statsService, storeId);
            }

            if (_firstRun || today.DayOfWeek == DayOfWeek.Monday)
            {
                var sellerService = scope.ServiceProvider.GetRequiredService<ISellerService>();
                var sellerIds = await sellerService.GetAllSellerIds();
                foreach (var sellerId in sellerIds)
                {
                    await CacheWeeklySalesGeneralAsync(statsService, sellerId);
                    await CacheSalesByCategory(statsService, sellerId);
                }
            }

            if (_firstRun || today.Day == 1)
            {
                var sellerService = scope.ServiceProvider.GetRequiredService<ISellerService>();
                var sellerIds = await sellerService.GetAllSellerIds();
                foreach (var sellerId in sellerIds)
                {
                    await CachePreviousMonthRevenueGeneralAsync(statsService, sellerId);
                    await CachePrePreviousMonthRevenueGeneralAsync(statsService, sellerId);
                }
            }

            _firstRun = false;
        }

        private async Task CachePreviousMonthRevenueAsync(IStatisticsService statsService, int storeId)
        {
            var date = DateTime.Now.AddMonths(-1);
            var key = $"monthly_revenue:{date.Year}-{date.Month:D2}:{storeId}";
            var revenue = await statsService.GetPreviousMonthRevenue(storeId);
            var json = JsonSerializer.Serialize(revenue);
            await _redisSafe.SafeSetStringAsync(key, json, GetCacheLifetimeForMonth(date));
        }

        private async Task CachePrePreviousMonthRevenueAsync(IStatisticsService statsService, int storeId)
        {
            var date = DateTime.Now.AddMonths(-2);
            var key = $"monthly_revenue:{date.Year}-{date.Month:D2}:{storeId}";
            var revenue = await statsService.GetPrePreviousMonthRevenue(storeId);
            var json = JsonSerializer.Serialize(revenue);
            await _redisSafe.SafeSetStringAsync(key, json, GetCacheLifetimeForMonth(date));
        }

        private async Task CacheWeeklySalesAsync(IStatisticsService statsService, int storeId)
        {
            var weeklyStats = await statsService.GetWeeksStatisticsBySales(storeId);
            var json = JsonSerializer.Serialize(weeklyStats);
            await _redisSafe.SafeSetStringAsync($"weekly_stats:{storeId}", json, TimeSpan.FromDays(7));
        }

        private async Task CacheWeeklySalesGeneralAsync(IStatisticsService statsService, int sellerId)
        {
            var weeklyStatsGeneral = await statsService.GetWeeksStatisticsBySalesGeneral(sellerId);
            var json = JsonSerializer.Serialize(weeklyStatsGeneral);
            await _redisSafe.SafeSetStringAsync($"weekly_general_stats:{sellerId}", json, TimeSpan.FromDays(7));
        }

        private async Task CachePreviousMonthRevenueGeneralAsync(IStatisticsService statsService, int sellerId)
        {
            var date = DateTime.Now.AddMonths(-1);
            var key = $"monthly_revenue_general:{date.Year}-{date.Month:D2}:{sellerId}";
            var revenue = await statsService.GetPreviousMonthRevenueGeneral(sellerId);
            var json = JsonSerializer.Serialize(revenue);
            await _redisSafe.SafeSetStringAsync(key, json, GetCacheLifetimeForMonth(date));
        }

        private async Task CachePrePreviousMonthRevenueGeneralAsync(IStatisticsService statsService, int sellerId)
        {
            var date = DateTime.Now.AddMonths(-2);
            var key = $"monthly_revenue_general:{date.Year}-{date.Month:D2}:{sellerId}";
            var revenue = await statsService.GetPrePreviousMonthRevenueGeneral(sellerId);
            var json = JsonSerializer.Serialize(revenue);
            await _redisSafe.SafeSetStringAsync(key, json, GetCacheLifetimeForMonth(date));
        }

        private async Task CacheTop10BestSellersAsync(IStatisticsService statsService, int storeId)
        {
            var topProducts = await statsService.GetTop10BestProducts(storeId);
            var json = JsonSerializer.Serialize(topProducts);
            await _redisSafe.SafeSetStringAsync($"top_products:{storeId}", json, TimeSpan.FromDays(7));
        }

        private async Task CacheStoreTotalStatisticsAsync(IStatisticsService statsService, int storeId)
        {
            var stats = await statsService.GetStoreTotalStatistics(storeId);
            var json = JsonSerializer.Serialize(stats);
            await _redisSafe.SafeSetStringAsync($"store_total_stats:{storeId}", json, TimeSpan.FromDays(7));
        }

        private async Task CacheSalesByCategory(IStatisticsService statsService, int sellerId)
        {
            var sales = await statsService.GetSalesByCategoriesGeneral(sellerId);
            var json = JsonSerializer.Serialize(sales);
            await _redisSafe.SafeSetStringAsync($"sales_by_category_stats:{sellerId}", json, TimeSpan.FromDays(7));
        }

        private TimeSpan GetCacheLifetimeForMonth(DateTime date)
        {
            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            return TimeSpan.FromDays(daysInMonth);
        }

        public async Task InitializeEmptyStatsForStore(int storeId)
        {
            await _redisSafe.SafeSetStringAsync($"weekly_stats:{storeId}", "[]", TimeSpan.FromDays(7));
            await _redisSafe.SafeSetStringAsync($"top_products:{storeId}", "[]", TimeSpan.FromDays(7));
            await _redisSafe.SafeSetStringAsync($"store_total_stats:{storeId}", JsonSerializer.Serialize(new StoreTotalStatisticsDTO()), TimeSpan.FromDays(7));

            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var emptyListJson = JsonSerializer.Serialize(new List<DailyRevenueDTO>());

            await _redisSafe.SafeSetStringAsync($"monthly_revenue:{twoMonthsAgo.Year}-{twoMonthsAgo.Month:D2}:{storeId}", emptyListJson, GetCacheLifetimeForMonth(twoMonthsAgo));
            await _redisSafe.SafeSetStringAsync($"monthly_revenue:{oneMonthAgo.Year}-{oneMonthAgo.Month:D2}:{storeId}", emptyListJson, GetCacheLifetimeForMonth(oneMonthAgo));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose() => _timer?.Dispose();
    }

}

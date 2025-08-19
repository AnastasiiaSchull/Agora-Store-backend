using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;

namespace Agora.DAL.Repository
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private AgoraContext db;
        public StatisticsRepository(AgoraContext context)
        {
            this.db = context;
        }

        public Task<IQueryable<object>> GetWeeksStatisticsBySales(int storeId)
        {
            Console.WriteLine("!!! Check work of  UpdateRedisCache - WeeksStatisticsBySales for store: " + storeId);

            var dateNow = DateOnly.FromDateTime(DateTime.Now);
           
            var weekAgo = dateNow.AddDays(-7);
            
            var query = db.OrderItems
                .Where(o => o.Product.Store.Id == storeId)
                .Where(o => o.Status == Enums.OrderStatus.Completed)
                .Where(o => o.Date >= weekAgo)
                .Select(o => new
                {
                    o.Date,
                    o.Quantity
                   
                });
            
            return Task.FromResult(query.Cast<object>());
        }

        public Task<IQueryable<object>> GetWeeksStatisticsBySalesGeneral(int sellerId)
        {
            var dateNow = DateOnly.FromDateTime(DateTime.Now);

            var weekAgo = dateNow.AddDays(-7);

            var query = db.OrderItems
                .Where(o => o.Product.Store.SellerId == sellerId )
                .Where(o => o.Status == Enums.OrderStatus.Completed)
                .Where(o => o.Date >= weekAgo)
                .Select(o => new
                {
                    o.Date,
                    o.Quantity

                });

            return Task.FromResult(query.Cast<object>());
        }
        public Task<IQueryable<object>> GetPreviousMonthRevenue(int storeId)
        {
            Console.WriteLine("!!! Check work of  UpdateRedisCache - PreviousMonthRevenue for store: " + storeId);

            var now = DateTime.Now;
            var firstDayOfThisMonth = new DateOnly(now.Year, now.Month, 1);
            var firstDayOfPreviousMonth = firstDayOfThisMonth.AddMonths(-1);
            var lastDayOfPreviousMonth = firstDayOfThisMonth.AddDays(-1);

            var query = db.OrderItems
                .Where(o => o.Product.Store.Id == storeId)
                .Where(o => o.Status == Enums.OrderStatus.Completed)
                .Where(o => o.Date >= firstDayOfPreviousMonth && o.Date <= lastDayOfPreviousMonth)
                .Select(o => new
                {
                    o.Date,
                    Revenue = o.PriceAtMoment * o.Quantity
                });

            return Task.FromResult(query.Cast<object>());
        }
        public Task<IQueryable<object>> GetPrePreviousMonthRevenue(int storeId)
        {
            Console.WriteLine("!!! Check work of  UpdateRedisCache - PrePreviousMonthRevenue for store: " + storeId);

            var now = DateTime.Now;
            var firstDayOfThisMonth = new DateOnly(now.Year, now.Month, 1);
            var firstDayOfPrePreviousMonth = firstDayOfThisMonth.AddMonths(-2);
            var firstDayOfPreviousMonth = firstDayOfThisMonth.AddMonths(-1);
            var lastDayOfPrePreviousMonth = firstDayOfPreviousMonth.AddDays(-1);

            var query = db.OrderItems
                .Where(o => o.Product.Store.Id == storeId)
                .Where(o => o.Status == Enums.OrderStatus.Completed)
                .Where(o => o.Date >= firstDayOfPrePreviousMonth && o.Date <= lastDayOfPrePreviousMonth)
                .Select(o => new
                {
                    o.Date,
                    Revenue = o.PriceAtMoment * o.Quantity
                });

            return Task.FromResult(query.Cast<object>());
        }

        public Task<IQueryable<object>> GetGeneralInfoAbtStore(int sellerId)
        {
            var query = db.OrderItems
                .Where(o => o.Product.Store.SellerId == sellerId)
                .Where(o => o.Status == Enums.OrderStatus.Completed)
                .Select(o => new
                {
                    o.PriceAtMoment,
                    o.Product.Store.Name

                });
           
            return Task.FromResult(query.Cast<object>());
        }

        // top 10 products for the week
        public Task<IQueryable<object>> GetTop10BestProducts(int storeId)
        {
            //var today = DateTime.Today;
            //var monthAgoDate = today.AddMonths(-1); 
            //var monthAgo = DateOnly.FromDateTime(monthAgoDate);
            var dateNow = DateOnly.FromDateTime(DateTime.Now);
            var weekAgo = dateNow.AddDays(-7);

            var query = db.OrderItems
                .Where(o => o.Product.Store.Id == storeId)
                .Where(o => o.Status == Enums.OrderStatus.Completed)
                .Where(o => o.Date >= weekAgo)
                .Select(o => new
                {
                    o.Date,
                    o.Quantity,
                    ProductId = o.Product.Id,
                    ProductName = o.Product.Name
                });

            return Task.FromResult(query.Cast<object>());
        }

        // for total statistics by store
        public Task<IQueryable<object>> GetRawStoreTotalStatistics(int storeId)
        {
            var query = db.OrderItems
                .Where(o => o.Product.Store.Id == storeId)
                .Where(o => o.Status == Enums.OrderStatus.Completed)
                .Select(o => new
                {
                    o.Quantity,
                    Revenue = o.PriceAtMoment * o.Quantity,
                    CustomerId = o.Order.Customer.Id
                });

            return Task.FromResult(query.Cast<object>());
        }

        public Task<IQueryable<object>> GetPreviousMonthRevenueGeneral(int sellerId)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("in");
            var now = DateTime.Now;
            var firstDayOfThisMonth = new DateOnly(now.Year, now.Month, 1);
            var firstDayOfPreviousMonth = firstDayOfThisMonth.AddMonths(-1);
            var lastDayOfPreviousMonth = firstDayOfThisMonth.AddDays(-1);

            var query = db.OrderItems
                .Where(o => o.Product.Store.SellerId == sellerId)
                .Where(o => o.Status == Enums.OrderStatus.Completed)
                .Where(o => o.Date >= firstDayOfPreviousMonth && o.Date <= lastDayOfPreviousMonth)
                .Select(o => new
                {
                    o.Date,
                    Revenue = o.PriceAtMoment * o.Quantity
                });

            return Task.FromResult(query.Cast<object>());
        }
        public Task<IQueryable<object>> GetPrePreviousMonthRevenueGeneral(int sellerId)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("in");
            var now = DateTime.Now;
            var firstDayOfThisMonth = new DateOnly(now.Year, now.Month, 1);
            var firstDayOfPrePreviousMonth = firstDayOfThisMonth.AddMonths(-2);
            var firstDayOfPreviousMonth = firstDayOfThisMonth.AddMonths(-1);
            var lastDayOfPrePreviousMonth = firstDayOfPreviousMonth.AddDays(-1);

            var query = db.OrderItems
                .Where(o => o.Product.Store.SellerId == sellerId)
                .Where(o => o.Status == Enums.OrderStatus.Completed)
                .Where(o => o.Date >= firstDayOfPrePreviousMonth && o.Date <= lastDayOfPrePreviousMonth)
                .Select(o => new
                {
                    o.Date,
                    Revenue = o.PriceAtMoment * o.Quantity
                });

            return Task.FromResult(query.Cast<object>());
        }
        public Task<IQueryable<object>> GetSalesByCategoriesGeneral(int sellerId)
        {
            var dateNow = DateOnly.FromDateTime(DateTime.Now);

            var weekAgo = dateNow.AddDays(-7);

            var query = db.OrderItems
                .Where(o => o.Product.Store.SellerId == sellerId)
                .Where(o => o.Status == Enums.OrderStatus.Completed)
                .Where(o => o.Date >= weekAgo)
                .Select(o => new
                {
                    o.Product.Category.Name,
                    o.Quantity

                });

            return Task.FromResult(query.Cast<object>());

        }

        public Task<IQueryable<object>> GetRawStoreTotalStatisticsGeneral(int sellerId)
        {
            var query = db.OrderItems
                .Where(o => o.Product.Store.SellerId == sellerId)
                .Where(o => o.Status == Enums.OrderStatus.Completed)
                .Select(o => new
                {
                    
                    Revenue = o.PriceAtMoment * o.Quantity,
                    CustomerId = o.Order.Customer.Id,

                });

            return Task.FromResult(query.Cast<object>());
        }
    }
}

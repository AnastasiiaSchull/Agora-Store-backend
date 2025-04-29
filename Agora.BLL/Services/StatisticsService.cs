using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using AutoMapper;

namespace Agora.BLL.Services
{
    public class StatisticsService : IStatisticsService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;
        IProductService _productService;
        public StatisticsService(IUnitOfWork database, IMapper mapper, IProductService productService)
        {
            Database = database;
            _mapper = mapper;
            _productService = productService;
        }
        public async Task<List<WeeklyStatisticsDTO>> GetWeeksStatisticsBySales(int storeId)
        {
            IQueryable<object> objects = await Database.Statistics.GetWeeksStatisticsBySales(storeId);
            List<WeeklyStatisticsDTO> list = new List<WeeklyStatisticsDTO>();

            foreach (var item in objects)
            {
                WeeklyStatisticsDTO dto = new WeeklyStatisticsDTO();
                var dateProperty = item.GetType().GetProperty("Date");
                var quantityProperty = item.GetType().GetProperty("Quantity");

                if (dateProperty != null && quantityProperty != null)
                {
                    dto.Date = (DateOnly)dateProperty.GetValue(item);
                    dto.QuantityOfSales = (int)quantityProperty.GetValue(item);
                }

                list.Add(dto);
            }


            var statisticsWithSummedQuantity = GetStatisticsWithSummedQuantity(list);

            return statisticsWithSummedQuantity;
        }

        public async Task<List<WeeklyStatisticsDTO>> GetWeeksStatisticsBySalesGeneral(int sellerId)
        {
            IQueryable<object> objects = await Database.Statistics.GetWeeksStatisticsBySalesGeneral(sellerId);
            List<WeeklyStatisticsDTO> list = new List<WeeklyStatisticsDTO>();

            foreach (var item in objects)
            {
                WeeklyStatisticsDTO dto = new WeeklyStatisticsDTO();
                var dateProperty = item.GetType().GetProperty("Date");
                var quantityProperty = item.GetType().GetProperty("Quantity");

                if (dateProperty != null && quantityProperty != null)
                {
                    dto.Date = (DateOnly)dateProperty.GetValue(item);
                    dto.QuantityOfSales = (int)quantityProperty.GetValue(item);
                }

                list.Add(dto);
            }


            var statisticsWithSummedQuantity = GetStatisticsWithSummedQuantity(list);

            return statisticsWithSummedQuantity;
        }
        public List<WeeklyStatisticsDTO> GetStatisticsWithSummedQuantity(List<WeeklyStatisticsDTO> statistics)
        {
            var groupedByDate = statistics
                .GroupBy(s => s.Date)
                .Where(group => group.Count() >= 1)
                .Select(group => new WeeklyStatisticsDTO
                {
                    Date = group.Key,
                    DayOfWeek = group.Key.ToString("ddd", new CultureInfo("en-EN")),
                    QuantityOfSales = group.Sum(s => s.QuantityOfSales)
                })
                .ToList();

            return groupedByDate;
        }

        public async Task<List<DailyRevenueDTO>> GetPrePreviousMonthRevenue(int storeId)
        {
            IQueryable<object> objects = await Database.Statistics.GetPrePreviousMonthRevenue(storeId);
            return GetStatisticsWithSummedRevenues(objects);
        }

        public async Task<List<DailyRevenueDTO>> GetPreviousMonthRevenue(int storeId)
        {
            IQueryable<object> objects = await Database.Statistics.GetPreviousMonthRevenue(storeId);
            return GetStatisticsWithSummedRevenues(objects);
        }

        private List<DailyRevenueDTO> GetStatisticsWithSummedRevenues(IQueryable<object> objects)
        {
            var revenueList = new List<DailyRevenueDTO>();

            foreach (var item in objects)
            {
                var dateProperty = item.GetType().GetProperty("Date");
                var revenueProperty = item.GetType().GetProperty("Revenue");

                if (dateProperty != null && revenueProperty != null)
                {
                    var dto = new DailyRevenueDTO
                    {
                        Date = (DateOnly)dateProperty.GetValue(item),
                        Revenue = (decimal)revenueProperty.GetValue(item),
                    };
                    dto.DayOfWeek = dto.Date.DayOfWeek.ToString();

                    revenueList.Add(dto);
                }
            }

            var grouped = revenueList
                .GroupBy(r => r.Date)
                .Select(g => new DailyRevenueDTO
                {
                    Date = g.Key,
                    DayOfWeek = g.Key.DayOfWeek.ToString(),
                    Revenue = g.Sum(x => x.Revenue)
                })
                .OrderBy(r => r.Date)
                .ToList();

            return grouped;
        }

        public async Task<List<GeneralInfoAbtStoreDTO>> GetGeneralIngoAbtStore(int sellerId)
        {
            IQueryable<object> objects = await Database.Statistics.GetGeneralInfoAbtStore(sellerId);
            Dictionary<string, decimal> storePriceMap = new Dictionary<string, decimal>();
            Dictionary<string, int> storeOrdersMap = new Dictionary<string, int>();

            foreach (var item in objects)
            {
                var storeProp = item.GetType().GetProperty("Name");
                var storeName = storeProp?.GetValue(item)?.ToString();

                var priceProp = item.GetType().GetProperty("PriceAtMoment");
                var priceObj = priceProp?.GetValue(item);
                decimal price = priceObj != null ? Convert.ToDecimal(priceObj) : 0;

                if (!string.IsNullOrEmpty(storeName))
                {
                    if (storePriceMap.ContainsKey(storeName))
                        storePriceMap[storeName] += price;
                    else
                        storePriceMap[storeName] = price;

                    if (storeOrdersMap.ContainsKey(storeName))
                        storeOrdersMap[storeName] += 1;
                    else
                        storeOrdersMap[storeName] = 1;
                }
            }

            var list = storePriceMap
                .Select(kvp => new GeneralInfoAbtStoreDTO
                {
                    StoreName = kvp.Key,
                    TotalRevenue = kvp.Value,
                    TotalOrders = storeOrdersMap.ContainsKey(kvp.Key) ? storeOrdersMap[kvp.Key] : 0
                })
                .ToList();

            return list;
        }

        public async Task<List<TopProductDTO>> GetTop10BestProducts(int storeId)
        {
            IQueryable<object> rawData = await Database.Statistics.GetTop10BestProducts(storeId);
            var salesList = new List<(int ProductId, string ProductName, int Quantity)>();

            foreach (var item in rawData)
            {
                var productIdProp = item.GetType().GetProperty("ProductId");
                var productNameProp = item.GetType().GetProperty("ProductName");
                var quantityProp = item.GetType().GetProperty("Quantity");

                if (productIdProp != null && productNameProp != null && quantityProp != null)
                {
                    int productId = (int)productIdProp.GetValue(item);
                    string name = productNameProp.GetValue(item)?.ToString();
                    int quantity = (int)quantityProp.GetValue(item);

                    salesList.Add((productId, name, quantity));
                }
            }

            var topProducts = salesList
                .GroupBy(x => new { x.ProductId, x.ProductName })
                .Select(g => new TopProductDTO
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    QuantitySold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(p => p.QuantitySold)
                .Take(10)
                .ToList();

            return topProducts;
        }

        public async Task<StoreTotalStatisticsDTO> GetStoreTotalStatistics(int storeId)
        {
            var objects = await Database.Statistics.GetRawStoreTotalStatistics(storeId);
            var totalSoldItems = 0;
            var totalOrderItems = 0;
            var totalRevenue = 0m;
            var customerIds = new HashSet<int>();

            foreach (var item in objects)
            {
                var quantityProp = item.GetType().GetProperty("Quantity");
                var revenueProp = item.GetType().GetProperty("Revenue");
                var customerIdProp = item.GetType().GetProperty("CustomerId");

                var quantity = (int)quantityProp?.GetValue(item);
                var revenue = (decimal)revenueProp?.GetValue(item);
                var customerId = (int)customerIdProp?.GetValue(item);

                totalSoldItems += quantity;
                totalOrderItems += 1;
                totalRevenue += revenue;
                customerIds.Add(customerId);
            }
        var totalRevenueRounded = Math.Round(totalRevenue).ToString(CultureInfo.InvariantCulture);


            return new StoreTotalStatisticsDTO
            {
                TotalSoldItems = totalSoldItems,
                TotalOrderItems = totalOrderItems,
                TotalRevenue = totalRevenueRounded,
                TotalCustomers = customerIds.Count
            };
        }
        
        public async Task<List<DailyRevenueDTO>> GetPrePreviousMonthRevenueGeneral(int sellerId)
        {
            IQueryable<object> objects = await Database.Statistics.GetPrePreviousMonthRevenueGeneral(sellerId);
            return GetStatisticsWithSummedRevenues(objects);
        }

        public async Task<List<DailyRevenueDTO>> GetPreviousMonthRevenueGeneral(int sellerId)
        {
            IQueryable<object> objects = await Database.Statistics.GetPreviousMonthRevenueGeneral(sellerId);
            return GetStatisticsWithSummedRevenues(objects);
        }

        public async Task<List<CategorySalesDTO>> GetSalesByCategoriesGeneral(int sellerId)
        {

            IQueryable<object> objects = await Database.Statistics.GetSalesByCategoriesGeneral(sellerId);
            List<CategorySalesDTO> list = new List<CategorySalesDTO>();

            foreach (var item in objects)
            {
                CategorySalesDTO dto = new CategorySalesDTO();
                var nameProperty = item.GetType().GetProperty("Name");
                var quantityProperty = item.GetType().GetProperty("Quantity");

                if (nameProperty != null && quantityProperty != null)
                {
                    dto.CategoryName = (string)nameProperty.GetValue(item);
                    dto.QuantityOfSales = (int)quantityProperty.GetValue(item);
                }

                list.Add(dto);
            }

            var groupedByName = list
               .GroupBy(s => s.CategoryName)
               .Where(group => group.Count() >= 1)
               .Select(group => new CategorySalesDTO
               {
                   CategoryName = group.Key,
                   QuantityOfSales = group.Sum(s => s.QuantityOfSales)
               })
               .ToList();

            

            return groupedByName;
        }

        public async Task<SellerTotalStatisticsDTO> GetRawStoreTotalStatisticsGeneral(int sellerId)
        {
            try
            {
                var objects = await Database.Statistics.GetRawStoreTotalStatisticsGeneral(sellerId);
                var productsBySeller = await _productService.GetProductsBySeller(sellerId);
                var totalProducts = productsBySeller.Count();
                var totalOrderItems = 0;
                var totalRevenue = 0m;
                var customerIds = new HashSet<int>();
           
               
            
                foreach (var item in objects)
                {
                    var revenueProp = item.GetType().GetProperty("Revenue");
                    var customerIdProp = item.GetType().GetProperty("CustomerId");

                    var revenue = (decimal)revenueProp?.GetValue(item);
                    var customerId = (int)customerIdProp?.GetValue(item);

                    totalOrderItems += 1;
                    totalRevenue += revenue;
                    customerIds.Add(customerId);
                }
                var totalRevenueRounded = totalRevenue.ToString("#");

                return new SellerTotalStatisticsDTO
                {
                    
                    TotalProducts = totalProducts,
                    TotalOrderItems = totalOrderItems,
                    TotalRevenue = totalRevenueRounded,
                    TotalCustomers = customerIds.Count
                };
            }
            catch (Exception ex)
            {
                return null;
            }

           
        }


    }
}

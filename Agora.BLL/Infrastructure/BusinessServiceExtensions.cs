using Agora.BLL.Interfaces;
using Agora.BLL.Services;
using Agora.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Agora.BLL.Infrastructure
{
    public static class BusinessServiceExtensions
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IBankCardService, BankCardService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ICashbackService, CashbackService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IDeliveryOptionsService, DeliveryOptionsService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IFAQCategoryService, FAQCategoryService>();
            services.AddScoped<IFAQService, FAQService>();
            services.AddScoped<IGiftCardService, GiftCardService>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductReviewService, ProductReviewService>();
            services.AddScoped<IReturnItemService, ReturnItemService>();
            services.AddScoped<IReturnService, ReturnService>();
            services.AddScoped<ISellerReviewService, SellerReviewService>();
            services.AddScoped<ISellerService, SellerService>();
            services.AddScoped<IShippingService, ShippingService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<ISubcategoryService, SubcategoryService>();
            services.AddScoped<ISupportService, SupportService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<ISecureService, SecureService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddSingleton<StatisticsCacheService>();
            services.AddSingleton<IHostedService>(sp => sp.GetRequiredService<StatisticsCacheService>());
            services.AddSingleton<IStatisticsInitializer>(sp => sp.GetRequiredService<StatisticsCacheService>());
        }
    }
}


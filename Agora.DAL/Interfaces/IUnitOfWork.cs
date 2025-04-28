using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Address> Addresses { get; }
        IAdminRepository Admins { get; }
        IRepository<BankCard> BankCards { get; }
        IRepository<Brand> Brands { get; }
        IRepository<Cashback> Cashbacks { get; }
        IRepository<Category> Categories { get; }
        IRepository<Country> Countries { get; }
        ICustomerRepository Customers { get; }
        IRepository<DeliveryOptions> DeliveryOptions { get; }
        IRepository<Discount> Discounts { get; }
        IRepository<FAQ> FAQs { get; }
        IRepository<FAQCategory> FAQCategories { get; }
        IRepository<GiftCard> GiftCards { get; }
        IRepository<Order> Orders { get; }
        IOrderItemRepository OrderItems { get; }
        IRepository<Payment> Payments { get; }
        IRepository<PaymentMethod> PaymentMethods { get; }
        IProductRepository Products { get; }
        IRepository<ProductReview> ProductReviews { get; }
        IRepository<Return> Returns { get; }
        IRepository<ReturnItem> ReturnItems { get; }
        ISellerRepository Sellers { get; }
        IRepository<SellerReview> SellerReviews { get; }
        IRepository<Shipping> Shippings { get; }
        IStoreRepository Stores { get; }
        IRepository<Subcategory> Subcategories { get; }
        IRepository<Support> Supports { get; }
        IUserRepository Users { get; }
        IRepository<Wishlist> Wishlists { get; }
        IStatisticsRepository Statistics { get; }
        Task Save();
    }
}


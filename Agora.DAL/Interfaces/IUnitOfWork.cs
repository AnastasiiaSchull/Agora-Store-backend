using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;
using Agora.DAL.Repository;

namespace Agora.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        //IRepository<Address> Addresses { get; }
        IAdminRepository Admins { get; }
        IRepository<BankCard> BankCards { get; }
        IBrandRepository Brands { get; }
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
        IProductReviewRepository ProductReviews { get; }
        IRepository<Return> Returns { get; }
        IRepository<ReturnItem> ReturnItems { get; }
        ISellerRepository Sellers { get; }
        IRepository<SellerReview> SellerReviews { get; }
        IShippingRepository Shippings { get; }
        IStoreRepository Stores { get; }
        IRepository<Subcategory> Subcategories { get; }
        IRepository<Support> Supports { get; }
        IUserRepository Users { get; }
        IWishlistRepository Wishlists { get; }
        IStatisticsRepository Statistics { get; }
        IAddressRepository Addresses { get; }
        IAddressUserRepository AddressUser { get; }
        Task Save();
    }
}


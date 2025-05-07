using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Agora.DAL.Repository
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private AgoraContext db;

        private AddressRepository addressRepository;
        private AdminRepository adminRepository;
        private BankCardRepository bankCardRepository;
        private BrandRepository brandRepository;
        private CashbackRepository cashbackRepository;
        private CategoryRepository categoryRepository;
        private CountryRepository countryRepository;
        private CustomerRepository customerRepository;
        private DeliveryOptionsRepository deliveryOptionsRepository;
        private DiscountRepository discountRepository;
        private FAQCategoryRepository FAQCategoryRepository;
        private FAQRepository FAQRepository;
        private GiftCardRepository giftCardRepository;
        private OrderRepository orderRepository;
        private OrderItemRepository orderItemRepository;
        private PaymentMethodRepository paymentMethodRepository;
        private PaymentRepository paymentRepository;
        private ProductRepository productRepository;
        private ProductReviewRepository productReviewRepository;
        private ReturnRepository returnRepository;
        private ReturnItemRepository returnItemRepository;
        private SellerRepository sellerRepository;
        private SellerReviewRepository sellerReviewRepository;
        private ShippingRepository shippingRepository;
        private StoreRepository storeRepository;
        private SubcategoryRepository subcategoryRepository;
        private SupportRepository supportRepository;
        private UserRepository userRepository;
        private WishlistRepository wishlistRepository;
        private StatisticsRepository statisticsRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        public EFUnitOfWork(AgoraContext context, UserManager<ApplicationUser> userManager)
        {
            db = context;
            _userManager = userManager;
        }

        public IRepository<Address> Addresses
        {
            get
            {
                if (addressRepository == null)
                    addressRepository = new AddressRepository(db);
                return addressRepository;
            }
        }
        public IAdminRepository Admins
        {
            get
            {
                if (adminRepository == null)
                    adminRepository = new AdminRepository(db);
                return adminRepository;
            }
        }
        public IRepository<BankCard> BankCards
        {
            get
            {
                if (bankCardRepository == null)
                    bankCardRepository = new BankCardRepository(db);
                return bankCardRepository;
            }
        }
        public IRepository<Brand> Brands
        {
            get
            {
                if (brandRepository == null)
                    brandRepository = new BrandRepository(db);
                return brandRepository;
            }
        }
        public IRepository<Cashback> Cashbacks
        {
            get
            {
                if (cashbackRepository == null)
                    cashbackRepository = new CashbackRepository(db);
                return cashbackRepository;
            }
        }
        public IRepository<Category> Categories
        {
            get
            {
                if (categoryRepository == null)
                    categoryRepository = new CategoryRepository(db);
                return categoryRepository;
            }
        }
        public IRepository<Country> Countries
        {
            get
            {
                if (countryRepository == null)
                    countryRepository = new CountryRepository(db);
                return countryRepository;
            }
        }
        public ICustomerRepository Customers
        {
            get
            {
                if (customerRepository == null)
                    customerRepository = new CustomerRepository(db);
                return customerRepository;
            }
        }
        public IRepository<DeliveryOptions> DeliveryOptions
        {
            get
            {
                if (deliveryOptionsRepository == null)
                    deliveryOptionsRepository = new DeliveryOptionsRepository(db);
                return deliveryOptionsRepository;
            }
        }
        public IRepository<Discount> Discounts
        {
            get
            {
                if (discountRepository == null)
                    discountRepository = new DiscountRepository(db);
                return discountRepository;
            }
        }
        public IRepository<FAQCategory> FAQCategories
        {
            get
            {
                if (FAQCategoryRepository == null)
                    FAQCategoryRepository = new FAQCategoryRepository(db);
                return FAQCategoryRepository;
            }
        }
        public IRepository<FAQ> FAQs
        {
            get
            {
                if (FAQRepository == null)
                    FAQRepository = new FAQRepository(db);
                return FAQRepository;
            }
        }
        public IRepository<GiftCard> GiftCards
        {
            get
            {
                if (giftCardRepository == null)
                    giftCardRepository = new GiftCardRepository(db);
                return giftCardRepository;
            }
        }
        public IRepository<Order> Orders
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new OrderRepository(db);
                return orderRepository;
            }
        }
        public IOrderItemRepository OrderItems
        {
            get
            {
                if (orderItemRepository == null)
                    orderItemRepository = new OrderItemRepository(db);
                return orderItemRepository;
            }
        }
        public IRepository<PaymentMethod> PaymentMethods
        {
            get
            {
                if (paymentMethodRepository == null)
                    paymentMethodRepository = new PaymentMethodRepository(db);
                return paymentMethodRepository;
            }
        }
        public IRepository<Payment> Payments
        {
            get
            {
                if (paymentRepository == null)
                    paymentRepository = new PaymentRepository(db);
                return paymentRepository;
            }
        }
        public IProductRepository Products
        {
            get
            {
                if (productRepository == null)
                    productRepository = new ProductRepository(db);
                return productRepository;
            }
        }
        public IRepository<ProductReview> ProductReviews
        {
            get
            {
                if (productReviewRepository == null)
                    productReviewRepository = new ProductReviewRepository(db);
                return productReviewRepository;
            }
        }
        public IRepository<Return> Returns
        {
            get
            {
                if (returnRepository == null)
                    returnRepository = new ReturnRepository(db);
                return returnRepository;
            }
        }
        public IRepository<ReturnItem> ReturnItems
        {
            get
            {
                if (returnItemRepository == null)
                    returnItemRepository = new ReturnItemRepository(db);
                return returnItemRepository;
            }
        }
        public ISellerRepository Sellers
        {
            get
            {
                if (sellerRepository == null)
                    sellerRepository = new SellerRepository(db);
                return sellerRepository;
            }
        }
        public IRepository<SellerReview> SellerReviews
        {
            get
            {
                if (sellerReviewRepository == null)
                    sellerReviewRepository = new SellerReviewRepository(db);
                return sellerReviewRepository;
            }
        }
        public IShippingRepository Shippings
        {
            get
            {
                if (shippingRepository == null)
                    shippingRepository = new ShippingRepository(db);
                return shippingRepository;
            }
        }
        public IStoreRepository Stores
        {
            get
            {
                if (storeRepository == null)
                    storeRepository = new StoreRepository(db);
                return storeRepository;
            }
        }
        public IRepository<Subcategory> Subcategories
        {
            get
            {
                if (subcategoryRepository == null)
                    subcategoryRepository = new SubcategoryRepository(db);
                return subcategoryRepository;
            }
        }
        public IRepository<Support> Supports
        {
            get
            {
                if (supportRepository == null)
                    supportRepository = new SupportRepository(db);
                return supportRepository;
            }
        }
        public IUserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db, _userManager);
                return userRepository;
            }
        }
        public IRepository<Wishlist> Wishlists
        {
            get
            {
                if (wishlistRepository == null)
                    wishlistRepository = new WishlistRepository(db);
                return wishlistRepository;
            }
        }
        public IStatisticsRepository Statistics
        {
            get
            {
                if (statisticsRepository == null)
                    statisticsRepository = new StatisticsRepository(db);
                return statisticsRepository;
            }
        }

        public async Task Save()
        {
            await db.SaveChangesAsync();

        }

    }
}

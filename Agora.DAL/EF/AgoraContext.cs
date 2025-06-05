using Microsoft.EntityFrameworkCore;
using Agora.DAL.Entities;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Agora.DAL.EF
{
    public class AgoraContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<BankCard> BankCards { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cashback> Cashbacks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DeliveryOptions> DeliveryOptions { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<FAQCategory> FAQCategories { get; set; }
        public DbSet<GiftCard> GiftCards { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Product> Products { get; set; }    
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<Return> Returns { get; set; }
        public DbSet<ReturnItem> ReturnItems { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<SellerReview> SellerReviews { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Support> Supports { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<AddressUser> AddressUser { get; set; }
        public DbSet<BrandSubcategory> BrandSubcategories { get; set; }

        public AgoraContext(DbContextOptions<AgoraContext> options)
                   : base(options)
        {
            //Database.EnsureCreated();
        }

        public class SampleContextFactory : IDesignTimeDbContextFactory<AgoraContext> // class for migrations
        {
            public AgoraContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<AgoraContext>();

                ConfigurationBuilder builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("appsettings.json");
                IConfigurationRoot config = builder.Build();
           
                string connectionString = config.GetConnectionString("DefaultConnection");
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                              .UseLazyLoadingProxies()
                               .EnableSensitiveDataLogging()
                                 .LogTo(Console.WriteLine, LogLevel.Information);


                return new AgoraContext(optionsBuilder.Options);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AddressUser>()
                .HasKey(au => new { au.UserId, au.AddressesId });

            modelBuilder.Entity<AddressUser>()
                .HasOne(au => au.User)
                .WithMany(u => u.AddressUsers)
                .HasForeignKey(au => au.UserId);

            modelBuilder.Entity<AddressUser>()
                .HasOne(au => au.Address)
                .WithMany(a => a.AddressUsers)
                .HasForeignKey(au => au.AddressesId);

            modelBuilder.Entity<AddressUser>()
                .ToTable("AddressUser");

            // каскадное удаление вопросов FAQ при удалении категории
            modelBuilder.Entity<FAQ>()
                .HasOne(f => f.FAQCategory)
                .WithMany(c => c.FAQs)
                .HasForeignKey(f => f.FAQCategoryId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<BrandSubcategory>()
                .HasKey(bs => new { bs.BrandId, bs.SubcategoryId });

            modelBuilder.Entity<BrandSubcategory>()
                .HasOne(bs => bs.Brand)
                .WithMany(b => b.BrandSubcategories)
                .HasForeignKey(bs => bs.BrandId);

            modelBuilder.Entity<BrandSubcategory>()
                .HasOne(bs => bs.Subcategory)
                .WithMany(sc => sc.BrandSubcategories)
                .HasForeignKey(bs => bs.SubcategoryId);

        }
    }

}


using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using AutoMapper;

namespace Agora.BLL.Services
{
    public class ProductService : IProductService
    {
        IUnitOfWork Database { get; set; }
        IMapper _mapper;

        public ProductService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var products = await Database.Products.Find(p => p.IsAvailable);

            // загружаем все  скидки           
            var discountsRaw = await Database.Discounts.Find(d =>
                d.StartDate <= today && d.EndDate >= today);

            var discounts = discountsRaw?.ToList() ?? new List<Discount>();

            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);

            foreach (var product in productDTOs)
            {
                var discount = discounts.FirstOrDefault(d =>
                   d.AllProducts
                    || (d.Products?.Any(p => p.Id == product.Id) ?? false)
                    || (d.Categories?.Any(c => c.Id == product.CategoryId) ?? false)
                    || (d.Subcategories?.Any(s => s.Id == product.SubcategoryId) ?? false)
                    || (d.Brands?.Any(b => b.Id == product.BrandId) ?? false)
                );

                if (discount != null)
                {
                    product.DiscountedPrice = product.Price - (product.Price * discount.Percentage / 100);
                }
            }
            return productDTOs;
        }

        public async Task<IEnumerable<ProductDTO>> GetFilteredByName(string filter)
        {
            var filteredProducts = await Database.Products.Find(p =>
                   p.IsAvailable &&
                   (
                       p.Name!.Contains(filter) ||
                       p.Category!.Name!.Contains(filter) ||
                       p.Subcategory!.Name!.Contains(filter) ||
                       p.Brand!.Name!.Contains(filter)
                   )
               );
            return _mapper.Map<IEnumerable<ProductDTO>>(filteredProducts);
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsBySeller(int sellerId)
        {
            var products = await Database.Products.GetProductsBySeller(sellerId);
            return _mapper.Map<IEnumerable<ProductDTO>>(products.ToList());
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByStore(int storeId)
        {
            var products = await Database.Products.Find(p => p.StoreId == storeId && p.IsAvailable);
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> Get(int id)
        {
            var product = await Database.Products.Get(id);
            if (product == null)
                throw new ValidationExceptionFromService("There is no product with this id", "");
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DiscountedPrice = product.DiscountedPrice,
                StockQuantity = product.StockQuantity,
                Rating = product.Rating,
                ImagesPath = product.ImagesPath,
                IsAvailable = product.IsAvailable,
                CategoryId = _mapper.Map<int>(product.CategoryId),
                SubcategoryId = _mapper.Map<int>(product.SubcategoryId),
                BrandId = _mapper.Map<int>(product.BrandId),
                StoreId = _mapper.Map<int>(product.StoreId),
                Store = product.Store == null ? null : _mapper.Map<StoreDTO>(product.Store),
                ReviewCount = product.ProductReviews?.Count ?? 0,
                SellerId = product.Store?.SellerId ?? 0,
            };
        }

        public async Task<ProductDTO> GetByName(string name)
        {
            var product = await Database.Products.GetByName(name);
            if (product == null)
                throw new ValidationExceptionFromService("There is no product with this id", "");
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DiscountedPrice = product.DiscountedPrice,
                StockQuantity = product.StockQuantity,
                Rating = product.Rating,
                ImagesPath = product.ImagesPath,
                IsAvailable = product.IsAvailable,
                ReviewCount = product.ProductReviews?.Count ?? 0
            };
        }

        public async Task<IEnumerable<ProductDTO>> GetSimilarProducts(int productId)
        {
            var product = await Database.Products.Get(productId);
            if (product == null)
                throw new ValidationExceptionFromService("Product not found", "");

            var today = DateOnly.FromDateTime(DateTime.Today);

            // ищем похожие продукты по категории или подкатегории
            var all = await Database.Products.Find(p =>
                p.Id != productId &&
                (p.CategoryId == product.CategoryId || p.SubcategoryId == product.SubcategoryId));

            var discountsRaw = await Database.Discounts.Find(d =>
                d.StartDate <= today && d.EndDate >= today);

            var discounts = discountsRaw?.ToList() ?? new List<Discount>();

            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(all.Take(10));

            foreach (var item in productDTOs)
            {
                var discount = discounts.FirstOrDefault(d =>
                    d.AllProducts
                    || (d.Products?.Any(p => p.Id == product.Id) ?? false)
                    || (d.Categories?.Any(c => c.Id == product.CategoryId) ?? false)
                    || (d.Subcategories?.Any(s => s.Id == product.SubcategoryId) ?? false)
                    || (d.Brands?.Any(b => b.Id == product.BrandId) ?? false)
                );

                if (discount != null)
                {
                    item.DiscountedPrice = item.Price - (item.Price * discount.Percentage / 100);
                }
            }
            return productDTOs;
        }

        public async Task Create(ProductDTO productDTO)
        {
            var product = new Product
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                StockQuantity = productDTO.StockQuantity,
                Rating = productDTO.Rating,
                ImagesPath = productDTO.ImagesPath,
                IsAvailable = productDTO.IsAvailable,
                StoreId = productDTO.StoreId,
                SubcategoryId = productDTO.SubcategoryId,
                CategoryId = productDTO.CategoryId,
                BrandId = productDTO.BrandId,
            };
            var discounts = await Database.Discounts.GetActiveWithRelations();


            ApplyDiscount(product, discounts);

            await Database.Products.Create(product);
            await Database.Save();
        }

        public async Task Update(ProductDTO productDTO)
        {

            var product = new Product
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                StockQuantity = productDTO.StockQuantity,
                Rating = productDTO.Rating,
                ImagesPath = productDTO.ImagesPath,
                IsAvailable = productDTO.IsAvailable,
                SubcategoryId = productDTO.SubcategoryId,
                CategoryId = productDTO.CategoryId,
                BrandId = productDTO.BrandId,
                StoreId = productDTO.StoreId,
            };
            var discounts = await Database.Discounts.GetActiveWithRelations();

            ApplyDiscount(product, discounts);

            Database.Products.Update(product);
            await Database.Save();
        }

        public async Task Delete(int id)
        {
            await Database.Products.Delete(id);
            await Database.Save();
        }

        private void ApplyDiscount(Product product, IEnumerable<Discount> discounts)
        {
            if (discounts == null || !discounts.Any())
            {
                product.DiscountedPrice = null;
                return;
            }
            var applicableDiscounts = discounts.Where(d =>
                d.AllProducts
                || (d.Products?.Any(p => p.Id == product.Id) ?? false)
                || (d.Categories?.Any(c => c.Id == product.CategoryId) ?? false)
                || (d.Subcategories?.Any(s => s.Id == product.SubcategoryId) ?? false)
                || (d.Brands?.Any(b => b.Id == product.BrandId) ?? false)
            );
            var bestDiscount = applicableDiscounts.OrderByDescending(d => d.Percentage).FirstOrDefault();

            if (bestDiscount != null)
            {
                //Console.WriteLine($"скидка {bestDiscount.Percentage}% к товару {product.Name}");
                product.DiscountedPrice = product.Price - (product.Price * bestDiscount.Percentage / 100);
            }
            else
            {
                product.DiscountedPrice = null;
            }
        }

        public async Task UpdateAllDiscountedPrices()
        {
            var products = await Database.Products.GetAll();

            var discounts = await Database.Discounts.GetActiveWithRelations();

            foreach (var product in products)
            {
                ApplyDiscount(product, discounts); 
                // Console.WriteLine($"Product {product.Name} new price: {product.DiscountedPrice}");
                Database.Products.Update(product);
            }

            await Database.Save();

        }
    }
}

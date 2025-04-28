using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;
using Agora.DAL.Entities;
using AutoMapper;

namespace Agora.BLL.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<Wishlist, WishlistDTO>();
            CreateMap<Support, SupportDTO>();
            CreateMap<Subcategory, SubcategoryDTO>();
            CreateMap<Store, StoreDTO>();
            CreateMap<Shipping, ShippingDTO>();
            CreateMap<Seller, SellerDTO>();
            CreateMap<SellerReview, SellerReviewDTO>();
            CreateMap<Return, ReturnDTO>();
            CreateMap<Address, AddressDTO>();
            CreateMap<Admin, AdminDTO>();
            CreateMap<BankCard, BankCardDTO>();
            CreateMap<Brand, BrandDTO>();
            CreateMap<Cashback, CashbackDTO>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<Country, CountryDTO>();
            CreateMap<Customer, CustomerDTO>();
            CreateMap<DeliveryOptions, DeliveryOptionsDTO>();
            CreateMap<Discount, DiscountDTO>();
            CreateMap<FAQ, FAQDTO>();
            CreateMap<FAQCategory, FAQCategoryDTO>();
            CreateMap<GiftCard, GiftCardDTO>();
            CreateMap<ReturnItem, ReturnItemDTO>();
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductReview, ProductReviewDTO>();
            CreateMap<Payment, PaymentDTO>();
            CreateMap<PaymentMethod, PaymentMethodDTO>();
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.ProductDTO, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}

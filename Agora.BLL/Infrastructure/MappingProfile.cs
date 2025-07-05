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
            CreateMap<ProductWishlist, ProductWishlistDTO>();
                //.ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
            CreateMap<Wishlist, WishlistDTO>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductWishlists.Select(pw => pw.Product)))
                .ForMember(dest => dest.ProductWishlists, opt => opt.MapFrom(src => src.ProductWishlists));
            CreateMap<Support, SupportDTO>();
            CreateMap<Subcategory, SubcategoryDTO>()
                .ForMember(dest => dest.CategoryDTO, opt => opt.MapFrom(src => src.Category));
            CreateMap<Store, StoreDTO>();
            CreateMap<Shipping, ShippingDTO>()
                .ForMember(dest => dest.DeliveryOptionsDTO, opt => opt.MapFrom(src => src.DeliveryOptions))
                //.ForMember(dest => dest.OrderItemDTO, opt => opt.MapFrom(src => src.OrderItem))
                .ForMember(dest => dest.AddressDTO, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<Seller, SellerDTO>();
            CreateMap<SellerReview, SellerReviewDTO>();
            CreateMap<Return, ReturnDTO>();
            CreateMap<Address, AddressDTO>()
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name));
            CreateMap<Admin, AdminDTO>();
            CreateMap<BankCard, BankCardDTO>();
            CreateMap<Brand, BrandDTO>();
            CreateMap<Cashback, CashbackDTO>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<Country, CountryDTO>();
            CreateMap<Customer, CustomerDTO>()
                 .ForMember(dest => dest.UserDTO, opt => opt.MapFrom(src => src.User));
            CreateMap<DeliveryOptions, DeliveryOptionsDTO>()
                 .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
            CreateMap<DiscountDTO, Discount>()
                 .ForMember(dest => dest.AllProducts, opt => opt.MapFrom(src => src.AllProducts));
            CreateMap<FAQ, FAQDTO>();
            CreateMap<FAQCategory, FAQCategoryDTO>();
            CreateMap<GiftCard, GiftCardDTO>();
            CreateMap<ReturnItem, ReturnItemDTO>();
            CreateMap<Product, ProductDTO>()
                 .ForMember(dest => dest.Store, opt => opt.MapFrom(src => src.Store))              
                 .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                 .ForMember(dest => dest.SubcategoryId, opt => opt.MapFrom(src => src.SubcategoryId))
                 .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
                 .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.StoreId))
                 .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.ProductReviews!.Count))
                 .ForMember(dest => dest.SellerId, opt => opt.MapFrom(src => src.Store!.SellerId));
            CreateMap<ProductReview, ProductReviewDTO>();
            CreateMap<Payment, PaymentDTO>();
            CreateMap<PaymentMethod, PaymentMethodDTO>();
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.CustomerDTO, opt => opt.MapFrom(src => src.Customer));
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.ProductDTO, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.OrderDTO, opt => opt.MapFrom(src => src.Order)) 
                .ForMember(dest => dest.ShippingDTO, opt => opt.MapFrom(src => src.Shipping))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<Address, AddressDTO>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.Name));

        }
    }
}

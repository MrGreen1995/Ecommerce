using AutoMapper;
using Basket.Core.Entities;
using Basket.Application.Responses;

namespace Basket.Application.Mappers;

public class BasketMappingProfile : Profile
{
    public BasketMappingProfile()
    {
        CreateMap<ShoppingCart, ShoppingCartResponse>().ReverseMap();
        CreateMap<ShoppingCartItem, ShoppingCartItemResponse>().ReverseMap();
    }
}
using AutoMapper;
using Discount.GRPC.Entities;
using Discount.GRPC.Protos;

namespace Discount.GRPC.Mapper
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<Coupon, CouponModel>().ForMember(p => p.Amount, opt => opt.MapFrom(x => x.ValueDiscount)).ReverseMap();
        }
    }
}

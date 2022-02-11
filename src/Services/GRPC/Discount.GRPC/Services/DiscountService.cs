using AutoMapper;
using Discount.GRPC.Entities;
using Discount.GRPC.Protos;
using Discount.GRPC.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Discount.GRPC.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;
        public DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {

            var coupon = await _repository.GetDiscountAsync(request.CouponCode);
            if (coupon == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discout with Coupon Code={request.CouponCode} was not found."));

            _logger.LogInformation("Discount is retrieved for productname: {couponCode}, Amount: {amount}", coupon.CouponCode);

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;

        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {

            var coupon = _mapper.Map<Coupon>(request.Coupon);

            await _repository.InsertDiscountAsync(coupon);
            _logger.LogInformation("Discount is successfully created. Discount Code: {couponCode}", coupon.CouponCode);

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;

        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            await _repository.UpdateDiscountAsync(coupon);
            _logger.LogInformation("Discount is successfully updated. Discount Code: {couponCode}", coupon.CouponCode);

            var couponmode = _mapper.Map<CouponModel>(coupon);
            return couponmode;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _repository.DeleteDiscountAsync(request.CouponCode);

            var response = new DeleteDiscountResponse()
            {
                Success = deleted.StatusSuccess
            };

            return response;

        }
    }
}

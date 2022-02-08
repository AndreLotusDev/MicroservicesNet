using Discount.API.Entities;
using Discount.API.Helper;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;

        public CouponController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        [HttpGet("{couponCode}" ,Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDiscount(string couponCode)
        {
            var discount = await _discountRepository.GetDiscountAsync(couponCode);
            return Ok(discount);
        }

        [HttpGet(Name = "GetAllCoupons")]
        [ProducesResponseType(typeof(IEnumerable<Coupon>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllCoupons()
        {
            var coupons = await _discountRepository.GetAllCoupons();
            return Ok(coupons);
        }

        [HttpPost(Name = "InsertCoupon")]
        [ProducesResponseType(typeof(OperationStatus), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> InsertCoupon([FromBody] Coupon couponToInsert)
        {
            var operationStatus = await _discountRepository.InsertDiscountAsync(couponToInsert);
            return Ok(operationStatus);
        }

        [HttpPut]
        [ProducesResponseType(typeof(OperationStatus), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateDiscountAsync([FromBody] Coupon couponToUpdate)
        {
            var operationStatus = await _discountRepository.UpdateDiscountAsync(couponToUpdate);
            return Ok(operationStatus);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(OperationStatus), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteDiscount(string couponCodeToDelete)
        {
            var operationStatus = await _discountRepository.DeleteDiscountAsync(couponCodeToDelete);
            return Ok(operationStatus);
        }

    }
}

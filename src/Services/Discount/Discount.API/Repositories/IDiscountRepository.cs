using Discount.API.Entities;
using Discount.API.Helper;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscountAsync(string couponCode);
        Task<IEnumerable<Coupon>> GetAllCoupons();
        Task<OperationStatus> InsertDiscountAsync(Coupon coupon);
        Task<OperationStatus> UpdateDiscountAsync(Coupon coupon);
        Task<OperationStatus> DeleteDiscountAsync(string couponCode);
    }
}

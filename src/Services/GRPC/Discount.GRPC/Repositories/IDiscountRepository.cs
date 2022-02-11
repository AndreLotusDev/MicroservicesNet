using Discount.GRPC.Entities;
using Discount.GRPC.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discount.GRPC.Repositories
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

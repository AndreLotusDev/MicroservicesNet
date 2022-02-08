using Dapper;
using Discount.API.Context;
using Discount.API.Entities;
using Discount.API.TableMapping;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OperationStatus = Discount.API.Helper.OperationStatus;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IContext _context;
        private string T_NAME => Coupons.TABLENAME;

        public DiscountRepository(IContext context)
        {
            _context = context;
        }

        public async Task<OperationStatus> InsertDiscountAsync(Coupon coupon)
        {
            bool isEverythingOk = await InsertSingleDiscount(coupon);

            if (isEverythingOk)
                return new OperationStatus("Successfully inserted!", isEverythingOk);

            return new OperationStatus("We had a problemn salving the new discount", false);
        }

        private async Task<bool> InsertSingleDiscount(Coupon coupon)
        {
            return Convert.ToBoolean(await _context.ContextDB.ExecuteAsync($"INSERT INTO {T_NAME} ({Coupons.DESCRIPTION}, {Coupons.COUPON_CODE} , {Coupons.VALUE_DISCOUNT})" +
                            $" VALUES(@{nameof(coupon.Description)}, @{nameof(coupon.CouponCode)}, @{nameof(coupon.ValueDiscount)})",
                            new { coupon.Description, coupon.CouponCode, coupon.ValueDiscount }));
        }

        public async Task<OperationStatus> DeleteDiscountAsync(string couponCode)
        {
            string queryToDelete = DeleteDiscount(couponCode);
            var deleted = Convert.ToBoolean(await _context.ContextDB.ExecuteAsync(queryToDelete, new { couponCode }));

            if (deleted)
                return new OperationStatus("Delete with success!", deleted);

            return new OperationStatus("We had a problemn deleting this coupon code!", deleted);
        }

        private string DeleteDiscount(string couponCode) => $"DELETE FROM {T_NAME} WHERE {Coupons.COUPON_CODE} = @{nameof(couponCode)}";

        public async Task<IEnumerable<Coupon>> GetAllCoupons()
        {
            string queryToGetAllCoupons = QueryAllDiscounts();

            dynamic coupons = await _context.ContextDB.QueryAsync<Coupon>(queryToGetAllCoupons);

            return coupons;
        }

        private string QueryAllDiscounts()
        {
            return $"SELECT" +
                            $" {T_NAME}.{Coupons.DESCRIPTION} AS Description," +
                            $" {T_NAME}.{Coupons.VALUE_DISCOUNT} AS ValueDiscount," +
                            $" {T_NAME}.{Coupons.COUPON_CODE} as CouponCode," +
                            $" {T_NAME}.{Coupons.DISCOUNT_ID} as Id" +
                            $" FROM {Coupons.TABLENAME}";
        }

        public async Task<Coupon> GetDiscountAsync(string couponCode)
        {
            Coupon coupon = await BringbackDiscounts(couponCode);

            var couponNotFound = coupon == null;
            if (couponNotFound)
                return new Coupon("Coupon invalid", 0, "INVALID COUPON");

            return coupon;
        }

        private async Task<Coupon> BringbackDiscounts(string couponCode)
        {
            return await _context.ContextDB.QueryFirstOrDefaultAsync<Coupon>(
                            $"SELECT " +
                            $" {T_NAME}.{Coupons.COUPON_CODE} AS CouponCode," +
                            $" {T_NAME}.{Coupons.DESCRIPTION} AS Descriptions, " +
                            $" {T_NAME}.{Coupons.DISCOUNT_ID} AS Id, " +
                            $" {T_NAME}.{Coupons.VALUE_DISCOUNT} As ValueDiscount " +
                            $" FROM {T_NAME} " +
                            $" WHERE {Coupons.COUPON_CODE} = @{nameof(couponCode)}", new { couponCode });
        }

        public async Task<OperationStatus> UpdateDiscountAsync(Coupon coupon)
        {
            bool updated = await UpdateOneCoupon(coupon);

            if (updated)
                return new OperationStatus("Updated with success!", updated);

            return new OperationStatus("We had a problem salving the new informations!", updated);
        }

        private async Task<bool> UpdateOneCoupon(Coupon coupon)
        {
            return Convert.ToBoolean(await _context.ContextDB.ExecuteAsync($"UPDATE {T_NAME} SET {Coupons.DESCRIPTION} = @{nameof(Coupons.DESCRIPTION)}," +
                            $" {Coupons.COUPON_CODE} = @{nameof(Coupon.CouponCode)}," +
                            $" {Coupons.VALUE_DISCOUNT} = @{nameof(Coupon.ValueDiscount)}" +
                            $" WHERE {Coupons.DISCOUNT_ID} = @{nameof(Coupon.Id)}", new { Description = coupon.Description, CouponCode = coupon.CouponCode, ValueDiscount = coupon.ValueDiscount, Id = coupon.Id }));
        }
    }
}

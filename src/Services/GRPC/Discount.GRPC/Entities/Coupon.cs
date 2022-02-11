using System;

namespace Discount.GRPC.Entities
{
    public class Coupon 
    {
        public Coupon(string description, int valueDiscount, string couponCode)
        {
            ValidateDescription(description);

            ValidateValueDiscount(valueDiscount);

            ValidateCouponCode(couponCode);
        }

        public Coupon()
        {

        }

        private void ValidateCouponCode(string couponCode)
        {
            var couponCantBeEmpty = couponCode.Length > 0;
            if (couponCantBeEmpty)
                CouponCode = couponCode;
            else
                throw new Exception("The coupon code cant be empty!");
        }

        private void ValidateValueDiscount(int valueDiscount)
        {
            var validValue = valueDiscount >= 0 || valueDiscount <= 100;
            if (validValue)
                ValueDiscount = valueDiscount;
            else
                throw new Exception("Your coupon needs to be in a valid value!");
        }

        private void ValidateDescription(string description)
        {
            var descriptionEShouldNotBeEmpty = description.Length > 0;
            if (descriptionEShouldNotBeEmpty)
                Description = description;
            else
                throw new Exception("The description should not be empty!");
        }

        public string Description { get; set; }

        public decimal ValueDiscount { get; set; }

        public string CouponCode { get; set;}

        public int Id { get; set; }


    }
}

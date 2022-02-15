using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        [HttpGet("userName", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basketOfTheUser = await _repository.GetBasket(userName);

            basketOfTheUser = await UpdatetePriceBasketWithDiscountCode(basketOfTheUser, basketOfTheUser.CouponCode);

            return Ok(SetANewUserIfNotExistOrReturnAnExistentUser(userName, basketOfTheUser));
        }

        private ShoppingCart SetANewUserIfNotExistOrReturnAnExistentUser(string userName, ShoppingCart basket) => basket ?? new ShoppingCart(userName);


        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basketWithNewInformations)
        {
            var basketOfTheUser = await UpdatetePriceBasketWithDiscountCode(basketWithNewInformations, basketWithNewInformations.CouponCode);

            return Ok(basketOfTheUser);
        }

        private async Task<ShoppingCart> UpdatetePriceBasketWithDiscountCode(ShoppingCart basketWithNewInformations, string couponCode)
        {
            var coupon = await _discountGrpcService.GetDiscount(couponCode);
            var basketOfTheUser = await _repository.UpdateBasket(basketWithNewInformations);
            basketOfTheUser.TotalPriceDiscount = basketOfTheUser.TotalPrice - (((decimal)coupon.Amount / 100) * basketOfTheUser.TotalPrice);
            return basketOfTheUser;
        }

        [HttpDelete]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _repository.GetBasket(basketCheckout.UserName);
            if(basket == null)
            {
                return BadRequest();
            }

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);

            //Add discount in future
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage);

            await _repository.DeleteBasket(basket.UserName);

            return Accepted();
        }
    }
}

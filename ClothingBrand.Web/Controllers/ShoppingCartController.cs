using ClothingBrand.Application.Common.DTO.OrderDto;
using ClothingBrand.Application.Common.DTO.Response.Payment;
using ClothingBrand.Application.Common.DTO.Response.Shipping;
using ClothingBrand.Application.Common.DTO.Response.ShoppingCart;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Application.Services;
using ClothingBrand.Web.helpers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ClothingBrand.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        // GET: api/shoppingcart/{userId}
        [HttpGet("{userId}")]
        public IActionResult GetShoppingCart(string userId)
        {
            try
            {
                var cart = _shoppingCartService.GetShoppingCart(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/shoppingcart/add
        [HttpPost("add")]
        public IActionResult AddToCart(string userId, [FromBody] AddToCartRequestDto request)
        {


            ShoppingCartItemDto requestDto = new ShoppingCartItemDto
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            try
            {
                _shoppingCartService.AddToCart(userId, requestDto);
                return Ok("Item added to cart successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/shoppingcart/remove
        [HttpDelete("remove/{userId}/{productId}")]
        public IActionResult RemoveFromCart(string userId, int productId)
        {
            try
            {
                _shoppingCartService.RemoveFromCart(userId, productId);
                return Ok("Item removed from cart successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/shoppingcart/checkout
        [HttpPost("checkout/{userId}")]
        public IActionResult Checkout(string userId, [FromBody] CheckoutRequestDto checkoutRequest)
        {
            ShippingDto checkoutShippingRequestDto = new ShippingDto
            {
                AddressLine1 = checkoutRequest.ShippingDetails.AddressLine1,
                AddressLine2 = checkoutRequest.ShippingDetails.AddressLine2,
                City = checkoutRequest.ShippingDetails.City,
                Country = checkoutRequest.ShippingDetails.Country,
                PostalCode = checkoutRequest.ShippingDetails.PostalCode,
                ShippingMethod = checkoutRequest.ShippingDetails.ShippingMethod,
                State = checkoutRequest.ShippingDetails.State
            };

            PaymentDto PaymentDetailsDto = new PaymentDto
            {
                CardNumber = checkoutRequest.PaymentDetails.CardNumber,
                ExpirationMonth = checkoutRequest.PaymentDetails.ExpirationMonth,
                ExpirationYear = checkoutRequest.PaymentDetails.ExpirationYear,
                Cvc = checkoutRequest.PaymentDetails.Cvc
            };

            try
            {
                var order = _shoppingCartService.Checkout(userId, checkoutShippingRequestDto, PaymentDetailsDto);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/shoppingcart/clear/{userId}
        [HttpDelete("clear/{userId}")]
        public IActionResult ClearCart(string userId)
        {
            try
            {
                _shoppingCartService.ClearCart(userId);
                return Ok("Shopping cart cleared successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

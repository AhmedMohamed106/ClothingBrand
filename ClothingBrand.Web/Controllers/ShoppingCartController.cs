using ClothingBrand.Application.Common.DTO.OrderDto;
using ClothingBrand.Application.Common.DTO.Response.Payment;
using ClothingBrand.Application.Common.DTO.Response.Shipping;
using ClothingBrand.Application.Common.DTO.Response.ShoppingCart;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Application.Services;
using ClothingBrand.Web.helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace ClothingBrand.WebApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ILogger<ShoppingCartController> _logger;


        public ShoppingCartController(IShoppingCartService shoppingCartService , ILogger<ShoppingCartController> logger)
        {
            _shoppingCartService = shoppingCartService;
            _logger = logger;

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

       //[Authorize]
        [HttpPost("add")]
        public IActionResult AddToCart(string userId , [FromBody] AddToCartRequestDto request)
        {
            //var userId = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userId))
            //{
            //    return Unauthorized("User ID not found.");
            //}


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
                // Log the detailed error
                _logger.LogError(ex, "Error adding item to cart for user {UserId}", userId);
                // Return a generic error message
                return BadRequest("An error occurred while adding the item to the cart.");
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
       // [Authorize]
        [HttpPost("checkout")]
        public IActionResult Checkout([FromBody] CheckoutRequestDto checkoutRequest)
        {
            if (checkoutRequest == null)
            {
                return BadRequest("Invalid request.");
            }

            if (string.IsNullOrWhiteSpace(checkoutRequest.userId))
            {
                return BadRequest("User ID is required.");
            }

            if (checkoutRequest.ShippingDetails == null)
            {
                return BadRequest("Shipping details are required.");
            }

            if (checkoutRequest.PaymentDto == null)
            {
                return BadRequest("Payment details are required.");
            }
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (string.IsNullOrEmpty(userId))
            //{
            //    return Unauthorized("User not authenticated.");
            //}

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

            PaymentDto paymentDetailsDto = new PaymentDto
            {
                Amount = checkoutRequest.PaymentDto.Amount,
               Currency = checkoutRequest.PaymentDto.Currency,
               PaymentMethodId = checkoutRequest.PaymentDto.PaymentMethodId
            };

            try
            {
                var order = _shoppingCartService.Checkout(checkoutRequest.userId, checkoutShippingRequestDto, paymentDetailsDto);
                return Ok(order);
            }
            catch (Exception ex)
            {
                // Log the detailed error
                _logger.LogError(ex, "Error during checkout for user {UserId}", checkoutRequest.userId);
                // Return a generic error message
                return StatusCode(500, $"Internal server error: {ex.Message}");
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

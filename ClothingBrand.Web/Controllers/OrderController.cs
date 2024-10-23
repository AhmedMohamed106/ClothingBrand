using ClothingBrand.Application.Common.DTO.OrderDto;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Application.Services;
using ClothingBrand.Web.helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ClothingBrand.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        //// POST: api/order/create
        [Authorize(Roles = "user,Admin")]  // Authorize Users and Admins to create orders
        [HttpPost("create")]
        public ActionResult<OrderDto> CreateOrder([FromBody] CreateOrderRequest request)
        {
            // Customize the request as needed based on your requirements
            var order = _orderService.CreateOrder(request.UserId, request.ShoppingCart, request.ShippingDetails);
            return CreatedAtAction(nameof(GetOrderById), new { orderId = order.OrderId }, order);
        }

        // GET: api/order
        [HttpGet]
       [Authorize(Roles ="Admin")]
        public ActionResult<IEnumerable<OrderSummaryDto>> GetOrders()
        {
            var orders = _orderService.GetOrders();
            var orderSummaries = new List<OrderSummaryDto>();

            foreach (var order in orders)
            {
                var orderSummary = new OrderSummaryDto
                {
                    orderId= order.OrderId,
                    OrderDate = order.OrderDate,
                    TotalPrice = order.TotalPrice,
                    PaymentStatus = order.PaymentStatus,
                    OrderStatus = order.OrderStatus,
                    AddressLine1 = order.ShippingDetails.AddressLine1,
                    AddressLine2 = order.ShippingDetails.AddressLine2,
                    City = order.ShippingDetails.City,
                    State = order.ShippingDetails.State,
                    PostalCode = order.ShippingDetails.PostalCode,
                    Country = order.ShippingDetails.Country,
                    OrderItems = new List<ItemDto>()
                };

                foreach (var item in order.OrderItems)
                {
                    orderSummary.OrderItems.Add(new ItemDto
                    {
                        Quantity = item.Quantity,
                        Price = item.Price,
                        ProductName = item.ProductName
                        ,ImageUrl = item.ImageUrl
                    });
                }

                orderSummaries.Add(orderSummary);
            }

            return Ok(orderSummaries);
        }

        [HttpGet("{orderId}")]
       [Authorize(Roles = "user,Admin")]
        public ActionResult<OrderSummaryDto> GetOrderById(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound(); // Return 404 if the order is not found
            }

            var orderSummary = new OrderSummaryDto
            {
                orderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                PaymentStatus = order.PaymentStatus,
                OrderStatus = order.OrderStatus,
                AddressLine1 = order.ShippingDetails.AddressLine1,
                AddressLine2 = order.ShippingDetails.AddressLine2,
                City = order.ShippingDetails.City,
                State = order.ShippingDetails.State,
                PostalCode = order.ShippingDetails.PostalCode,
                Country = order.ShippingDetails.Country,
                OrderItems = new List<ItemDto>() // Updated class name
            };

            foreach (var item in order.OrderItems)
            {
                orderSummary.OrderItems.Add(new ItemDto // Updated class name
                {
                    Quantity = item.Quantity,
                    Price = item.Price,
                    ProductName = item.ProductName,
                    ImageUrl = item.ImageUrl
                    
                });
            }

            return Ok(orderSummary);
        }


        // PUT: api/order/{id}/status
        [HttpPut("{orderId}/status")]
        [Authorize(Roles = "Admin")]  // Only Admin can update order status
        public ActionResult UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto dto)
        {
            _orderService.UpdateOrderStatus(orderId, dto);
            return NoContent();
        }

        // PUT: api/order/{id}/payment-status
        [HttpPut("{orderId}/payment-status")]
        [Authorize(Roles = "Admin")]  // Only Admin can update order status
        public ActionResult UpdatePaymentStatus(int orderId, [FromBody] string paymentStatus)
        {
            _orderService.UpdatePaymentStatus(orderId, paymentStatus);
            return NoContent();
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "user,Admin")]  // Users and Admins can view user-specific orders
        public ActionResult<IEnumerable<OrderSummaryDto>> GetUserOrders(string userId)
        {
            var orders = _orderService.GetUserOrders(userId);
            var orderSummaries = new List<OrderSummaryDto>();

            foreach (var order in orders)
            {
                var orderSummary = new OrderSummaryDto
                {
                    orderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    TotalPrice = order.TotalPrice,
                    PaymentStatus = order.PaymentStatus,
                    OrderStatus = order.OrderStatus,
                    AddressLine1 = order.ShippingDetails.AddressLine1,
                    AddressLine2 = order.ShippingDetails.AddressLine2,
                    City = order.ShippingDetails.City,
                    State = order.ShippingDetails.State,
                    PostalCode = order.ShippingDetails.PostalCode,
                    Country = order.ShippingDetails.Country,
                    OrderItems = new List<ItemDto>() // Updated class name
                };

                foreach (var item in order.OrderItems)
                {
                    orderSummary.OrderItems.Add(new ItemDto // Updated class name
                    {
                        Quantity = item.Quantity,
                        Price = item.Price,
                        ProductName = item.ProductName,
                        ImageUrl = item.ImageUrl
                    });
                }

                orderSummaries.Add(orderSummary);
            }

            return Ok(orderSummaries);
        }


        // DELETE: api/order/{id}/cancel
        [HttpDelete("{orderId}/cancel")]
        [Authorize(Roles = "user,Admin")]  // Users and Admins can cancel an order
        public ActionResult CancelOrder(int orderId)
        {
            var result = _orderService.CancelOrder(orderId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

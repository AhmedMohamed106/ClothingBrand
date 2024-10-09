using ClothingBrand.Application.Common.DTO.OrderDto;
using ClothingBrand.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothingBrand.Web.Controllers
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

        // POST: api/order
        [HttpPost]
        public IActionResult CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                var order =  _orderService.CreateOrder(createOrderDto);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/order/{id}
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            try
            {
                var order =  _orderService.GetOrderById(id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // PUT: api/order/{id}/status
        [HttpPut("{id}/status")]
        public IActionResult UpdateOrderStatus(int id, UpdateOrderStatusDto orderstatusdto)
        {
            try
            {
                 _orderService.UpdateOrderStatus(id, orderstatusdto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/order/{id}/payment-status
        [HttpPut("{id}/payment-status")]
        public IActionResult UpdatePaymentStatus(int id, [FromBody] string paymentStatus)
        {
            try
            {
                 _orderService.UpdatePaymentStatus(id, paymentStatus);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/order/user/{userId}
        [HttpGet("user/{userId}")]
        public IActionResult GetUserOrders(string userId)
        {
            try
            {
                var orders =  _orderService.GetUserOrders(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrderDto>> GetOrders()
        {
            var orders = _orderService.GetOrders();

            if (orders == null || !orders.Any())
            {
                return NotFound(); // Return 404 if no orders found
            }

            return Ok(orders); // Return 200 with orders data
        }

        [HttpPost("{orderId}/cancel")]
        public ActionResult CancelOrder(int orderId)
        {
            var result = _orderService.CancelOrder(orderId);

            if (!result)
            {
                return NotFound(); // Return 404 if the order is not found or not cancellable
            }

            return NoContent(); // Return 204 No Content if cancellation is successful
        }
    }
}

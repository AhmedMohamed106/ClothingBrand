using ClothingBrand.Application.Common.DTO.OrderDto;
using ClothingBrand.Application.Common.DTO.Response.Shipping;
using ClothingBrand.Application.Common.DTO.Response.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
    public interface IOrderService
    {
        OrderDto CreateOrder(string userId, ShoppingCartDto cart , ShippingDto shippingDetails); // Create a new order
        OrderDto GetOrderById(int orderId); // Retrieve an order by its ID
        void UpdateOrderStatus(int orderId, UpdateOrderStatusDto dto); // Update the order status (e.g., Pending, Shipped)
        void UpdatePaymentStatus(int orderId, string paymentStatus); // Update the payment status (e.g., Paid, Unpaid)
        IEnumerable<OrderDto> GetUserOrders(string userId); // Retrieve all orders for a particular user
        bool CancelOrder(int orderId); // Cancel an order

        IEnumerable<OrderDto> GetOrders();

    }

}

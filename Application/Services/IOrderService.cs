using ClothingBrand.Application.Common.DTO.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
    public interface IOrderService
    {
        OrderDto CreateOrder(CreateOrderDto createOrderDto); // Create a new order
        OrderDto GetOrderById(int orderId); // Retrieve an order by its ID
        void UpdateOrderStatus(int orderId, UpdateOrderStatusDto dto); // Update the order status (e.g., Pending, Shipped)
        void UpdatePaymentStatus(int orderId, string paymentStatus); // Update the payment status (e.g., Paid, Unpaid)
        IEnumerable<OrderDto> GetUserOrders(string userId); // Retrieve all orders for a particular user
        void CancelOrder(int orderId); // Cancel an order

        IEnumerable<OrderDto> GetOrders();

    }

}

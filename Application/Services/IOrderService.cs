//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ClothingBrand.Application.Services
//{
//    public interface IOrderService
//    {
//        Task<OrderDto> CreateOrderAsync(OrderCreateDto dto);
//        Task<OrderDto> GetOrderByIdAsync(int id);
//        Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId);
//        Task<OrderDto> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto dto);
//        Task<bool> ApplyDiscountAsync(int orderId, string discountCode);
//        Task<OrderDto> CancelOrderAsync(int id);
//    }
//}

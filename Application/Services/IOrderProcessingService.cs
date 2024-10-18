using ClothingBrand.Application.Common.DTO.OrderDto;
using ClothingBrand.Application.Common.DTO.Response.Shipping;
using ClothingBrand.Application.Common.DTO.Response.ShoppingCart;
using ClothingBrand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
    public interface IOrderProcessingService
    {
        OrderDto CreateOrder(string userId, ShoppingCartDto cart, ShippingDto shippingDetails);

         void UpdateOrderStatus(int orderId, string status);
        void UpdatePaymentStatus(int orderId, string paymentStatus);


    }
}

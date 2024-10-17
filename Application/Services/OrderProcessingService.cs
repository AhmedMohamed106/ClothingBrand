using ClothingBrand.Application.Common.DTO.OrderDto;
using ClothingBrand.Application.Common.DTO.Response.Shipping;
using ClothingBrand.Application.Common.DTO.Response.ShoppingCart;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
    public class OrderProcessingService : IOrderProcessingService
    {
        private readonly IOrderService _orderService;
        private readonly IUnitOfWork _unitOfWork;

        public OrderProcessingService(IOrderService orderService, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _unitOfWork = unitOfWork;
        }

        public OrderDto CreateOrder(string userId, ShoppingCartDto cart, ShippingDto shippingDetails)
        {
            if (cart == null || cart.ShoppingCartItems == null || cart.ShoppingCartItems.Count == 0)
            {
                throw new Exception("Shopping cart is empty or not initialized.");
            }

            if (shippingDetails == null)
            {
                throw new Exception("Shipping details are required.");
            }

            var newOrder = _orderService.CreateOrder(userId, cart, shippingDetails);
            if (newOrder == null)
            {
                throw new Exception("Failed to create the order.");
            }

            return newOrder;
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            var orderStatusDto = new UpdateOrderStatusDto { OrderStatus = status };
            _orderService.UpdateOrderStatus(orderId, orderStatusDto);
            _unitOfWork.Save();
        }

        public void UpdatePaymentStatus(int orderId, string paymentStatus)
        {
            _orderService.UpdatePaymentStatus(orderId, paymentStatus);
            _unitOfWork.Save();
        }
    }

}

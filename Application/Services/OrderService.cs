using ClothingBrand.Application.Common.DTO.OrderDto;
using ClothingBrand.Application.Common.DTO.Response.ShoppingCart;
using ClothingBrand.Application.Common.DTO.Response.Shipping;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClothingBrand.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public OrderDto CreateOrder(string userId, ShoppingCartDto cart, ShippingDto shippingDetails)
        {
            // Create order entity
            var newOrder = new Order
            {
                OrderDate = DateTime.UtcNow,
                TotalPrice = cart.TotalPrice,
                PaymentStatus = "Unpaid",
                OrderStatus = "Pending",
                UserId = userId,
                ShippingDetails = new Shipping // Assuming Shipping is an entity that maps to ShippingDto
                {
                    AddressLine1 = shippingDetails.AddressLine1,
                    AddressLine2 = shippingDetails.AddressLine2,
                    City = shippingDetails.City,
                    PostalCode = shippingDetails.PostalCode,
                    Country = shippingDetails.Country
                },
                OrderItems = cart.ShoppingCartItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price * item.Quantity
                }).ToList()
            };

            // Save order
            _unitOfWork.orderRepository.Add(newOrder);
            _unitOfWork.Save();

            return new OrderDto
            {
                OrderId = newOrder.OrderId,
                OrderDate = newOrder.OrderDate,
                TotalPrice = newOrder.TotalPrice,
                PaymentStatus = newOrder.PaymentStatus,
                OrderStatus = newOrder.OrderStatus,
                UserId = newOrder.UserId,
                ShippingDetails = new ShippingDto
                {
                    AddressLine1 = newOrder.ShippingDetails.AddressLine1,
                    AddressLine2 = newOrder.ShippingDetails.AddressLine2,
                    City = newOrder.ShippingDetails.City,
                    PostalCode = newOrder.ShippingDetails.PostalCode,
                    Country = newOrder.ShippingDetails.Country
                },
                OrderItems = newOrder.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    orderId = oi.OrderId,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };
        }

        public OrderDto GetOrderById(int orderId)
        {
            var order = _unitOfWork.orderRepository.Get(o => o.OrderId == orderId, includeProperties: "OrderItems,ShippingDetails");

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            return new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                PaymentStatus = order.PaymentStatus,
                OrderStatus = order.OrderStatus,
                UserId = order.UserId,
                ShippingDetails = new ShippingDto
                {
                    AddressLine1 = order.ShippingDetails.AddressLine1,
                    AddressLine2 = order.ShippingDetails.AddressLine2,
                    City = order.ShippingDetails.City,
                    PostalCode = order.ShippingDetails.PostalCode,
                    Country = order.ShippingDetails.Country
                },
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    orderId = oi.OrderId,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };
        }

        public void UpdateOrderStatus(int orderId, UpdateOrderStatusDto dto)
        {
            var order = _unitOfWork.orderRepository.Get(o => o.OrderId == orderId);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            order.OrderStatus = dto.OrderStatus;
            _unitOfWork.Save();
        }

        public void UpdatePaymentStatus(int orderId, string paymentStatus)
        {
            var order = _unitOfWork.orderRepository.Get(o => o.OrderId == orderId);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            order.PaymentStatus = paymentStatus;
            _unitOfWork.Save();
        }

        public IEnumerable<OrderDto> GetUserOrders(string userId)
        {
            var orders = _unitOfWork.orderRepository.GetBy(o => o.UserId == userId, includeProperties: "OrderItems,ShippingDetails");

            return orders.Select(order => new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                PaymentStatus = order.PaymentStatus,
                OrderStatus = order.OrderStatus,
                UserId = order.UserId,
                ShippingDetails = new ShippingDto
                {
                    AddressLine1 = order.ShippingDetails.AddressLine1,
                    AddressLine2 = order.ShippingDetails.AddressLine2,
                    City = order.ShippingDetails.City,
                    PostalCode = order.ShippingDetails.PostalCode,
                    Country = order.ShippingDetails.Country
                },
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    orderId = oi.OrderId,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            }).ToList();
        }

        public bool CancelOrder(int orderId)
        {
            var order = _unitOfWork.orderRepository.Get(o => o.OrderId == orderId);

            if (order == null || order.OrderStatus != "Pending")
            {
                return false;
            }

            order.OrderStatus = "Canceled";
            _unitOfWork.Save();
            return true;
        }

        public IEnumerable<OrderDto> GetOrders()
        {
            var orders = _unitOfWork.orderRepository.GetAll(includeProperties: "OrderItems,ShippingDetails");

            return orders.Select(order => new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                PaymentStatus = order.PaymentStatus,
                OrderStatus = order.OrderStatus,
                UserId = order.UserId,
                ShippingDetails = new ShippingDto
                {
                    AddressLine1 = order.ShippingDetails.AddressLine1,
                    AddressLine2 = order.ShippingDetails.AddressLine2,
                    City = order.ShippingDetails.City,
                    PostalCode = order.ShippingDetails.PostalCode,
                    Country = order.ShippingDetails.Country
                },
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    orderId = oi.OrderId,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            }).ToList();
        }

        
    }
}

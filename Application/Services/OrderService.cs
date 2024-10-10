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

        private decimal CalculateShippingCost(ShippingDto shippingDetails)
        {
            switch (shippingDetails.ShippingMethod.ToLower())
            {
                case "standard":
                    return 5.00m;
                case "express":
                    return 10.00m;
                case "international":
                    return 20.00m;
                default:
                    throw new Exception("Unknown shipping method.");
            }
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

            // Validate shipping details
            if (string.IsNullOrEmpty(shippingDetails.AddressLine1))
            {
                throw new Exception("Address Line 1 is required.");
            }
            // Add other validations as necessary...

            // Validate each item in the cart
            foreach (var item in cart.ShoppingCartItems)
            {
                if (item == null)
                {
                    throw new Exception("One or more items in the shopping cart are not initialized.");
                }
            }

            // Create order entity
            var newOrder = new Order
            {
                OrderDate = DateTime.UtcNow,
                TotalPrice = cart.TotalPrice,
                PaymentStatus = "Unpaid",
                OrderStatus = "Pending",
                UserId = userId,
                ShippingDetails = new Shipping
                {
                    AddressLine1 = shippingDetails.AddressLine1,
                    AddressLine2 = shippingDetails.AddressLine2,
                    City = shippingDetails.City,
                    PostalCode = shippingDetails.PostalCode,
                    Country = shippingDetails.Country,
                    State = shippingDetails.State,
                    ShippingMethod = shippingDetails.ShippingMethod,
                    ShippingCost = CalculateShippingCost(shippingDetails) // Ensure Shipping entity has this property
                },
                OrderItems = cart.ShoppingCartItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price * item.Quantity
                }).ToList()
            };

            // Validate newOrder
            if (newOrder == null || newOrder.OrderItems == null || newOrder.OrderItems.Count == 0)
            {
                throw new Exception("Failed to create the order entity.");
            }

            // Ensure _unitOfWork and its repository are initialized
            if (_unitOfWork == null || _unitOfWork.orderRepository == null)
            {
                throw new Exception("Database context is not initialized.");
            }

            // Save order
            _unitOfWork.orderRepository.Add(newOrder);
            _unitOfWork.Save();

            // Return order DTO (consider implementing a method to map Order to OrderDto)
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

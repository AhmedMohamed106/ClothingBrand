using ClothingBrand.Application.Common.DTO.OrderDto;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Application.Services;
using ClothingBrand.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
   
        public class OrderService : IOrderService
        {
            private readonly IUnitOfWork _unitOfWork;

            public OrderService(
                IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public OrderDto CreateOrder(CreateOrderDto createOrderDto)
            {
                // Fetch the user and shopping cart
                var user =  _unitOfWork.applicationUserRepository.Get(p => p.Id == createOrderDto.UserId);
                var shoppingCart =  _unitOfWork.shoppingCartRepository.Get(p=>p.Id == createOrderDto.ShoppingCartId);

                if (user == null || shoppingCart == null || !shoppingCart.ShoppingCartItems.Any())
                {
                    throw new Exception("Invalid user or shopping cart");
                }

                // Calculate total price
                decimal totalPrice = shoppingCart.ShoppingCartItems.Sum(item => item.Quantity * item.Product.Price);

                // Create order entity
                var newOrder = new Order
                {
                    OrderDate = DateTime.Now,
                    TotalPrice = totalPrice,
                    PaymentStatus = "Unpaid",
                    OrderStatus = "Pending",
                    ShippingId = createOrderDto.ShippingId,
                    UserId = createOrderDto.UserId,
                    ShoppingCartId = createOrderDto.ShoppingCartId,
                    OrderItems = shoppingCart.ShoppingCartItems.Select(item => new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Product.Price * item.Quantity
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
                    ShippingId = newOrder.ShippingId,
                    OrderItems = newOrder.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                        Price = oi.Price
                    }).ToList()
                };
            }

            public OrderDto GetOrderById(int orderId)
            {
                var order =  _unitOfWork.orderRepository.Get(p=>p.OrderId == orderId);

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
                    ShippingId = order.ShippingId,
                    OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                        Price = oi.Price
                    }).ToList()
                };
            }

            public void UpdateOrderStatus(int orderId, UpdateOrderStatusDto dto)
            {
                var order =  _unitOfWork.orderRepository.Get(p=>p.OrderId ==  orderId);

                if (order == null)
                {
                    throw new Exception("Order not found");
                }

                order.OrderStatus = dto.OrderStatus;
                 _unitOfWork.Save();
            }

            public void UpdatePaymentStatus(int orderId, string paymentStatus)
            {
                var order =  _unitOfWork.orderRepository.Get(p=>p.OrderId == orderId);

                if (order == null)
                {
                    throw new Exception("Order not found");
                }

                order.PaymentStatus = paymentStatus;
                 _unitOfWork.Save();
            }

            public IEnumerable<OrderDto> GetUserOrders(string userId)
            {
                var orders =  _unitOfWork.orderRepository.GetBy(p=>p.UserId == userId , includeProperties: "OrderItems");

                return orders.Select(order => new OrderDto
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    TotalPrice = order.TotalPrice,
                    PaymentStatus = order.PaymentStatus,
                    OrderStatus = order.OrderStatus,
                    UserId = order.UserId,
                    ShippingId = order.ShippingId,
                    OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                        Price = oi.Price
                    }).ToList()
                });
            }

            public bool CancelOrder(int orderId)
            {
                var order =  _unitOfWork.orderRepository.Get(p=>p.OrderId == orderId);

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
            var orders = _unitOfWork.orderRepository.GetAll(includeProperties: "OrderItems");

            return orders.Select(order => new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                PaymentStatus = order.PaymentStatus,
                OrderStatus = order.OrderStatus,
                UserId = order.UserId,
                ShippingId = order.ShippingId,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            });
        }
    }
}









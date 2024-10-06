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
    public class ShippingService : IShippingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShippingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ShippingDto CreateShipping(ShippingDto shippingDto)
        {
            var shipping = new Shipping
            {
                Address = shippingDto.Address,
                City = shippingDto.City,
                PostalCode = shippingDto.PostalCode,
                PhoneNumber = shippingDto.PhoneNumber,

                // Map the list of OrderDtos to Order entities
                Orders = shippingDto.orders != null
             ? shippingDto.orders.Select(orderDto => new Order
             {
                 OrderId = orderDto.OrderId,
                 OrderDate = orderDto.OrderDate,
                 ShippingId= orderDto.ShippingId,
                 PaymentStatus= orderDto.PaymentStatus,
                 OrderStatus= orderDto.OrderStatus,
                 PaymentId = orderDto.PaymentId,
                 TotalPrice = orderDto.TotalPrice,
                 UserId = orderDto.UserId,
                 OrderItems = orderDto.OrderItems != null
                    ? orderDto.OrderItems.Select(orderItemDto => new OrderItem
                    {
                        OrderItemId = orderItemDto.Id, // Assuming order items already exist
                        ProductId = orderItemDto.ProductId,
                        Quantity = orderItemDto.Quantity,
                        Price = orderItemDto.Price,
                        OrderId= orderItemDto.OrderId
                        // Map any other properties as needed
                    }).ToList()
                    : new List<OrderItem>()

             }).ToList()
             : new List<Order>()  // If no orders, initialize an empty list
            };

            _unitOfWork.shippingRepository.Add(shipping);
            _unitOfWork.Save();

            // Manually map to DTO
            var createdShippingDto = new ShippingDto
            {
                Id = shipping.Id,
                Address = shipping.Address,
                City = shipping.City,
                PostalCode = shipping.PostalCode,
                PhoneNumber = shipping.PhoneNumber,
               orders = shipping.Orders.Select(order => new OrderDto
               {
                   OrderId = order.OrderId, // Assuming you have an OrderId property
                   OrderDate = order.OrderDate,
                   ShippingId = order.ShippingId,
                   PaymentStatus = order.PaymentStatus,
                   OrderStatus = order.OrderStatus,
                   PaymentId = order.PaymentId,
                   TotalPrice = order.TotalPrice,
                   UserId = order.UserId,
                   OrderItems = order.OrderItems.Select(orderItem => new OrderItemDto
                   {
                       Id = orderItem.OrderItemId,
                       ProductId = orderItem.ProductId,
                       Quantity = orderItem.Quantity,
                       Price = orderItem.Price,
                       OrderId = orderItem.OrderId // Ensure this references the correct OrderId
                   }).ToList()
               }).ToList()
            };

            return createdShippingDto;
        }

        public ShippingDto GetShippingById(int shippingId)
        {
            var shipping = _unitOfWork.ShippingRepository.GetById(shippingId);

            if (shipping == null)
            {
                return null;
            }

            // Manually map to DTO
            var shippingDto = new ShippingDto
            {
                Id = shipping.Id,
                Address = shipping.Address,
                City = shipping.City,
                PostalCode = shipping.PostalCode,
                PhoneNumber = shipping.PhoneNumber,
                orders = shipping.Orders.Select(order => new OrderDto
                {
                    OrderId = order.OrderId, // Assuming you have an OrderId property
                    OrderDate = order.OrderDate,
                    ShippingId = order.ShippingId,
                    PaymentStatus = order.PaymentStatus,
                    OrderStatus = order.OrderStatus,
                    PaymentId = order.PaymentId,
                    TotalPrice = order.TotalPrice,
                    UserId = order.UserId,
                    OrderItems = order.OrderItems.Select(orderItem => new OrderItemDto
                    {
                        Id = orderItem.OrderItemId,
                        ProductId = orderItem.ProductId,
                        Quantity = orderItem.Quantity,
                        Price = orderItem.Price,
                        OrderId = orderItem.OrderId // Ensure this references the correct OrderId
                    }).ToList()

                }).ToList()
            };

            return shippingDto;
        }
    }

}

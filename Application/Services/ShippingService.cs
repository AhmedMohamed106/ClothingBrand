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
                        OrderItemId = orderItemDto.OrderItemId, // Assuming order items already exist
                        ProductId = orderItemDto.ProductId,
                        Quantity = orderItemDto.Quantity,
                        Price = orderItemDto.Price,
                        OrderId = orderItemDto.orderId
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
                       OrderItemId = orderItem.OrderItemId,
                       ProductId = orderItem.ProductId,
                       Quantity = orderItem.Quantity,
                       Price = orderItem.Price,
                       orderId = orderItem.OrderId // Ensure this references the correct OrderId
                   }).ToList()

               }).ToList()
            };

            return createdShippingDto;
        }
        public IEnumerable<ShippingDto> GetAllShipping()
        {
            var shippings = _unitOfWork.shippingRepository.GetAll(includeProperties: "Orders");
            return shippings.Select(shipping => new ShippingDto
            {
                Id = shipping.Id,
                Address = shipping.Address,
                City = shipping.City,
                PostalCode = shipping.PostalCode,
                PhoneNumber = shipping.PhoneNumber,
                orders = shipping.Orders.Select(order => new OrderDto
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    ShippingId = order.ShippingId,
                    PaymentStatus = order.PaymentStatus,
                    OrderStatus = order.OrderStatus,
                    PaymentId = order.PaymentId,
                    TotalPrice = order.TotalPrice,
                    UserId = order.UserId,
                    OrderItems = order.OrderItems.Select(orderItem => new OrderItemDto
                    {
                        OrderItemId = orderItem.OrderItemId,
                        ProductId = orderItem.ProductId,
                        Quantity = orderItem.Quantity,
                        Price = orderItem.Price,
                        orderId = orderItem.OrderId
                    }).ToList()
                }).ToList()
            });
        }

        public ShippingDto GetShippingById(int shippingId)
        {
            var shipping = _unitOfWork.shippingRepository.Get(p=>p.Id == shippingId);

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
                        OrderItemId = orderItem.OrderItemId,
                        ProductId = orderItem.ProductId,
                        Quantity = orderItem.Quantity,
                        Price = orderItem.Price,
                        orderId = orderItem.OrderId // Ensure this references the correct OrderId
                    }).ToList()

                }).ToList()
            };

            return shippingDto;
        }

        public ShippingDto UpdateShipping(int shippingId, ShippingDto shippingDto)
        {
            if (shippingDto == null)
                throw new ArgumentNullException(nameof(shippingDto));

            var shipping = _unitOfWork.shippingRepository.Get(
                p => p.Id == shippingId,
                includeProperties: "Orders.OrderItems");

            if (shipping == null)
                throw new KeyNotFoundException($"Shipping with ID {shippingId} not found.");

            // Update shipping fields
            shipping.Address = shippingDto.Address;
            shipping.City = shippingDto.City;
            shipping.PostalCode = shippingDto.PostalCode;
            shipping.PhoneNumber = shippingDto.PhoneNumber;

            // Optionally, update orders and order items if needed
            // For simplicity, assuming orders are managed separately

            _unitOfWork.shippingRepository.Update(shipping);
            _unitOfWork.Save();

            // Map to DTO
            var updatedShippingDto = new ShippingDto
            {
                Id = shipping.Id,
                Address = shipping.Address,
                City = shipping.City,
                PostalCode = shipping.PostalCode,
                PhoneNumber = shipping.PhoneNumber,
                orders = shipping.Orders.Select(order => new OrderDto
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    ShippingId = order.ShippingId,
                    PaymentStatus = order.PaymentStatus,
                    OrderStatus = order.OrderStatus,
                    PaymentId = order.PaymentId,
                    TotalPrice = order.TotalPrice,
                    UserId = order.UserId,
                    OrderItems = order.OrderItems.Select(orderItem => new OrderItemDto
                    {
                        OrderItemId = orderItem.OrderItemId,
                        ProductId = orderItem.ProductId,
                        Quantity = orderItem.Quantity,
                        Price = orderItem.Price,
                        orderId = orderItem.OrderId
                    }).ToList()
                }).ToList()
            };

            return updatedShippingDto;
        }

        public void DeleteShipping(int shippingId)
        {
            var shipping = _unitOfWork.shippingRepository.Get(
                p => p.Id == shippingId,
                includeProperties: "Orders");

            if (shipping == null)
                throw new KeyNotFoundException($"Shipping with ID {shippingId} not found.");

            // Optionally, check if there are any orders associated with this shipping
            if (shipping.Orders != null && shipping.Orders.Any())
                throw new InvalidOperationException("Cannot delete shipping because it has associated orders.");

            _unitOfWork.shippingRepository.Remove(shipping);
            _unitOfWork.Save();
        }
    }

}

using ClothingBrand.Application.Common.DTO.OrderDto;
using ClothingBrand.Application.Common.DTO.Response.Payment;
using ClothingBrand.Application.Common.DTO.Response.Shipping;
using ClothingBrand.Application.Common.DTO.Response.ShoppingCart;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClothingBrand.Application.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;

        public ShoppingCartService(IUnitOfWork unitOfWork, IOrderService orderService, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _orderService = orderService;
            _paymentService = paymentService;
        }

        public ShoppingCartDto GetShoppingCart(string userId)
        {
            // Retrieve the user's shopping cart
            var cart = _unitOfWork.shoppingCartRepository.Get(c => c.UserId == userId , includeProperties: "ShoppingCartItems.Product");
            return new ShoppingCartDto
            {
                TotalPrice = cart.TotalPrice,
                ShoppingCartItems = cart.ShoppingCartItems.Select(item => new ShoppingCartItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ProductName = item.Product.Name,
                    Price = item.Price
                }).ToList()
            };
        }

        public void AddToCart(string userId, ShoppingCartItemDto item)
        {
            var product = _unitOfWork.productRepository.Get(p => p.Id == item.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }

           
            // Add item to the shopping cart
            var cart = _unitOfWork.shoppingCartRepository.Get(c => c.UserId == userId);
            if (cart == null)
            {
                // Create a new cart if it doesn't exist
                cart = new ShoppingCart { UserId = userId };
                _unitOfWork.shoppingCartRepository.Add(cart);
            }
            item.ProductName = product.Name;
            item.Price = product.Price;

            // Update cart item quantity or add new item
            var existingItem = cart.ShoppingCartItems.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cart.ShoppingCartItems.Add(new ShoppingCartItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ShoppingCartId = item.ShoppingCartId
                    //Price = item.Price // Assume price is the price per item
                });
            }

            
            
            _unitOfWork.Save();
        }

        public void RemoveFromCart(string userId, int productId)
        {
            // Retrieve the shopping cart
            var cart = _unitOfWork.shoppingCartRepository.Get(c => c.UserId == userId);
            if (cart == null)
            {
                throw new Exception("Shopping cart not found.");
            }

            // Find the shopping cart item to remove
            var existingItem = cart.ShoppingCartItems.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                // Remove the item from the shopping cart
                cart.ShoppingCartItems.Remove(existingItem);
                _unitOfWork.Save();
            }
            else
            {
                throw new Exception("Item not found in the shopping cart.");
            }
        }

        public decimal GetTotalPriceWithShipping(string userId, ShippingDto shippingDetails)
        {
            // Retrieve the shopping cart
            var cart = _unitOfWork.shoppingCartRepository.Get(c => c.UserId == userId);
            if (cart == null)
            {
                throw new Exception("Shopping cart not found.");
            }

            // Calculate total price including shipping
            decimal shippingCost = CalculateShippingCost(shippingDetails); // Get the shipping cost from ShippingDto
            return cart.TotalPrice + shippingCost;
        }

        private decimal CalculateShippingCost(ShippingDto shippingDetails)
        {
            decimal shippingCost = 0;

            // Example of shipping cost calculation based on shipping method
            switch (shippingDetails.ShippingMethod)
            {
                case "Standard":
                    shippingCost = 5.00m; // Example: flat rate for standard shipping
                    break;
                case "Express":
                    shippingCost = 10.00m; // Example: higher rate for express shipping
                    break;
                case "International":
                    shippingCost = 20.00m; // Example: rate for international shipping
                    break;
                default:
                    throw new Exception("Unknown shipping method.");
            }

            return shippingCost;
        }


        public OrderDto Checkout(string userId, ShippingDto shippingDetails, PaymentDto paymentDto)
        {
            // Retrieve shopping cart
            var cart = GetShoppingCart(userId);
            if (cart.ShoppingCartItems.Count == 0)
            {
                throw new Exception("Shopping cart is empty.");
            }

            decimal totalPrice = GetTotalPriceWithShipping(userId, shippingDetails);

            paymentDto.Amount = totalPrice;
            // Create the order
            var order = _orderService.CreateOrder(userId, cart, shippingDetails);

            // Process payment
            var paymentResult = _paymentService.ProcessPayment(paymentDto);
            if (!paymentResult.IsSuccessful)
            {
                throw new Exception(paymentResult.Message);
            }

            // Update payment status on order
            _orderService.UpdatePaymentStatus(order.OrderId, "Paid");

            // Clear shopping cart after successful checkout
            ClearCart(userId);

            return order;
        }

        public void ClearCart(string userId)
        {
            // Clear the shopping cart
            var cart = _unitOfWork.shoppingCartRepository.Get(c => c.UserId == userId);
            if (cart != null)
            {
                _unitOfWork.shoppingCartRepository.Remove(cart);
                _unitOfWork.Save();
            }
        }
    }
}

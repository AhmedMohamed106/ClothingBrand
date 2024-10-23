using ClothingBrand.Application.Common.DTO.OrderDto;
using ClothingBrand.Application.Common.DTO.Response.Payment;
using ClothingBrand.Application.Common.DTO.Response.Shipping;
using ClothingBrand.Application.Common.DTO.Response.ShoppingCart;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
        private readonly IOrderProcessingService _orderProcessingService;
        private string imagesPath;
        private readonly IConfiguration _configuration;
        private readonly string ApplicationUrl;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShoppingCartService(IUnitOfWork unitOfWork, IOrderService orderService, IPaymentService paymentService, IOrderProcessingService orderProcessingService, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _orderService = orderService;
            _paymentService = paymentService;
            _orderProcessingService = orderProcessingService;
            imagesPath = "/images/";
            _configuration = configuration;
            ApplicationUrl = _configuration["ApplicationUrl"];
            _userManager = userManager;
        }

        public ShoppingCartDto GetShoppingCart(string userId)
        {
            // Retrieve the user's shopping cart
            var cart = _unitOfWork.shoppingCartRepository.Get(c => c.UserId == userId , includeProperties: "ShoppingCartItems.Product" , tracked:true);

            if(cart == null)
            {
                return null;
            }

            return new ShoppingCartDto
            {
                Id = cart.Id,
                TotalPrice = cart.TotalPrice,
                UserId = cart.UserId,
                ShoppingCartItems = cart.ShoppingCartItems.Select(item => new ShoppingCartItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ProductName = item.Product != null ? item.Product.Name : "Unknown", // Safe null check
                    Price = item.Price,
                    imageUrl = ApplicationUrl + imagesPath + item.Product?.ImageUrl,
                    ShoppingCartId = item.ShoppingCartId
                    
                }).ToList()
            };

        }

        public void AddToCart(string userId, ShoppingCartItemDto item)
        {
            var user = _unitOfWork.applicationUserRepository.Get(u => u.Id == userId , tracked: true);
            var user2=_userManager.FindByIdAsync(user.Id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            // Get the product from the repository
            var product = _unitOfWork.productRepository.Get(p => p.Id == item.ProductId , tracked: true);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            // Retrieve or create the shopping cart with ShoppingCartItems and Product included
            var cart = _unitOfWork.shoppingCartRepository.Get(
                c => c.UserId == userId,
                includeProperties: "ShoppingCartItems.Product" , tracked: true
            );

            if (cart == null)
            {
                // Create a new cart if it doesn't exist
                cart = new ShoppingCart
                {
                    UserId = userId,
                    ShoppingCartItems = new List<ShoppingCartItem>()
                };
                _unitOfWork.shoppingCartRepository.Add(cart);
            }

            // Check if the item already exists in the cart
            var existingItem = cart.ShoppingCartItems.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                // Update the quantity of the existing item
                existingItem.Quantity += item.Quantity;
                // Optionally, update the price if product prices can change dynamically
                existingItem.Price = product.Price;
            }
            else
            {
                // Add the new item to the cart using navigation property
                var cartItem = new ShoppingCartItem
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Product = product,       // Establish relationship via navigation property
                    Price = product.Price    // Set price based on the product
                };
                cart.ShoppingCartItems.Add(cartItem);
            }

            // Save changes to the database
            _unitOfWork.Save();
        }


        public void RemoveFromCart(string userId, int productId)
        {
            // Retrieve the shopping cart
            var cart = _unitOfWork.shoppingCartRepository.Get(c => c.UserId == userId, includeProperties: "ShoppingCartItems", tracked: true);
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
                var itemIdsInCart = cart.ShoppingCartItems.Select(i => i.ProductId).ToList();
                throw new Exception("Item not found in the shopping cart.");
            }
        }

        public decimal GetTotalPriceWithShipping(string userId, ShippingDto shippingDetails)
        {
            // Retrieve the shopping cart
            var cart = _unitOfWork.shoppingCartRepository.Get(c => c.UserId == userId, tracked: true);
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
                case "NA":
                    shippingCost = 0m;
                    break;
                default:
                    
                    throw new Exception("Unknown shipping method.");
            }

            return shippingCost;
        }
        public OrderDto Checkout(string userId, ShippingDto shippingDetails)
        {
            // Retrieve shopping cart
            var cart = GetShoppingCart(userId);
            if (cart == null || cart.ShoppingCartItems == null || cart.ShoppingCartItems.Count == 0)
            {
                throw new Exception("Shopping cart is empty or not initialized.");
            }

            // Check if shipping details are provided
            if (shippingDetails == null)
            {
                throw new Exception("Shipping details are required.");
            }
           
            // Create the order with status "Pending Payment"
            var newOrder = _orderProcessingService.CreateOrder(userId, cart, shippingDetails);

            // Optionally, return the order details to show the user before proceeding with payment
            return newOrder;
        }


        //public OrderDto Checkout(string userId, ShippingDto shippingDetails, PaymentDto paymentDto)
        //{
        //    // Retrieve shopping cart
        //    var cart = GetShoppingCart(userId);
        //    if (cart == null || cart.ShoppingCartItems == null || cart.ShoppingCartItems.Count == 0)
        //    {
        //        throw new Exception("Shopping cart is empty or not initialized.");
        //    }

        //    // Check if shipping details are provided
        //    if (shippingDetails == null)
        //    {
        //        throw new Exception("Shipping details are required.");
        //    }

        //    // Calculate total price including shipping
        //    decimal shippingCost = CalculateShippingCost(shippingDetails); // Assuming you have this method
        //    decimal totalPrice = cart.TotalPrice + shippingCost;

        //    paymentDto.Amount = (long)( totalPrice * 100);  // converts dollars to cents

        //    // Create the order
        //    var order = _orderService.CreateOrder(userId, cart, shippingDetails);
        //    if (order == null)
        //    {
        //        throw new Exception("Failed to create the order.");
        //    }

        //    // Process payment
        //    var paymentResult = _paymentService.ProcessPayment(paymentDto).Result;
        //    if (!paymentResult.IsSuccessful)
        //    {
        //        throw new Exception(paymentResult.Message);
        //    }
        //    UpdateOrderStatusDto orderstatus = new UpdateOrderStatusDto();

        //    orderstatus.OrderStatus = "Confirmed";

        //    // Update payment status on order
        //    _orderService.UpdatePaymentStatus(order.OrderId, "Paid");
        //    _orderService.UpdateOrderStatus(order.OrderId, orderstatus);
        //    _unitOfWork.Save();
        //    // Clear shopping cart after successful checkout
        //    ClearCart(userId);

        //    return order;
        //}

        public PaymentResultDto ProceedToPayment(string userId , int orderId, PaymentDto paymentDto)
        {
            // Retrieve the existing order
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            // Calculate total price to charge
            paymentDto.Amount = (long)(order.TotalPrice * 100);  // Convert to cents if using a payment gateway like Stripe

            // Process payment
            var paymentResult = _paymentService.ProcessPayment(paymentDto).Result;
            if (!paymentResult.IsSuccessful)
            {
                throw new Exception(paymentResult.Message);
            }

            // Update payment and order status
            _orderProcessingService.UpdatePaymentStatus(order.OrderId, "Paid");
            _orderProcessingService.UpdateOrderStatus(order.OrderId, "Confirmed");
            _unitOfWork.Save();

            ClearCart(userId);
            // Return payment result or order confirmation
            return new PaymentResultDto
            {
                IsSuccessful = true,
                Message = "Payment successful. Order confirmed."
            };
        }




        public void ClearCart(string userId)
        {
            // Clear the shopping cart
            var cart = _unitOfWork.shoppingCartRepository.Get(c => c.UserId == userId, tracked: true);
            if (cart != null)
            {
                _unitOfWork.shoppingCartRepository.Remove(cart);
                _unitOfWork.Save();
            }
        }
    }
}

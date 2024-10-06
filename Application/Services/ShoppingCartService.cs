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
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ShoppingCartDto GetShoppingCartByUserId(string userId)
        {
            var cart = _unitOfWork.shoppingCartRepository.Get(p=>p.UserId == userId);

            if (cart == null)
            {
                return null;
            }

            var cartDto = new ShoppingCartDto
            {
                Id = cart.Id,
                TotalPrice = cart.TotalPrice,
                UserId = cart.UserId,
                ShoppingCartItems = cart.ShoppingCartItems.Select(item => new ShoppingCartItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = _unitOfWork.productRepository.Get(p=>p.Id == item.ProductId)?.Name, // get product name
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            return cartDto;
        }

        public ShoppingCartDto AddItemToCart(int cartId, ShoppingCartItemDto itemDto)
        {
            var cart = _unitOfWork.shoppingCartRepository.Get(p=>p.Id == cartId);
            var product = _unitOfWork.productRepository.Get(p=>p.Id == itemDto.ProductId);

            if (product == null) throw new Exception("Product not found");

            var cartItem = new ShoppingCartItem
            {
                ProductId = product.Id,
                ShoppingCartId = cart.Id,
                Quantity = itemDto.Quantity,
                Price = product.Price
                
            };

            cart.ShoppingCartItems.Add(cartItem);
            _unitOfWork.Save();

            // Manually map back the updated cart to DTO
            var cartDto = new ShoppingCartDto
            {
                Id = cart.Id,
                TotalPrice = cart.TotalPrice,
                UserId = cart.UserId,
                ShoppingCartItems = cart.ShoppingCartItems.Select(item => new ShoppingCartItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = product.Name,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            return cartDto;
        }

        public void RemoveItemFromCart(int cartId, int itemId)
        {
            var cart = _unitOfWork.shoppingCartRepository.Get(p=>p.Id == cartId);
            var item = cart.ShoppingCartItems.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                cart.ShoppingCartItems.Remove(item);
                _unitOfWork.Save();
            }
        }

        public void ClearCart(int cartId)
        {
            var cart = _unitOfWork.shoppingCartRepository.Get(p=>p.Id == cartId);
            cart.ShoppingCartItems.Clear();
            _unitOfWork.Save();
        }
    }

}

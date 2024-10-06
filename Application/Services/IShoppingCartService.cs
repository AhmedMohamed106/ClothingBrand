using ClothingBrand.Application.Common.DTO.Response.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
    public interface IShoppingCartService
    {
        ShoppingCartDto GetShoppingCartByUserId(string userId);
        ShoppingCartDto AddItemToCart(int cartId, ShoppingCartItemDto itemDto);
        void RemoveItemFromCart(int cartId, int itemId);
        void ClearCart(int cartId);
    }

}

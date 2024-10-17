using ClothingBrand.Application.Common.DTO.OrderDto;
using ClothingBrand.Application.Common.DTO.Response.Payment;
using ClothingBrand.Application.Common.DTO.Response.Shipping;
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
        ShoppingCartDto GetShoppingCart(string userId);
        void AddToCart(string userId, ShoppingCartItemDto item);
        //OrderDto Checkout(string userId, ShippingDto shippingDetails, PaymentDto paymentDto);
        OrderDto Checkout(string userId, ShippingDto shippingDetails);
        PaymentResultDto ProceedToPayment(string userId , int orderId, PaymentDto paymentDto);
        void ClearCart(string userId);
        void RemoveFromCart(string userId, int productId);
    }


}

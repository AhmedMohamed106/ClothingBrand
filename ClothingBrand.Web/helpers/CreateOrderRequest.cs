using ClothingBrand.Application.Common.DTO.Response.Shipping;
using ClothingBrand.Application.Common.DTO.Response.ShoppingCart;

namespace ClothingBrand.Web.helpers
{
    public class CreateOrderRequest
    {
        public string UserId { get; set; }
        public ShoppingCartDto ShoppingCart { get; set; }
        public ShippingDto ShippingDetails { get; set; }
    }
}

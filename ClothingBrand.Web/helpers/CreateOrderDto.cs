namespace ClothingBrand.Web.helpers
{
    public class CreateOrderDto
    {
        public List<AddToCartRequestDto> ShoppingCartItems { get; set; } // Products and quantities
        public CheckoutShippingDto ShippingDetails { get; set; } // Shipping details
    }
}

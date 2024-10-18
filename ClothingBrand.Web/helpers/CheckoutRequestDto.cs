using ClothingBrand.Application.Common.DTO.Response.Payment;
using ClothingBrand.Application.Common.DTO.Response.Shipping;

namespace ClothingBrand.Web.helpers
{
    public class CheckoutRequestDto
    {
        public string userId { get; set; }
        public ShippingDto ShippingDetails { get; set; }
        //public PaymentDetailsDto PaymentDto { get; set; }
    }
}

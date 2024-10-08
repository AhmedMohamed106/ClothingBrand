using ClothingBrand.Application.Common.DTO.Response.Payment;
using ClothingBrand.Application.Common.DTO.Response.Shipping;

namespace ClothingBrand.Web.helpers
{
    public class CheckoutRequestDto
    {
        public CheckoutShippingDto ShippingDetails { get; set; }
        public PaymentDetailsDto PaymentDetails { get; set; }
    }
}

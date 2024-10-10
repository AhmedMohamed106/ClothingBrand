namespace ClothingBrand.Web.helpers
{
    public class PaymentDetailsDto
    {
        public string CardNumber { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string Cvc { get; set; }
    }

}

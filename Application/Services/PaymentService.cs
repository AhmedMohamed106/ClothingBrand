using ClothingBrand.Application.Common.DTO.Response.Payment;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Application.Services;
using ClothingBrand.Application.Settings;
using Microsoft.Extensions.Options;
using Stripe;

namespace ClothingBrand.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly StripeSettings _stripeSettings;

        public PaymentService(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        public PaymentResultDto ProcessPayment(PaymentDto paymentDto)
        {
            var options = new ChargeCreateOptions
            {
                Amount = (long)(paymentDto.Amount * 100),  // Convert to cents
                Currency = paymentDto.Currency,
                Source = GenerateStripeToken(paymentDto),
                Description = "Order Payment"
            };

            var service = new ChargeService();
            Charge charge;

            try
            {
                charge = service.Create(options);

                if (charge.Status == "succeeded")
                {
                    return new PaymentResultDto
                    {
                        IsSuccessful = true,
                        TransactionId = charge.Id,
                        Message = "Payment successful"
                    };
                }
                else
                {
                    return new PaymentResultDto
                    {
                        IsSuccessful = false,
                        TransactionId = charge.Id,
                        Message = charge.FailureMessage
                    };
                }
            }
            catch (Exception ex)
            {
                return new PaymentResultDto
                {
                    IsSuccessful = false,
                    TransactionId = null,
                    Message = ex.Message
                };
            }
        }

        private string GenerateStripeToken(PaymentDto paymentDto)
        {
            if (string.IsNullOrEmpty(paymentDto.ExpirationMonth) ||
             !int.TryParse(paymentDto.ExpirationMonth, out int expMonth) || expMonth < 1 || expMonth > 12)
            {
                throw new ArgumentException("Invalid expiration month. It should be between 1 and 12.");
            }

            if (string.IsNullOrEmpty(paymentDto.ExpirationYear) ||
                !int.TryParse(paymentDto.ExpirationYear, out int expYear) || expYear < DateTime.Now.Year)
            {
                throw new ArgumentException("Invalid expiration year. It should be the current year or later.");
            }
            var tokenOptions = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Number = paymentDto.CardNumber,
                    ExpMonth = paymentDto.ExpirationMonth,
                    ExpYear = paymentDto.ExpirationYear,
                    Cvc = paymentDto.Cvc,
                    
                }
            };

            var tokenService = new TokenService();
            Token stripeToken = tokenService.Create(tokenOptions);
            return stripeToken.Id;
        }
    }
}

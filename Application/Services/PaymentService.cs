﻿using ClothingBrand.Application.Common.DTO.Response.Payment;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Application.Services;
using ClothingBrand.Application.Settings;
using ClothingBrand.Domain.Models;
using Microsoft.Extensions.Options;
using Stripe;
using System.Diagnostics;

namespace ClothingBrand.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly StripeSettings _stripeSettings;
        private readonly Stripe.PaymentIntentService _paymentIntentService;

        public PaymentService(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            _paymentIntentService = new Stripe.PaymentIntentService();
        }

        public async Task<PaymentResultDto> ProcessPayment(PaymentDto paymentDto)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = paymentDto.Amount,  // Convert to cents
                Currency = paymentDto.Currency,
                PaymentMethod = paymentDto.PaymentMethodId,
                ConfirmationMethod = "manual",
                Confirm = true
            };

            var service = new PaymentIntentService();
            try
            {
                var paymentIntent = await service.CreateAsync(options);

                return new PaymentResultDto
                {
                    IsSuccessful = true,
                    Message = "Payment processed successfully",
                    RequiresAction = paymentIntent.Status == "requires_action",
                    PaymentIntentClientSecret = paymentIntent.ClientSecret
                    
                };
            }
            catch (StripeException ex)
            {
                return new PaymentResultDto { 
                    IsSuccessful = false,
                    Message = ex.Message,
                    RequiresAction = false,
                    PaymentIntentClientSecret = null
                };
            }
        }
        }

        //private string GenerateStripeToken(PaymentDto paymentDto)
        //{
        //    if (string.IsNullOrEmpty(paymentDto.ExpirationMonth) ||
        //     !int.TryParse(paymentDto.ExpirationMonth, out int expMonth) || expMonth < 1 || expMonth > 12)
        //    {
        //        throw new ArgumentException("Invalid expiration month. It should be between 1 and 12.");
        //    }

        //    if (string.IsNullOrEmpty(paymentDto.ExpirationYear) ||
        //        !int.TryParse(paymentDto.ExpirationYear, out int expYear) || expYear < DateTime.Now.Year)
        //    {
        //        throw new ArgumentException("Invalid expiration year. It should be the current year or later.");
        //    }
        //    var tokenOptions = new TokenCreateOptions
        //    {
        //        Card = new TokenCardOptions
        //        {
        //            Number = paymentDto.CardNumber,
        //            ExpMonth = paymentDto.ExpirationMonth,
        //            ExpYear = paymentDto.ExpirationYear,
        //            Cvc = paymentDto.Cvc,
                    
        //        }
        //    };

        //    var tokenService = new TokenService();
        //    Token stripeToken = tokenService.Create(tokenOptions);
        //    return stripeToken.Id;
        //}
}

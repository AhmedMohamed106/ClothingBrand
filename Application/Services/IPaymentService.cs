using ClothingBrand.Application.Common.DTO.Response.Payment;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
    public interface IPaymentService
    {
        public Task<PaymentResultDto> ProcessPayment(PaymentDto paymentDto);
    }

}

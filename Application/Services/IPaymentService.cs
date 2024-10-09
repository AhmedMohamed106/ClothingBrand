using ClothingBrand.Application.Common.DTO.Response.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
    public interface IPaymentService
    {
        PaymentDto ProcessPayment(PaymentDto paymentDto);
        PaymentDto GetPaymentById(int paymentId);
        IEnumerable<PaymentDto> GetPaymentsByUserId(string userId);
    }

}

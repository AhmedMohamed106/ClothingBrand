using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.DTO.Response.Payment
{
    public class PaymentDto
    {
        public string PaymentMethodId { get; set; }
        public long Amount { get; set; } // Amount in cents
        public string Currency { get; set; } = "usd"; // Currency like "usd"
    }


}

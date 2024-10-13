using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.DTO.Response.Payment
{
    public class PaymentResultDto
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public bool RequiresAction { get; set; } // Used for 3D Secure authentication
        public string PaymentIntentClientSecret { get; set; } // ClientSecret for further confirmation

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.DTO.Request.Account
{
    public class ResetPassword
    {
        public string userId { get; set; }
        public string Token {  get; set; }
        [DataType(DataType.Password)]
        public string password { get; set;}

    }
}

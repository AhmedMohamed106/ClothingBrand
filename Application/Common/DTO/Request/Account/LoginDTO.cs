using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Account
{
    public class LoginDTO
    {
        [EmailAddress,Required,DataType(DataType.EmailAddress)]
        [Display(Name ="Email Address")]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

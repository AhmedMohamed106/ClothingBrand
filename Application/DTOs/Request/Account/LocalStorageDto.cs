using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Account
{
    public class LocalStorageDto
    {
        public string? Token { get; set; }
        public string? Refresh { get; set; }
    }
}

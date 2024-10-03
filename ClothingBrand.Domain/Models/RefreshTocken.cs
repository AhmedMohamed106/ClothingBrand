using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Domain.Models
{
    public class RefreshTocken
    {
        public int Id { get; set; }
        public string? UserID { get; set; }
        public string? Token { get; set; }
    }
}

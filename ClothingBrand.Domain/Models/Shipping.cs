using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Domain.Models
{
    public class Shipping
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }

        // Navigation Properties
        public virtual Order? Order { get; set; } // One-to-one with Order
    }

}

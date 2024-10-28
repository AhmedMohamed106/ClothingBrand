using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }


        public List<RefreshTocken>? RefreshTokens { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<CustomClothingOrder> CustomClothingOrders { get; set; } = new List<CustomClothingOrder>();

        public virtual ICollection<Enrollment>? Enrollments { get; set; } // One-to-many with Enrollment
    }
}

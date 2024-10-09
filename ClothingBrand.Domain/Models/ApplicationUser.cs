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
        public virtual ICollection<Enrollment>? Enrollments { get; set; } // One-to-many with Enrollment
    }
}

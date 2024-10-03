using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Domain.Models
{
    public class SewingCourse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Duration { get; set; }

        // Navigation Properties
        public virtual ICollection<Enrollment> Enrollments { get; set; } // One-to-many with Enrollment
    }

}

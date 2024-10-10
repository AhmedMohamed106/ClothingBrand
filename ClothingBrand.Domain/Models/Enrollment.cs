using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Domain.Models
{
    public class Enrollment
    {
        //public int Id { get; set; }

        // public int UserId { get; set; }
        //[ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        //[ForeignKey("SewingCourse")]
        public int SewingCourseId { get; set; }
        public DateTime EnrollDate { get; set; }

        // Navigation Properties
      //  public User User { get; set; } // Many-to-one with User
        public virtual SewingCourse? SewingCourse { get; set; } // Many-to-one with SewingCourse
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }

}

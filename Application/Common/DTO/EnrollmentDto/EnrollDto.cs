using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.DTO.EnrollmentDto
{
    public class EnrollDto
    {
        public string? UserId {  get; set; }
        public int? CourseId { get; set; }
        public string? EnrollDate { get; set; }
        public ICollection<string>? Users { get; set; } = new List<string>();
        public ICollection<string>? Courses { get; set; } = new List<string>();




    }
}
/*
  public int Id { get; set; }

        // public int UserId { get; set; }
        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }

        [ForeignKey("SewingCourse")]
        public int SewingCourseId { get; set; }
        public DateTime EnrollDate { get; set; }

        // Navigation Properties
      //  public User User { get; set; } // Many-to-one with User
        public virtual SewingCourse SewingCourse { get; set; } // Many-to-one with SewingCourse
        public virtual ApplicationUser ApplicationUser { get; set; }
 */

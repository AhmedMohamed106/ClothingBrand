using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.DTO.EnrollmentDto
{
    public class CreateEnrollmentDto
    {
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollDate { get; set; }=DateTime.Now;
    }
}

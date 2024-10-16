using ClothingBrand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.Interfaces
{
    public interface IEnrollmentRepository: IRepository<Enrollment>
    {
        IEnumerable<ApplicationUser> GetUserEnrollInCourse(int CourseId);
        IEnumerable<SewingCourse> GetCoursesForUser(string UserId);
        
    }
}

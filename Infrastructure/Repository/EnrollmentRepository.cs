using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Domain.Models;
using ClothingBrand.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Infrastructure.Repository
{
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        private readonly ApplicationDbContext _db;

        public EnrollmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SewingCourse> GetCoursesForUser(string UserId)
        {
            return _db.Enrollments
           .Where(uc => uc.ApplicationUserId == UserId)
           .Select(uc => uc.SewingCourse)
           .ToList();
        }

        public IEnumerable<ApplicationUser> GetUserEnrollInCourse(int CourseId)
        {
            return _db.Enrollments
           .Where(uc => uc.SewingCourseId == CourseId)
           .Select(uc => uc.ApplicationUser)
           .ToList();
        }
    }
}

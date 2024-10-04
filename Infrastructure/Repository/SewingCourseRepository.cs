using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Domain.Models;
using ClothingBrand.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Infrastructure.Repository
{
    public class SewingCourseRepository : Repository<SewingCourse>, ISewingCourseRepository
    {
        private readonly ApplicationDbContext _db;

        public SewingCourseRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(SewingCourse sewingCourse)
        {
            _db.SewingCourses.Update(sewingCourse);
        }
    }
}

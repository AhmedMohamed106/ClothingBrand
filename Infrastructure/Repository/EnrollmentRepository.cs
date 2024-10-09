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
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        private readonly ApplicationDbContext _db;

        public EnrollmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

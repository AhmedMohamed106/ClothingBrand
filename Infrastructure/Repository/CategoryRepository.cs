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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Category obj)
        {

            //_db.products.Update(obj);
            var objFromDb = _db.Categories.FirstOrDefault(p => p.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
             

                objFromDb.Description = obj.Description;
              
            }
            _db.SaveChanges();

        }

    }
}

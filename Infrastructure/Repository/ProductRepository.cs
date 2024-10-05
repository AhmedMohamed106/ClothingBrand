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
    public class ProductRepository:Repository<Product>,IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) :base(db)
        {
            _db= db;
        }
        public void Update(Product obj)
        {

            //_db.products.Update(obj);
            var objFromDb = _db.Products.FirstOrDefault(p => p.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
                
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId; 
                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
            _db.SaveChanges();

        }

    }
}

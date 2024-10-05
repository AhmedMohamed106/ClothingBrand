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
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        private readonly ApplicationDbContext _db;

        public DiscountRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Discount obj)
        {

            //_db.products.Update(obj);
            var objFromDb = _db.Discounts.FirstOrDefault(p => p.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Code = obj.Code;
                objFromDb.StartDate = obj.StartDate;
                objFromDb.EndDate = obj.EndDate;

                objFromDb.Percentage = obj.Percentage;
                objFromDb.Id = obj.Id;
                if (obj.Products != null)
                {
                    objFromDb.Products = obj.Products;
                }
            }
            _db.SaveChanges();

        }

    }
}

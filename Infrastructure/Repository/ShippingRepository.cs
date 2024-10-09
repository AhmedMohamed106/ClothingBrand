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
    public class ShippingRepository : Repository<Shipping>, IShippingRepository
    {
        private readonly ApplicationDbContext _db;

        public ShippingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Shipping shipping)
        {
            _db.Update(shipping);
        }
    }
}

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
    public class CustomClothingOrderRepository : Repository<CustomClothingOrder>, ICustomClothingOrderRepository
    {

        private readonly ApplicationDbContext _db;

        public CustomClothingOrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(CustomClothingOrder customClothingOrder)
        {
            _db.customClothingOrders.Update(customClothingOrder);
        }
    }
}

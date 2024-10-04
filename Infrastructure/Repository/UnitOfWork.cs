using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _db;
        public IApplicationUserRepository applicationUserRepository { get; private set; }

        public ICategoryRepository categoryRepository { get; private set; }

        public ICustomClothingOrderRepository customClothingOrderRepository { get; private set; }

        public IDiscountRepository ciscountRepository { get; private set; }

        public IOrderItemRepository orderItemRepository { get; private set; }

        public IOrderRepository orderRepository { get; private set; }
        public IProductRepository productRepository { get; private set; }

        public ISewingCourseRepository sewingCourseRepository { get; private set; }

        public IShoppingCartItemRepository shoppingCartItemRepository { get; private set; }

        public IShoppingCartRepository shoppingCartRepository { get; private set; }


        public UnitOfWork(ApplicationDbContext _db)
        {
            this._db = _db;

            applicationUserRepository = 



        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}

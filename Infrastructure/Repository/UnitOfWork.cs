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

        public IDiscountRepository discountRepository { get; private set; }

        public IOrderItemRepository orderItemRepository { get; private set; }
        public IOrderRepository orderRepository { get; private set; }
        public IProductRepository productRepository { get; private set; }

        public ISewingCourseRepository sewingCourseRepository { get; private set; }

        public IShoppingCartItemRepository shoppingCartItemRepository { get; private set; }

        public IShoppingCartRepository shoppingCartRepository { get; private set; }

        public IShippingRepository shippingRepository { get; private set; }
        public IEnrollmentRepository enrollmentRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext _db)
        {
            this._db = _db;

            applicationUserRepository = new ApplicationUserRepository(_db);
            categoryRepository = new CategoryRepository(_db);
            customClothingOrderRepository = new CustomClothingOrderRepository(_db);
            discountRepository = new DiscountRepository(_db);
            orderItemRepository = new OrderItemRepository(_db);
            productRepository = new ProductRepository(_db);
            sewingCourseRepository = new SewingCourseRepository(_db);
            shoppingCartItemRepository = new ShoppingCartItemRepository(_db);
            shoppingCartRepository = new ShoppingCartRepository(_db);
            shippingRepository = new ShippingRepository(_db);
            orderRepository = new OrderRepository(_db);
            enrollmentRepository = new EnrollmentRepository(_db);   



        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {

        IApplicationUserRepository applicationUserRepository { get; }
        ICategoryRepository categoryRepository { get; }
        ICustomClothingOrderRepository customClothingOrderRepository { get; }
        IDiscountRepository discountRepository { get; }
        IOrderItemRepository orderItemRepository { get; }
        IOrderRepository orderRepository { get; }
        IProductRepository productRepository { get; }
        ISewingCourseRepository sewingCourseRepository { get; }
        IShoppingCartItemRepository shoppingCartItemRepository { get; }
        IShoppingCartRepository shoppingCartRepository { get; }
        IEnrollmentRepository enrollmentRepository { get; }
        IShippingRepository shippingRepository { get; }



        void Save();
    }
}

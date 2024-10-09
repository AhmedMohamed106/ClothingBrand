using ClothingBrand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order order);

        IEnumerable<Order> GetBy(
       Expression<Func<Order, bool>> filter = null,
       Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null,
       string includeProperties = "");
    }
}

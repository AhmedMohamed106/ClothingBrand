using ClothingBrand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.Interfaces
{
    public interface IProductRepository : IRepository<Product>
   
    {
        public void Update(Product obj);

        IQueryable<Product> GetAllWithPagaAsync(int page, int pageSize, Expression<Func<Product, bool>>? filter = null, string? includeProperties = null, bool tracked = false);

    }
}

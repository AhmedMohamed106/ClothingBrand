using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Domain.Models;
using ClothingBrand.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Infrastructure.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {

        private readonly ApplicationDbContext _db;
        internal DbSet<Order> _dbSet;
        public OrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
            _dbSet = _db.Set<Order>();
        }

        public IEnumerable<Order> GetBy(
        Expression<Func<Order, bool>> filter = null,
        Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null,
        string includeProperties = "")
            {
            IQueryable<Order> query = _dbSet; // Assuming _dbSet is the DbSet<Order>

            // Apply the filter, if any
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Include related entities
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            // Apply ordering if provided
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public void Update(Order order)
        {
            _db.Orders.Update(order);
        }
    }
}

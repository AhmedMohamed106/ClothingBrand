using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Domain.Models;
using ClothingBrand.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

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
                    var oldImageUrl = objFromDb.ImageUrl;
                    objFromDb.ImageUrl = obj.ImageUrl;

                    if (oldImageUrl != null)
                    {
                        //delete for old 
                        if (System.IO.File.Exists(("wwwroot/images/" + oldImageUrl)))
                        {

                            System.IO.File.Delete(("wwwroot/images/" + oldImageUrl));
                        }
                    }
                }
            }
           

        }


        public IQueryable<Product> GetAllWithPagaAsync(int page, int pageSize, Expression<Func<Product, bool>>? filter = null, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<Product> query;
            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }
            var skip = (page - 1) * pageSize;
            return  query.Skip(skip).Take(pageSize).AsQueryable();
            
        }


    }
}

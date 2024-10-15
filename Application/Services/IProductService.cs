using ClothingBrand.Application.Common.DTO.Request;
using ClothingBrand.Application.Common.DTO.Response;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
    public interface IProductService
    {
        public IEnumerable<GETProductDTO> GetAll(Expression<Func<Product, bool>>? filter = null, string? includeProperties = null, bool tracked = false);

        public GETProductDTO GEtProduct(int id);
        public  Task AddProduct(ProductDTO productDTO);
        public void Remove(int id);


        public Task update(int id, ProductDTO productDTO);
        public IEnumerable<GETProductDTO> GetAllWithpagination(int page, int pageSize, Expression<Func<Product, bool>>? filter = null, string? includeProperties = null, bool tracked = false);



    }
}

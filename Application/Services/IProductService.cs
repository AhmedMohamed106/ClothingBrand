using ClothingBrand.Application.Common.DTO.Request;
using ClothingBrand.Application.Common.DTO.Response;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
    public interface IProductService
    {
        public IEnumerable<GETProductDTO> GEtAll();

        public GETProductDTO GEtProduct(int id);
        public void AddProduct(ProductDTO productDTO);
        public void Remove(int id);


        public void update(int id, ProductDTO productDTO);
       

    }
}

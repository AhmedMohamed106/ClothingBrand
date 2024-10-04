using ClothingBrand.Application.Common.DTO.Request;
using ClothingBrand.Application.Common.DTO.Response;
using ClothingBrand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
    public interface IDiscountService
    {
        public IEnumerable<Discount> GEtAll();

        public Discount GEtDiscount(int id);
        public void AddDiscount(CreateDiscountDTO categoryDto);
        public void Remove(int id);


        public void update(int id, CreateDiscountDTO categoryDto);
    }
}

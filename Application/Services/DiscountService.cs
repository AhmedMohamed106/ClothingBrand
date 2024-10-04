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
    public class DiscountService:IDiscountService
    {
       
            private readonly IDiscountRepository _discountRepository;
            public DiscountService(IDiscountRepository discountRepository)
            {
                _discountRepository = discountRepository;
            }
            public IEnumerable<Discount> GEtAll()
            {
                var iList = _discountRepository.GetAll(includeProperties: "Products")
                    .Select(e => new Discount
                    {

                        Code = e.Code,
                        Percentage = e.Percentage,
                        Id = e.Id,
                        EndDate = e.EndDate,
                        Products = e.Products,
                        StartDate = e.StartDate


                    });
                return iList;
            }

            public Discount GEtDiscount(int id)
            {
                var category = _discountRepository.Get((x) => x.Id == id, includeProperties: "Products");

                var cage = new Discount
                {

                    Code = category.Code,
                    Percentage = category.Percentage,
                    Id = category.Id,
                    EndDate = category.EndDate,
                    Products = category.Products,
                    StartDate = category.StartDate



                };


                return cage;
            }


            public void AddDiscount(CreateDiscountDTO discountDto)
            {
                var catg = new Discount()
                {

                    Code = discountDto.Code,
                    Percentage = discountDto.Percentage,
                   
                    EndDate = discountDto.EndDate,
                   
                    StartDate = discountDto.StartDate


                };
                _discountRepository.Add(catg);
            }
            public void Remove(int id)
            {
                _discountRepository.Remove(_discountRepository.Get((x) => x.Id == id));

            }
            public void update(int id, CreateDiscountDTO discountDto)
            {
                var discount = new Discount()
                {

                    Code = discountDto.Code,
                    Percentage = discountDto.Percentage,

                    EndDate = discountDto.EndDate,

                    StartDate = discountDto.StartDate,
                    Id = id

                };
                _discountRepository.Update(discount);
            }
        }
    
}

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
       
            private readonly IUnitOfWork _unitRepository;
            public DiscountService(IUnitOfWork unitRepository)
            {
                _unitRepository = unitRepository;
            }
            public IEnumerable<Discount> GEtAll()
            {
                var iList = _unitRepository.discountRepository.GetAll(includeProperties: "Products")
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
                var category = _unitRepository.discountRepository.Get((x) => x.Id == id, includeProperties: "Products");

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
                _unitRepository.discountRepository.Add(catg);
            _unitRepository.Save();
            }
            public void Remove(int id)
            {
                _unitRepository.discountRepository.Remove(_unitRepository.discountRepository.Get((x) => x.Id == id));
            _unitRepository.Save();


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
                _unitRepository.discountRepository.Update(discount);
            _unitRepository.Save();

        }
    }
    
}

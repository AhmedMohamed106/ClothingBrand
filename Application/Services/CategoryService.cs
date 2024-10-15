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
    public class CategoryService : IcategoryService
    {
        private readonly IUnitOfWork _unitRepository;
        public CategoryService(IUnitOfWork unitRepository)
        {
           _unitRepository = unitRepository;
        }
        public IEnumerable<GEtCategoryDTO> GEtAll()
        {
            var iList =_unitRepository.categoryRepository.GetAll(includeProperties: "Products")
                .Select(e => new GEtCategoryDTO
                {
                   
                    Name = e.Name,
                    Description = e.Description,
                   Id = e.Id
                   

                });
            return iList;
        }

        public GEtCategoryDTO GEtCategory(int id)
        {
            var category =_unitRepository.categoryRepository.Get((x) => x.Id == id, includeProperties: "Products");

            var cage= new GEtCategoryDTO
            {
                
                Name = category.Name,
                Description = category.Description,
               
                Id = category.Id,


            };
           

            return cage;
        }


        public void AddCategory(CreateCategoryDto categoryDto)
        {
            var catg = new Category()
            {
              
                Name = categoryDto.Name,
                Description = categoryDto.Description,
              

            };
           _unitRepository.categoryRepository.Add(catg);
            _unitRepository.Save();
        }
        public void Remove(int id)
        {
           _unitRepository.categoryRepository.Remove(_unitRepository.categoryRepository.Get((x) => x.Id == id));
            _unitRepository.Save();

        }
        public void update(int id, CreateCategoryDto categoryDto)
        {
            var category = new Category()
            {
              
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                Id = id

            };
           _unitRepository.categoryRepository.Update(category);
            _unitRepository.Save();
        }
    }
}

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
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public IEnumerable<GEtCategoryDTO> GEtAll()
        {
            var iList = _categoryRepository.GetAll(includeProperties: "Products")
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
            var category = _categoryRepository.Get((x) => x.Id == id, includeProperties: "Products");

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
            _categoryRepository.Add(catg);
        }
        public void Remove(int id)
        {
            _categoryRepository.Remove(_categoryRepository.Get((x) => x.Id == id));

        }
        public void update(int id, CreateCategoryDto categoryDto)
        {
            var category = new Category()
            {
              
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                Id = id

            };
            _categoryRepository.Update(category);
        }
    }
}

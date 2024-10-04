using ClothingBrand.Application.Common.DTO.Request;
using ClothingBrand.Application.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
    public interface IcategoryService
    {
        public IEnumerable<GEtCategoryDTO> GEtAll();

        public GEtCategoryDTO GEtCategory(int id);
        public void AddCategory(CreateCategoryDto categoryDto);
        public void Remove(int id);


        public void update(int id, CreateCategoryDto categoryDto);
    }
}

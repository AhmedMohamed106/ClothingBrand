using ClothingBrand.Application.Common.DTO.Request;
using ClothingBrand.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothingBrand.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IcategoryService _CategoryService;
        public CategoryController(IcategoryService categoryService)
        {
            _CategoryService = categoryService;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _CategoryService.GEtAll();
            return Ok(products);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IActionResult Create(CreateCategoryDto categoryDto)
        {
            if (categoryDto == null) { return BadRequest(); }

            _CategoryService.AddCategory(categoryDto);

            return Ok();
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, CreateCategoryDto categoryDto)
        {
            if (categoryDto == null) { return BadRequest(); }

            _CategoryService.update(id, categoryDto);

            return Ok();
        }
        [HttpGet("{id}")]
        public IActionResult GetAll(int id)
        {
            var products = _CategoryService.GEtCategory(id);
            return Ok(products);
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]

        public IActionResult Remove(int id)
        {
            _CategoryService.Remove(id);
            return Ok();
        }
    }
}

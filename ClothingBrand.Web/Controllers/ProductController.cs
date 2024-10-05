using ClothingBrand.Application.Common.DTO.Request;
using ClothingBrand.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothingBrand.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public IActionResult GetAll() {
         var products=   _productService.GEtAll();
          return Ok(products);
        }
        [HttpPost]
        public IActionResult Create(ProductDTO productDTO)
        {
            if (productDTO == null) { return BadRequest(); }

            _productService.AddProduct(productDTO);

            return Ok();
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id,ProductDTO productDTO)
        {
            if (productDTO == null) { return BadRequest(); }

            _productService.update(id,productDTO);

            return Ok();
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var products = _productService.GEtProduct(id);
            return Ok(products);
        }

        
    }
}

using ClothingBrand.Application.Common.DTO.Request;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClothingBrand.Application.Contract;

namespace ClothingBrand.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailService _mailingService;

        public ProductController(IProductService productService, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IEmailService mailingService)
        {
            _productService = productService;
            this._webHostEnvironment = webHostEnvironment;
            _mailingService = mailingService;
        }
        [HttpGet]
        public IActionResult GetAll() {
            var products = _productService.GetAll();
          return Ok(products);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductDTO productDTO)
        {
            if (productDTO == null) { return BadRequest(); }

             await _productService.AddProduct(productDTO);

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

        [HttpGet("Filtering")]
        public IActionResult Filtering(string CategoryName = null, decimal Maxprice = 0, decimal MinPrice = 0, string KeyWord = null)
        {
            if (ModelState.IsValid)
            {
                var products = _productService.GetAll();

                if (CategoryName != null)
                {
                    products = products.Where(p => p.CategoryName == CategoryName);
                }

                if (Maxprice > 0)
                {
                    products = products.Where(p => p.Price < Maxprice);
                }

                 if (MinPrice > 0)
                {
                    products = products.Where(p => p.Price >MinPrice);
                   
                }
                if (KeyWord != null)
                {
                    products = products.Where(p => p.Price > MinPrice);

                }





                //if (CategoryName != null && MinPrice > 0 && Maxprice > 0 && KeyWord != null)
                //{
                //    products = _productService.productRepository.GetAll(a => (a.Category.Name.Contains(CategoryName) && a.Name.Contains(KeyWord)));
                //}
                //else if (MinPrice > 0 && Maxprice > 0 && KeyWord != null)
                //{

                //}
                //else if (CategoryName != null && MinPrice > 0 && Maxprice > 0 )
                //{

                //}
                //else if (CategoryName != null)
                //{
                //     products = _productService.productRepository.GetAll(a => a.Category.Name.Contains(CategoryName));
                //}
                //else if (MinPrice > 0 && Maxprice > 0)
                //{
                //     products = _productService.productRepository.GetAll(a => (a.Price > MinPrice && a.Price < Maxprice));
                //}
                //else if (Maxprice > 0)
                //{
                //     products = _productService.productRepository.GetAll(a => a.Price < Maxprice);
                //}

                //else if (MinPrice > 0)
                //{
                //     products = _productService.productRepository.GetAll(a => a.Price > MinPrice);
                //}

                //else if (KeyWord != null)
                //{
                //     products = _productService.productRepository.GetAll(a => a.Name.Contains(KeyWord));

                //}
                

                if (products.Any())
                {
                   
                    return Ok(products);

                }
                return NoContent();
            }

            return BadRequest();

        }
        //[HttpPost("send")]
        //public async Task<IActionResult> SendMail([FromForm] MailRequestDto dto)
        //{
        //    await _mailingService.SendEmailAsync(dto.ToEmail, dto.Subject, dto.Body);
        //    return Ok();
        //}

    }
}

using ClothingBrand.Application.Common.DTO.Request;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClothingBrand.Application.Contract;
using Microsoft.AspNetCore.Authorization;

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
        //[Authorize]
        public IActionResult GetAll() {
            var products = _productService.GetAll();
          return Ok(products);
        }
        [HttpPost]
       // [Authorize(Roles = "Admin")] // Only Admins can create products
        public async Task<IActionResult> Create([FromForm] ProductDTO productDTO)
        {
            if (productDTO == null) { return BadRequest(); }

             await _productService.AddProduct(productDTO);

            return Ok();
        }
        [HttpPut("{id}")]
       [Authorize(Roles = "Admin")] // Only Admins can update products
        public IActionResult Update(int id,[FromForm]ProductDTO productDTO)
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

        [HttpDelete]
        [Authorize(Roles = "Admin")] // Only Admins can delete products
        public IActionResult Remove(int id)
        {
            _productService.Remove(id);
            return Ok();
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
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequestDto dto)
        {
            //var filePath = $"{Directory.GetCurrentDirectory()}\\Templates\\EmailTemplate.html";
            //var str = new StreamReader(filePath);

            //var mailText = str.ReadToEnd();
            //str.Close();
            //mailText = mailText.Replace("[username]", dto.Subject).Replace("[email]", dto.ToEmail);
            await _mailingService.SendEmailAsync(dto.ToEmail, dto.Subject, dto.Body,dto.Attachments);
            return Ok();
        }


        [HttpGet("pagination")]
        public IActionResult pagination(int page,int pageSize,string CategoryName = null, decimal Maxprice = 0, decimal MinPrice = 0, string KeyWord = null)
        {
            if (ModelState.IsValid)
            {
                var products = _productService.GetAllWithpagination(page, pageSize);

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
                    products = products.Where(p => p.Price > MinPrice);

                }
                if (KeyWord != null)
                {
                    products = products.Where(p => p.Price > MinPrice);

                }



                if (products.Any())
                {

                    return Ok(products);

                }
                return NoContent();
            }

            return BadRequest();

        }

    }
}

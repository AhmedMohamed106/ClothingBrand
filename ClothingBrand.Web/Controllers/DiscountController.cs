using ClothingBrand.Application.Common.DTO.Request;
using ClothingBrand.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothingBrand.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
            private readonly IDiscountService _discountService;
            public DiscountController(IDiscountService discountService)
            {
                _discountService = discountService;
            }
            [HttpGet]
       
            public IActionResult GetAll()
            {
                var discounts = _discountService.GEtAll();
                return Ok(discounts);
            }

            [HttpPost]
           // [Authorize(Roles = "Admin")]
        public IActionResult Create(CreateDiscountDTO discountDTO)
            {
                if (discountDTO == null) { return BadRequest(); }

                _discountService.AddDiscount(discountDTO);

                return Ok();
            }

            [HttpPut("{id}")]
         //   [Authorize(Roles = "Admin")]

        public IActionResult Update(int id, CreateDiscountDTO discountDTO)
            {
                if (discountDTO == null) { return BadRequest(); }

                _discountService.update(id, discountDTO);

                return Ok();
            }
            [HttpGet("{id}")]
            public IActionResult Get(int id)
            {
                var discount = _discountService.GEtDiscount(id);
                return Ok(discount);
            }
        [HttpDelete]
      //  [Authorize(Roles = "Admin")]
        public IActionResult Remove(int id)
        {
            _discountService.Remove(id);
            return Ok();
        }
    }
}

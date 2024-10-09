using ClothingBrand.Application.Common.DTO.Response.Shipping;
using ClothingBrand.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothingBrand.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _shippingService;

        public ShippingController(IShippingService shippingService)
        {
            _shippingService = shippingService;
        }

        // POST: api/shipping
        [HttpPost]
        public IActionResult CreateShipping([FromBody] ShippingDto shippingDto)
        {
            if (shippingDto == null)
            {
                return BadRequest("Invalid shipping data.");
            }

            try
            {
                var createdShipping = _shippingService.CreateShipping(shippingDto);
                return CreatedAtAction(nameof(GetShippingById), new { shippingId = createdShipping.Id }, createdShipping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/shipping/{shippingId}
        [HttpGet("{shippingId}")]
        public IActionResult GetShippingById(int shippingId)
        {
            try
            {
                var shipping = _shippingService.GetShippingById(shippingId);
                if (shipping == null)
                {
                    return NotFound("Shipping record not found.");
                }

                return Ok(shipping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/shipping
        [HttpGet]
        public IActionResult GetAllShipping()
        {
            try
            {
                var shippings = _shippingService.GetAllShipping();
                return Ok(shippings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/shipping/{shippingId}
        [HttpPut("{shippingId}")]
        public IActionResult UpdateShipping(int shippingId, [FromBody] ShippingDto shippingDto)
        {
            if (shippingDto == null || shippingDto.Id != shippingId)
            {
                return BadRequest("Invalid shipping data or mismatched ID.");
            }

            try
            {
                var existingShipping = _shippingService.GetShippingById(shippingId);
                if (existingShipping == null)
                {
                    return NotFound("Shipping record not found.");
                }

                var updatedShipping = _shippingService.UpdateShipping(shippingId, shippingDto);
                return Ok(updatedShipping);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/shipping/{shippingId}
        [HttpDelete("{shippingId}")]
        public IActionResult DeleteShipping(int shippingId)
        {
            try
            {
                var shipping = _shippingService.GetShippingById(shippingId);
                if (shipping == null)
                {
                    return NotFound("Shipping record not found.");
                }

                _shippingService.DeleteShipping(shippingId);
                return NoContent(); // Successfully deleted
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

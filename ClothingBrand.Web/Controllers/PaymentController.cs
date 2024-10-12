using ClothingBrand.Application.Common.DTO.Response.Payment;
using ClothingBrand.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothingBrand.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("process")]
        public ActionResult<PaymentResultDto> ProcessPayment([FromBody] PaymentDto paymentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _paymentService.ProcessPayment(paymentDto);
            if (result.IsCompletedSuccessfully)
                return Ok(result);

            return BadRequest(result);
        }
    }
}

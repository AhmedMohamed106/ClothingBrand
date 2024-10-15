using ClothingBrand.Application.Common.DTO.Request.CustomOrderClothes;
using ClothingBrand.Application.Common.DTO.Response.CustomOrderClothes;
using ClothingBrand.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class CustomClothingOrderController : ControllerBase
{
    private readonly ICustomClothingOrderService _customClothingOrderService;

    public CustomClothingOrderController(ICustomClothingOrderService customClothingOrderService)
    {
        _customClothingOrderService = customClothingOrderService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CustomClothingOrderDto>> GetAllClothingOrders()
    {
        var orders = _customClothingOrderService.GetAllCustomClothingOrders();
        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public ActionResult<CustomClothingOrderDto> GetClothingOrderById(int id)
    {
        var order = _customClothingOrderService.GetCustomClothingOrderById(id);
        if (order == null)
        {
            return NotFound($"Custom clothing order with ID {id} not found.");
        }

        return Ok(order);
    }

    [HttpPost]
    public ActionResult<CustomClothingOrderDto> CreateClothingOrder([FromForm] CreateCustomClothingOrderDto orderDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdOrder = _customClothingOrderService.CreateCustomClothingOrder(orderDto);
            return CreatedAtAction(nameof(GetClothingOrderById), new { id = createdOrder.Id }, createdOrder);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the order.");
        }
    }

    [HttpPut("{id:int}")]
    public ActionResult<CustomClothingOrderDto> UpdateClothingOrder(int id, [FromForm] UpdateCustomClothingOrderDto orderDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updatedOrder = _customClothingOrderService.UpdateCustomClothingOrder(id, orderDto);
            if (updatedOrder == null)
            {
                return NotFound($"Custom clothing order with ID {id} not found.");
            }

            return Ok(updatedOrder);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the order.");
        }
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteClothingOrder(int id)
    {
        try
        {
            _customClothingOrderService.DeleteCustomClothingOrder(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Custom clothing order with ID {id} not found.");
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the order.");
        }
    }

    [HttpPatch("{id:int}/status")]
    public ActionResult<CustomClothingOrderDto> UpdateClothingOrderStatus(int id, [FromBody] string newStatus)
    {
        if (string.IsNullOrWhiteSpace(newStatus))
        {
            return BadRequest("New status cannot be null or empty.");
        }

        try
        {
            var updatedOrder = _customClothingOrderService.UpdateCustomOrderStatus(id, newStatus);
            return Ok(updatedOrder);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Custom clothing order with ID {id} not found.");
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the order status.");
        }
    }
}

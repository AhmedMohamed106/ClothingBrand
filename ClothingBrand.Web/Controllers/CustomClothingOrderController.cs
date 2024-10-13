using ClothingBrand.Application.Common.DTO.Request.CustomOrderClothes;
using ClothingBrand.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class CustomClothingOrderController : ControllerBase
{
    private readonly ICustomClothingOrderService _customClothingOrderService;

    public CustomClothingOrderController(ICustomClothingOrderService customClothingOrderService)
    {
        _customClothingOrderService = customClothingOrderService;
    }

    // POST: api/CustomClothingOrder
    [HttpPost]
    public IActionResult CreateCustomClothingOrder([FromBody] CreateCustomClothingOrderDto orderDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdOrder = _customClothingOrderService.CreateCustomClothingOrder(orderDto);
        return CreatedAtAction(nameof(GetCustomClothingOrderById), new { id = createdOrder.Id }, createdOrder);
    }

    // PUT: api/CustomClothingOrder/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateCustomClothingOrder(int id, [FromBody] UpdateCustomClothingOrderDto orderDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updatedOrder = _customClothingOrderService.UpdateCustomClothingOrder(id, orderDto);
            return Ok(updatedOrder);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // PATCH: api/CustomClothingOrder/{id}/status
    [HttpPatch("{id}/status")]
    public IActionResult UpdateCustomOrderStatus(int id, [FromBody] string newStatus)
    {
        try
        {
            var updatedOrder = _customClothingOrderService.UpdateCustomOrderStatus(id, newStatus);
            return Ok(updatedOrder);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // DELETE: api/CustomClothingOrder/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteCustomClothingOrder(int id)
    {
        try
        {
            _customClothingOrderService.DeleteCustomClothingOrder(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // GET: api/CustomClothingOrder/{id}
    [HttpGet("{id}")]
    public IActionResult GetCustomClothingOrderById(int id)
    {
        try
        {
            var order = _customClothingOrderService.GetCustomClothingOrderById(id);
            return Ok(order);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // GET: api/CustomClothingOrder
    [HttpGet]
    public IActionResult GetAllCustomClothingOrders()
    {
        var orders = _customClothingOrderService.GetAllCustomClothingOrders();
        return Ok(orders);
    }
}

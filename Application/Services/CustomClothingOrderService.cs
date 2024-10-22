using ClothingBrand.Application.Behaviours;
using ClothingBrand.Application.Common.DTO.Request.CustomOrderClothes;
using ClothingBrand.Application.Common.DTO.Response.CustomOrderClothes;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Application.Services;
using ClothingBrand.Domain.Models;
using Microsoft.AspNetCore.Http;
using Stripe.Climate;

public class CustomClothingOrderService : ICustomClothingOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;
    private readonly IAppConfiguration _appConfiguration;  // Add this line

    public CustomClothingOrderService(IUnitOfWork unitOfWork, IFileService fileService, IAppConfiguration appConfiguration)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _appConfiguration = appConfiguration;
    }


    private void ReplaceImage(CustomClothingOrder order, IFormFile newImage)
    {
        _fileService.DeleteImage(order?.ImageUrl, "CustomOrderImages");
        order.ImageUrl = _fileService.SaveImage(newImage, "CustomOrderImages");
    }
    public CustomClothingOrderDto CreateCustomClothingOrder(CreateCustomClothingOrderDto orderDto)
    {
        var customOrder = orderDto.MapToEntity();
        customOrder.ImageUrl = _fileService.SaveImage(orderDto.Image, "CustomOrderImages");

        _unitOfWork.customClothingOrderRepository.Add(customOrder);
        _unitOfWork.Save();

        return customOrder.MapToDto(_appConfiguration.ApplicationUrl);
    }

    public CustomClothingOrderDto UpdateCustomClothingOrder(int id, UpdateCustomClothingOrderDto orderDto)
    {
        var existingOrder = GetExistingOrder(id);

        // Use a mapping method to update the properties from orderDto
        existingOrder?.UpdateFromDto(orderDto);

        if (orderDto.Image != null)
        {
            ReplaceImage(existingOrder, orderDto.Image);
        }

        _unitOfWork.Save();

        return existingOrder?.MapToDto(_appConfiguration.ApplicationUrl);
    }

  

    public void DeleteCustomClothingOrder(int id)
    {
        var existingOrder = GetExistingOrder(id);
        _fileService.DeleteImage(existingOrder?.ImageUrl, "CustomOrderImages");
        _unitOfWork.customClothingOrderRepository.Remove(existingOrder);
        _unitOfWork.Save();
    }

    public CustomClothingOrderDto GetCustomClothingOrderById(int id)
    {
        var order = GetExistingOrder(id);
        return order.MapToDto(_appConfiguration.ApplicationUrl);
    }

    public IEnumerable<CustomClothingOrderDto> GetAllCustomClothingOrders()
    {
        var orders = _unitOfWork.customClothingOrderRepository.GetAll();
        return orders.Select(order=>order.MapToDto(_appConfiguration.ApplicationUrl)).ToList();
    }

    private CustomClothingOrder GetExistingOrder(int id)
    {
        var existingOrder = _unitOfWork.customClothingOrderRepository.Get(p=>p.Id == id);
        if (existingOrder == null)
        {
            throw new KeyNotFoundException($"Custom clothing order with ID {id} not found.");
        }
        return existingOrder;
    }
    public CustomClothingOrderDto UpdateCustomOrderStatus(int id, string newStatus)
    {
        var existingOrder = GetExistingOrder(id);
        existingOrder.CustomOrderStatus = newStatus;

        _unitOfWork.Save();
        return existingOrder.MapToDto(_appConfiguration.ApplicationUrl);
    }

    public IEnumerable<CustomClothingOrderDto> GetUserOrders(string userId)
    {
        // Get all orders that belong to the specified user
        var userOrders = _unitOfWork.customClothingOrderRepository.GetAll(order => order.UserId == userId);
        if (!userOrders.Any())
        {
            throw new KeyNotFoundException($"No custom clothing orders found for user with ID {userId}.");
        }

        // Map the orders to DTOs and return them
        return userOrders.Select(order => order.MapToDto(_appConfiguration.ApplicationUrl)).ToList();
    }
}

















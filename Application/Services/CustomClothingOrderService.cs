using ClothingBrand.Application.Common.DTO.Request.CustomOrderClothes;
using ClothingBrand.Application.Common.DTO.Response.CustomOrderClothes;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Application.Services;
using ClothingBrand.Domain.Models;

public class CustomClothingOrderService : ICustomClothingOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomClothingOrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public CustomClothingOrderDto CreateCustomClothingOrder(CreateCustomClothingOrderDto orderDto)
    {
        var customOrder = new CustomClothingOrder
        {
            DesignDescription = orderDto.DesignDescription,
            FabricDetails = orderDto.FabricDetails,
            DepositAmount = orderDto.DepositAmount,
            CustomOrderStatus = "Pending", // Default status
            ShoulderWidth = orderDto.ShoulderWidth,
            ChestCircumference = orderDto.ChestCircumference,
            WaistCircumference = orderDto.WaistCircumference,
            HipCircumference = orderDto.HipCircumference,
            WaistLength = orderDto.WaistLength,
            ArmLength = orderDto.ArmLength,
            BicepSize = orderDto.BicepSize,
            ModelLength = orderDto.ModelLength,
            UserId = orderDto.UserId
        };

        _unitOfWork.customClothingOrderRepository.Add(customOrder);
        _unitOfWork.Save();

        return MapToDto(customOrder);
    }

    public CustomClothingOrderDto UpdateCustomClothingOrder(int id, UpdateCustomClothingOrderDto orderDto)
    {
        var existingOrder = GetExistingOrder(id);

        // Use a mapping method to update the properties from orderDto
        UpdateProperties(existingOrder, orderDto);

        _unitOfWork.Save();

        return MapToDto(existingOrder);
    }

    public CustomClothingOrderDto UpdateCustomOrderStatus(int id, string newStatus)
    {
        var existingOrder = GetExistingOrder(id);
        existingOrder.CustomOrderStatus = newStatus;

        _unitOfWork.Save();
        return MapToDto(existingOrder);
    }

    public void DeleteCustomClothingOrder(int id)
    {
        var existingOrder = GetExistingOrder(id);
        _unitOfWork.customClothingOrderRepository.Remove(existingOrder);
        _unitOfWork.Save();
    }

    public CustomClothingOrderDto GetCustomClothingOrderById(int id)
    {
        var order = GetExistingOrder(id);
        return MapToDto(order);
    }

    public IEnumerable<CustomClothingOrderDto> GetAllCustomClothingOrders()
    {
        var orders = _unitOfWork.customClothingOrderRepository.GetAll();
        return orders.Select(MapToDto).ToList();
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

    private void UpdateProperties(CustomClothingOrder existingOrder, UpdateCustomClothingOrderDto orderDto)
    {
        // Use reflection to update properties if they are not null
        foreach (var property in typeof(UpdateCustomClothingOrderDto).GetProperties())
        {
            var newValue = property.GetValue(orderDto);
            if (newValue != null)
            {
                var existingProperty = typeof(CustomClothingOrder).GetProperty(property.Name);
                if (existingProperty != null)
                {
                    existingProperty.SetValue(existingOrder, newValue);
                }
            }
        }
    }

    private CustomClothingOrderDto MapToDto(CustomClothingOrder order)
    {
        return new CustomClothingOrderDto
        {
            Id = order.Id,
            DesignDescription = order.DesignDescription,
            FabricDetails = order.FabricDetails,
            DepositAmount = order.DepositAmount,
            CustomOrderStatus = order.CustomOrderStatus,
            ShoulderWidth = order.ShoulderWidth,
            ChestCircumference = order.ChestCircumference,
            WaistCircumference = order.WaistCircumference,
            HipCircumference = order.HipCircumference,
            WaistLength = order.WaistLength,
            ArmLength = order.ArmLength,
            BicepSize = order.BicepSize,
            ModelLength = order.ModelLength,
            UserId = order.UserId
        };
    }
}

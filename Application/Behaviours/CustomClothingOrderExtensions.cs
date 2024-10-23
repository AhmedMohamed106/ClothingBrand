using ClothingBrand.Application.Common.DTO.Request.CustomOrderClothes;
using ClothingBrand.Application.Common.DTO.Response.CustomOrderClothes;
using ClothingBrand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Behaviours
{
    public static class CustomClothingOrderExtensions
    {
        public static CustomClothingOrder MapToEntity(this CreateCustomClothingOrderDto dto)
        {
            return new CustomClothingOrder
            {
                DesignDescription = dto.DesignDescription,
                FabricDetails = dto.FabricDetails,
                customerName = dto.customerName,
                phoneNumber = dto.phoneNumber,
                DepositAmount = dto.DepositAmount,
                ShoulderWidth = dto.ShoulderWidth,
                ChestCircumference = dto.ChestCircumference,
                WaistCircumference = dto.WaistCircumference,
                HipCircumference = dto.HipCircumference,
                WaistLength = dto.WaistLength,
                ArmLength = dto.ArmLength,
                BicepSize = dto.BicepSize,
                ModelLength = dto.ModelLength,
                CustomOrderStatus = "Pending",  // Default status
                UserId = dto.UserId
            };
        }

        public static void UpdateFromDto(this CustomClothingOrder order, UpdateCustomClothingOrderDto dto)
        {
            order.DesignDescription = dto.DesignDescription ?? order.DesignDescription;
            order.FabricDetails = dto.FabricDetails ?? order.FabricDetails;
            order.DepositAmount = dto.DepositAmount ?? order.DepositAmount;
            order.customerName = dto.customerName ?? order.customerName;
            order.phoneNumber = dto.phoneNumber ?? order.phoneNumber;
            order.ShoulderWidth = dto.ShoulderWidth ?? order.ShoulderWidth;
            order.ChestCircumference = dto.ChestCircumference ?? order.ChestCircumference;
            order.WaistCircumference = dto.WaistCircumference ?? order.WaistCircumference;
            order.HipCircumference = dto.HipCircumference ?? order.HipCircumference;
            order.WaistLength = dto.WaistLength ?? order.WaistLength;
            order.ArmLength = dto.ArmLength ?? order.ArmLength;
            order.BicepSize = dto.BicepSize ?? order.BicepSize;
            order.ModelLength = dto.ModelLength ?? order.ModelLength;
        }

        public static CustomClothingOrderDto MapToDto(this CustomClothingOrder order , string applicationUrl)
        {
            return new CustomClothingOrderDto
            {
                Id = order.Id,
                DesignDescription = order.DesignDescription,
                FabricDetails = order.FabricDetails,
                customerName = order.customerName,
                phoneNumber = order.phoneNumber,
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
                ImageUrl = string.IsNullOrWhiteSpace(order.ImageUrl)
            ? null
            : $"{applicationUrl}/{order.ImageUrl.TrimStart('/')}",  // Full image URL
                UserId = order.UserId
            };
        }
    }

}

using ClothingBrand.Application.Common.DTO.Request.CustomOrderClothes;
using ClothingBrand.Application.Common.DTO.Response.CustomOrderClothes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
    public interface ICustomClothingOrderService
    {
        CustomClothingOrderDto CreateCustomClothingOrder(CreateCustomClothingOrderDto orderDto);
        CustomClothingOrderDto UpdateCustomClothingOrder(int id, UpdateCustomClothingOrderDto orderDto);
        CustomClothingOrderDto UpdateCustomOrderStatus(int id, string newStatus);
        void DeleteCustomClothingOrder(int id);
        CustomClothingOrderDto GetCustomClothingOrderById(int id);
        IEnumerable<CustomClothingOrderDto> GetAllCustomClothingOrders();
        IEnumerable<CustomClothingOrderDto> GetUserOrders(string userId);
    }

}

using ClothingBrand.Application.Common.DTO.Response.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
    public interface IShippingService
    {
        ShippingDto CreateShipping(ShippingDto shippingDto);
        ShippingDto GetShippingById(int shippingId);
        IEnumerable<ShippingDto> GetAllShipping();
        ShippingDto UpdateShipping(int shippingId, ShippingDto shippingDto);
        void DeleteShipping(int shippingId);

    }

}

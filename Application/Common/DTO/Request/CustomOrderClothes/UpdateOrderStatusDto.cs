using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.DTO.Request.CustomOrderClothes
{
    public class UpdateOrderStatusDto
    {
        [Required]
        public string NewStatus { get; set; }
    }

}

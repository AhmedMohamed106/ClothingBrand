using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.DTO.OrderDto
{
    public class UpdateOrderStatusDto
    {
        [Required]
        [StringLength(50)]
        public string OrderStatus { get; set; }
    }

}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.DTO.OrderDto
{
    public class CreateOrderDto
    {
        public string UserId { get; set; }
        public int ShippingId { get; set; }
        public int ShoppingCartId { get; set; }
    }

}
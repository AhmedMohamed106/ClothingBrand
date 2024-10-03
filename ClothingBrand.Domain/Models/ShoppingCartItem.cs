﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Domain.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public int ShoppingCartId { get; set; }
        public virtual ShoppingCart? ShoppingCart { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}

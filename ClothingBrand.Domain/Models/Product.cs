using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int StockQuantity { get; set; }

        // Category relation
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Discount relation

        [ForeignKey("Discount")]
        public int? DiscountId { get; set; }
        public virtual Discount Discount { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } // One-to-many with OrderItem

       // public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } // One-to-many with ShoppingCartItem


    }

}

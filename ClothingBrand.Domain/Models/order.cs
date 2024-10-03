using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Domain.Models
{
 
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentStatus { get; set; } // "Paid", "Unpaid"
        public string OrderStatus { get; set; } // "Pending", "Confirmed", "Shipped"

        [ForeignKey("Shipping")]
        public int ShippingId { get; set; }

        // Foreign Key
        public int UserId { get; set; }
        // public User User { get; set; }
        public virtual Shipping? Shipping { get; set; } // One-to-one with Shipping

        // Navigation property
        public virtual ICollection<OrderItem>? OrderItems { get; set; }

        [ForeignKey("ShoppingCart")]
        public int ShoppingCartId { get; set; }  // Linking to the shopping cart that was checked out
        public virtual ShoppingCart? ShoppingCart { get; set; }

    }

}

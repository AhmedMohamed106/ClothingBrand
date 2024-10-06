using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Domain.Models
{
    public class OrderItem
    {

        [Key]
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Foreign Key
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }   // An order item belongs to one order

        // Foreign Key
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }    // An order item is associated with one product
    }

}

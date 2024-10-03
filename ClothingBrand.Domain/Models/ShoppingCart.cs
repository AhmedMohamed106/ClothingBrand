using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Domain.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual ICollection<ShoppingCartItem> Items { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}

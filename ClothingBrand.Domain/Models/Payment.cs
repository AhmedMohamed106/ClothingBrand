using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Domain.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentMethod { get; set; } // "Online", "Offline"
        public string PaymentStatus { get; set; } // "Paid", "Unpaid"

        // Foreign Key (optional for different types of payments)
        public int? OrderId { get; set; }
        public Order Order { get; set; }

        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

}

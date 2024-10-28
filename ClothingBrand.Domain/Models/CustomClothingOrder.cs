using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Domain.Models
{
    public class CustomClothingOrder
    {
        public int Id { get; set; }
        public string DesignDescription { get; set; }
        public string FabricDetails { get; set; }
        public decimal DepositAmount { get; set; }
        public string CustomOrderStatus { get; set; }
        public string customerName { get; set; }
        public string phoneNumber { get; set; }
        public DateTime customOrderDate => DateTime.UtcNow;
        // Measurement properties
        public double ShoulderWidth { get; set; }
        public double ChestCircumference { get; set; }
        public double WaistCircumference { get; set; }
        public double HipCircumference { get; set; }
        public double WaistLength { get; set; }
        public double ArmLength { get; set; }
        public double BicepSize { get; set; }
        public double ModelLength { get; set; }

        // New Image property
        public string ImageUrl { get; set; }

        // Navigation Properties
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; } // Many-to-one with User
    }


}
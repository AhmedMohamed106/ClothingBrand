using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.DTO.Request.CustomOrderClothes
{
    public class CreateCustomClothingOrderDto
    {
        [Required]
        public string DesignDescription { get; set; }

        [Required]
        public string FabricDetails { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Deposit amount must be greater than zero.")]
        public decimal DepositAmount { get; set; }

        // Measurement properties
        [Required]
        public double ShoulderWidth { get; set; }

        [Required]

        public string customerName { get; set; }
        [Required]
        public string phoneNumber { get; set; }

        [Required]
        public double ChestCircumference { get; set; }

        [Required]
        public double WaistCircumference { get; set; }

        [Required]
        public double HipCircumference { get; set; }

        [Required]
        public double WaistLength { get; set; }

        [Required]
        public double ArmLength { get; set; }

        [Required]
        public double BicepSize { get; set; }

        [Required]
        public double ModelLength { get; set; }

        public IFormFile Image { get; set; }


        // UserId can be included if needed in order creation
        public string UserId { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.DTO.Request.CustomOrderClothes
{
   public class UpdateCustomClothingOrderDto
{
    public string DesignDescription { get; set; }
    public string FabricDetails { get; set; }
    public decimal? DepositAmount { get; set; } // Nullable for partial updates

    // Measurement properties
    public double? ShoulderWidth { get; set; }
    public double? ChestCircumference { get; set; }
    public double? WaistCircumference { get; set; }
    public double? HipCircumference { get; set; }
    public double? WaistLength { get; set; }
    public double? ArmLength { get; set; }
    public double? BicepSize { get; set; }
    public double? ModelLength { get; set; }
}


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClothingBrand.Domain.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public decimal Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigation property
        [JsonIgnore]
        public virtual ICollection<Product>? Products { get; set; }
    }

}

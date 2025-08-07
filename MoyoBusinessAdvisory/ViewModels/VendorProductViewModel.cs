using MoyoBusinessAdvisory.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoyoBusinessAdvisory.ViewModels
{
    public class VendorProductViewModel
    {

        public int? ProductId { get; set; }


        public string? VendorId { get; set; } // this has to be a string

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [ForeignKey("VendorId")]
        public Vendor? Vendor { get; set; }

        // public Price Price { get; set; }

        public double Price { get; set; }

        public int QuantityOnHand { get; set; }

        public ICollection<ProductOrder>? Orders { get; set; }
    }
}

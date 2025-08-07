using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoyoBusinessAdvisory.Models
{
     [PrimaryKey(nameof(VendorId), nameof(ProductId))]

    
    public class VendorProduct
    {
        // https://stackoverflow.com/questions/74379809/the-foreign-key-property-was-created-in-shadow-state-because-a-conflicting-pr

     
        //public int Id { get; set; }

       
        public int? ProductId { get; set; }

      
        public string? VendorId { get; set; } // this has to be a string

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("VendorId")]
        public Vendor Vendor { get; set; }

        // public Price Price { get; set; }

        public double Price { get; set; }

        public int QuantityOnHand { get; set; }

        public ICollection<ProductOrder>? Orders { get; set; }
        public void AssignVendortoOrder()
        {

        }

    }
}

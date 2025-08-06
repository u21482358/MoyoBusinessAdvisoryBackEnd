namespace MoyoBusinessAdvisory.Models
{
    public class Price
    {

        public int Id { get; set; }

        public double Amount { get; set; }

        public ICollection<VendorProduct>? VendorProducts { get; set; } // needed? probably not.

        public ICollection<ProductOrder>? ProductOrders { get; set; }
    }
}

namespace MoyoBusinessAdvisory.Models
{
    public class VendorProduct
    {

        public int Id { get; set; }
        public Product Product { get; set; }

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

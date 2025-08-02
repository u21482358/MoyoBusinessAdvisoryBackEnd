namespace MoyoBusinessAdvisory.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        //public int VendorID { get; set; }

        public Vendor Vendor { get; set; } // Navigation property to the Vendor

        public double price { get; set; }

        public int stockonHand { get; set; }
    }
}

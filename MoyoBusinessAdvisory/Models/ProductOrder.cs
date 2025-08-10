namespace MoyoBusinessAdvisory.Models
{
    public class ProductOrder
    {

        public int Id { get; set; }

        public VendorProduct VendorProduct { get; set; } // should it be many to one.

        public Client? Client { get; set; }

        public OrderStatus? OrderStatus { get; set; }

        public DateTime? OrderDate { get; set; }

        public double NumberOfItems { get; set; } // THIS IS BECAUSE PRICES CAN CHANGE IN THE FUTURE.

        public double? UnitPrice { get; set; }
    }
}

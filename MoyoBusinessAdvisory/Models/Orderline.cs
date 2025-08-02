namespace MoyoBusinessAdvisory.Models
{
    public class Orderline
    {

        public int Id { get; set; }
        public Product Product { get; set; }
       // public int OrderID { get; set; }

        public int Quantity { get; set; }
    }
}

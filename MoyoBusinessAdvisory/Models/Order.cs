namespace MoyoBusinessAdvisory.Models
{
    public class Order
    { // https://www.entityframeworktutorial.net/code-first/configure-one-to-many-relationship-in-code-first.aspx

        public int Id { get; set; }

        //public string OrderStatusID { get; set; }

        public ICollection<Orderline> Orderlines { get; set; }
        public DateTime PlacedOn { get; set; }
    }
}

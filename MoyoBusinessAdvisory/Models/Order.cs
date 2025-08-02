namespace MoyoBusinessAdvisory.Models
{
    public class Order
    { // https://www.entityframeworktutorial.net/code-first/configure-one-to-many-relationship-in-code-first.aspx\
      //    https://medium.com/@leroyleowdev/understanding-one-to-many-relationships-in-ef-core-with-examples-adb8db6eaa7c

        public int Id { get; set; }

        //public int OrderStatusID { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public Client Client { get; set; }

        public ICollection<Orderline> Orderlines { get; set; }
        public DateTime PlacedOn { get; set; }
    }
}

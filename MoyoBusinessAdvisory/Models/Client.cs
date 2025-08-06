namespace MoyoBusinessAdvisory.Models
{
    public class Client : AppUser
    {
        // public int Id { get; set; }
        // public string Name { get; set; }
        //public ICollection<Order>? Orders { get; set; }
        // only one username and password
        public ICollection<ProductOrder>? Orders { get; set; }

        public override List<ProductOrder> GetOrders(DataContext _context)
        {

            return this.Orders.ToList();
            Console.WriteLine("Drawing a generic shape.");
        }
    }
}

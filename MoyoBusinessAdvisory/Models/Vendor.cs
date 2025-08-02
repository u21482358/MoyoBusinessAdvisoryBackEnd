namespace MoyoBusinessAdvisory.Models
{
    public class Vendor : AppUser
    {
        public int Id { get; set; }
        //public string Name { get; set; }

        //public int userRoleID { get; set; } = 1;

        public ICollection<Product> Products { get; set; }
    }
}

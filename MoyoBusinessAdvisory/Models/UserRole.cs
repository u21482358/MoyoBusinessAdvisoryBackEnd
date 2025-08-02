namespace MoyoBusinessAdvisory.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
       // public ICollection<User> Students { get; set; } // dont need all users for a particular role.
    }
}

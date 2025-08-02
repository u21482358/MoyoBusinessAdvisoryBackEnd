namespace MoyoBusinessAdvisory.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UserRole UserRole { get; set; } // want to only get one user role. for each person. User can only have one role.

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}

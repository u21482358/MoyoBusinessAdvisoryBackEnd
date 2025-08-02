using Microsoft.AspNetCore.Identity;
namespace MoyoBusinessAdvisory.Models
{
    //https://medium.com/@leroyleowdev/understanding-one-to-many-relationships-in-ef-core-with-examples-adb8db6eaa7c
    public class AppUser : IdentityUser
    {
       // public int? Id { get; set; }
        public string Name { get; set; }


        public int? UserRoleID { get; set; } // this can be derived in the backend
        public UserRole? UserRole { get; set; } // want to only get one user role. for each person. User can only have one role.

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}

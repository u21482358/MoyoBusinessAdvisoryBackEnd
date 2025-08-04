using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
namespace MoyoBusinessAdvisory.Models
{
    //https://medium.com/@leroyleowdev/understanding-one-to-many-relationships-in-ef-core-with-examples-adb8db6eaa7c
    public class AppUser : IdentityUser
    {
        // public int? Id { get; set; }
        //[Required] //https://stackoverflow.com/questions/10710393/nullable-property-to-entity-field-entity-framework-through-code-first/53831297
        public string Name { get; set; }


        //public int? UserRoleID { get; set; } // this can be derived in the backend
       // public UserRole? UserRole { get; set; } // want to only get one user role. for each person. User can only have one role.

        //public string UserName { get; set; }

        public string Password { get; set; } // only a password hash

        
        public async Task<IdentityResult?> AddUser(UserManager<AppUser> _userManager, DataContext _context, RoleManager<IdentityRole> _roleManager,string role) // calling it on an object
        {
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var owner = await _userManager.FindByEmailAsync(Email);
                    var result = await _userManager.CreateAsync(this,Password);
                    if (result.Succeeded)
                    {
                        var currentUser = await _userManager.FindByIdAsync(Id);

                        var roleresult = await _userManager.AddToRoleAsync(currentUser, role);
                        if (roleresult.Succeeded)
                        {
                            await _context.SaveChangesAsync();
                            //await _context.SaveChanges();
                            // User added to role successfully
                        }
                        else
                        {
                            // Handle role assignment failure
                            return roleresult;
                        }
                    }
                    else
                    {
                        return result;
                    }
                    _context.SaveChanges();

                    dbcxtransaction.Commit();
                }
                catch (Exception ex)
                {
                    {
                        return IdentityResult.Failed();

                        // dbcxtransaction.Rollback();
                    }
                }
            }
            return IdentityResult.Success;
        }
    }
}

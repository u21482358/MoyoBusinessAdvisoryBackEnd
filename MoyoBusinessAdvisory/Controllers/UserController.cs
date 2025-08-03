//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using MoyoBusinessAdvisory.Models;
using System.Numerics;
namespace MoyoBusinessAdvisory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(DataContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }
        // GET: UserController


        // GET: UserController/Create
        //[HttpPost]
        //[Route("postVendor")]
        //public async Task<ActionResult<Product>> CreateVendor(Vendor vendor)
        //{
        //    // https://stackoverflow.com/questions/19689183/add-user-to-role-asp-net-identity
        //    // https://stackoverflow.com/questions/22486489/entity-framework-6-transaction-rollback
        //    using (var dbcxtransaction = _context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var owner = await _userManager.FindByNameAsync(vendor.UserName);
        //            var result = await _userManager.CreateAsync(vendor, vendor.Password);
        //            if (result.Succeeded)
        //            {
        //                var currentUser = await _userManager.FindByIdAsync(vendor.Id);

        //                var roleresult = await _userManager.AddToRoleAsync(currentUser, "vendor");
        //                if (roleresult.Succeeded)
        //                {
        //                    await _context.SaveChangesAsync();
        //                    //await _context.SaveChanges();
        //                    // User added to role successfully
        //                }
        //                else
        //                {
        //                    // Handle role assignment failure
        //                    return BadRequest(roleresult.Errors);
        //                }
        //            }
        //            else
        //            {
        //                return BadRequest(result.Errors);
        //            }
        //            _context.SaveChanges();

        //            dbcxtransaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            {
        //                return BadRequest(ex.Message);

        //                // dbcxtransaction.Rollback();
        //            }
        //        }
        //    }

        //AppUser.Children.Add(vendor);
        // _context.Users.Add(vendor);


        //        return CreatedAtAction("CreateVendor", new { id = vendor.Id }, vendor);
        //   // return View();
        //}
    

            //AppUser.Children.Add(vendor);
            // _context.Users.Add(vendor);


            
           // return View();
        

        [HttpGet]
        [Route("CreateUserRoles")]
        public async Task<ActionResult<Product>> CreateUserRoles()
        {
          var result =  await _roleManager.CreateAsync(new IdentityRole("vendor"));
            var result2 = await _roleManager.CreateAsync(new IdentityRole("client"));
            var result3 = await _roleManager.CreateAsync(new IdentityRole("capturer"));
            var result4 = await _roleManager.CreateAsync(new IdentityRole("manager"));
            return CreatedAtAction("CreateVendor", new { id = result }, result);
        }

        [HttpPost]
        [Route("postVendor")]
        public async Task<ActionResult<Product>> CreateVendor(Vendor vendor)
        {
            // https://stackoverflow.com/questions/19689183/add-user-to-role-asp-net-identity
            // https://stackoverflow.com/questions/22486489/entity-framework-6-transaction-rollback
            var result = await vendor.AddUser(_userManager, _context, _roleManager, "vendor");
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("postCapturer")]
        public async Task<ActionResult<Product>> CreateCapturer(AppUser user)
        {
            var result = await user.AddUser(_userManager, _context, _roleManager, "capturer");
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("postClient")]
        public async Task<ActionResult<Product>> CreateClient(Client client)
        {
            var result = await client.AddUser(_userManager, _context, _roleManager, "client");
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }
        // return View();


        [HttpPost]
        [Route("postManager")]
        // only to be run in swagger
        public async Task<ActionResult<Product>> CreateManager(AppUser user)
        {
            //UserManager.
            //AppUser.Children.Add(vendor);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("CreateVendor", new { id = user.Id }, user);
            // return View();
        }


        [HttpGet]
        [Route("getVendors")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetVendors()
        {
            // returning children of AppUser?

            // NB FIX THE RETURNING OF PASSWORD
           var vendors = await _context.Users.Where(x => x is Vendor).ToArrayAsync();
            //var users = await _context.Users.ToListAsync();
            //Vendor[] vendors = await _context.Users.OfType<Vendor>().ToArrayAsync();
            return vendors;
        }

        // POST: UserController/Create
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Create(IFormCollection collection)
    //    {
    //        try
    //        {
    //            return RedirectToAction(nameof(Index));
    //        }
    //        catch
    //        {
    //            return View();
    //        }
    //    }

    //    // GET: UserController/Edit/5
    //    public ActionResult Edit(int id)
    //    {
    //        return View();
    //    }

    //    // POST: UserController/Edit/5
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Edit(int id, IFormCollection collection)
    //    {
    //        try
    //        {
    //            return RedirectToAction(nameof(Index));
    //        }
    //        catch
    //        {
    //            return View();
    //        }
    //    }

    //    // GET: UserController/Delete/5
    //    public ActionResult Delete(int id)
    //    {
    //        return View();
    //    }

    //    // POST: UserController/Delete/5
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Delete(int id, IFormCollection collection)
    //    {
    //        try
    //        {
    //            return RedirectToAction(nameof(Index));
    //        }
    //        catch
    //        {
    //            return View();
    //        }
    //    }
    }
}

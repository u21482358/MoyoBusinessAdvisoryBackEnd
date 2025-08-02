using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoyoBusinessAdvisory.Models;
using Microsoft.EntityFrameworkCore;
namespace MoyoBusinessAdvisory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;
        }
        // GET: UserController
      

        // GET: UserController/Create
        [HttpPost]
        [Route("postVendor")]
        public async Task<ActionResult<Product>> CreateVendor(Vendor vendor)
        {
            //AppUser.Children.Add(vendor);
           _context.Users.Add(vendor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("CreateVendor", new { id = vendor.Id }, vendor);
           // return View();
        }


        [HttpGet]
        [Route("getVendors")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetVendors()
        {
            // returning children of AppUser?
           var vendors = await _context.Users.Where(x => x is Vendor).ToArrayAsync();
            var users = await _context.Users.ToListAsync();
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

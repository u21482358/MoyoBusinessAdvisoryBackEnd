using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoyoBusinessAdvisory.Models;
using System.Security.Claims;

namespace MoyoBusinessAdvisory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipalFactory;
        public OrderController(DataContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IUserClaimsPrincipalFactory<AppUser> claimsPrincipalFactory)
        {

            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _configuration = configuration;
            _claimsPrincipalFactory = claimsPrincipalFactory;
        }
        // GET: OrderController
        //    public ActionResult GetCustomerOrders()
        //    {
        //        return View();
        //    }

        //    // GET: OrderController/Details/5
        //    // For Capturer
        //    public ActionResult GetAllOrders(int id)
        //    {
        //        return View();
        //    }

        //    // GET: OrderController/Create
        //    public ActionResult GetVendorOrders()
        //    {
        //        return View();
        //    }

        // POST: OrderController/Create
        [HttpPost]
        [Route("post")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        
        public async Task<ActionResult> PlaceOrder(ProductOrder productOrder)
        {

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Client currentUser = (Client)await _userManager.FindByIdAsync(userId);
            // then assign that user to the order.
            _context.Attach(productOrder.VendorProduct);
            productOrder.OrderDate = DateTime.UtcNow;
            productOrder.Client = currentUser;
            productOrder.UnitPrice = productOrder.VendorProduct.Price;
            _context.Orders.Add(productOrder);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("get")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult> GetOrders()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          // Client currentUser = _userManager.FindByIdAsync(userId);


            // you can call an override method on a specific type.
            return Ok();
        }

        //    // GET: OrderController/Edit/5
        //    public ActionResult Edit(int id)
        //    {
        //        return View();
        //    }

        //    // POST: OrderController/Edit/5
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

        //    // GET: OrderController/Delete/5
        //    public ActionResult Delete(int id)
        //    {
        //        return View();
        //    }

        //    // POST: OrderController/Delete/5
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

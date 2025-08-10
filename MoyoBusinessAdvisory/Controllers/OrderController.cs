using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoyoBusinessAdvisory.Models;
using System.Data.Entity;
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
      

        // POST: OrderController/Create
        [HttpPost]
        [Route("post")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        
        public async Task<ActionResult> PlaceOrder(ProductOrder productOrder)
        {

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Client currentUser = (Client)await _userManager.FindByIdAsync(userId);
            // then assign that user to the order.\
            productOrder.VendorProduct.ProductId = productOrder.VendorProduct.Product.Id;
            productOrder.VendorProduct.VendorId = productOrder.VendorProduct.Vendor.Id;
            var productId = productOrder.VendorProduct.ProductId;
            // var vendorProduct = _context.VendorProducts.Where(c => c.VendorId == productOrder.VendorProduct.Vendor.Id && c.ProductId == productOrder.VendorProduct.Product.Id).AsNoTracking().FirstOrDefault();
            _context.Attach(productOrder.VendorProduct);
            //_context.VendorProducts.Attach(productOrder.VendorProduct);
            productOrder.OrderDate = DateTime.Now;
            productOrder.Client = currentUser;
            productOrder.OrderStatus = _context.OrderStatuses.Find(1);
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
          AppUser currentUser = await _userManager.FindByIdAsync(userId);
            object products;
           var orders = currentUser.GetOrders(_context);


            // you can call an override method on a specific type.
            return Ok(orders);
        }

        [HttpPut]
        [Route("put")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult> UpdateProductOrderStatus(ProductOrder productOrder)
        {
           var Order = _context.Orders.Find(productOrder.Id);
            Order.OrderStatus = _context.OrderStatuses.Find(productOrder.OrderStatus.Id);
            _context.Orders.Update(Order);
            await _context.SaveChangesAsync();
            return Ok();
        }





    }
}

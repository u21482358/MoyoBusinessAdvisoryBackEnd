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
            // https://stackoverflow.com/questions/54080975/update-newly-inserted-records-id-to-another-table-using-entity-framework
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Client currentUser = (Client)await _userManager.FindByIdAsync(userId);
                // then assign that user to the order.\

                var selectedProd = productOrder.VendorProduct;
                //vendorProd.
                var vendorProduct = _context.VendorProducts.Find(selectedProd.VendorId, selectedProd.ProductId);
                vendorProduct.QuantityOnHand = vendorProduct.QuantityOnHand - productOrder.NumberOfItems;

                _context.Update(vendorProduct);
                await _context.SaveChangesAsync();
                _context.Entry(vendorProduct).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
               // _context.ChangeTracker.Clear();
                productOrder.VendorProduct = vendorProduct;
                _context.VendorProducts.Attach(vendorProduct);
                productOrder.OrderDate = DateTime.Now;
                productOrder.Client = currentUser;
                productOrder.OrderStatus = _context.OrderStatuses.Find(1);
                productOrder.UnitPrice = productOrder.VendorProduct.Price;
                await _context.Orders.AddAsync(productOrder);
                //var vendor _context.VendorProducts.Where(c => c.ProductId == productOrder.VendorProduct.ProductId && c.VendorId == productOrder.VendorProduct.VendorId).FirstOrDefault();

                await _context.SaveChangesAsync();
                dbcxtransaction.Commit();
            }
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
           var orders = await currentUser.GetOrders(_context);


            // you can call an override method on a specific type.
            return Ok(orders);
        }

        [HttpPut]
        [Route("put")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult> UpdateProductOrderStatus(ProductOrder productOrder)
        {
           var Order = await _context.Orders.FindAsync(productOrder.Id);
            Order.OrderStatus = await _context.OrderStatuses.FindAsync(productOrder.OrderStatus.Id);
            _context.Orders.Update(Order);
            await _context.SaveChangesAsync();
            return Ok();
        }





    }
}

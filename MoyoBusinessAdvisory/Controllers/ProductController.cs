using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoyoBusinessAdvisory.Models;
using System.Security.Claims;
using MoyoBusinessAdvisory.ViewModels;
using Microsoft.AspNetCore.Identity;
namespace MoyoBusinessAdvisory.Controllers
{

    // https://github.com/INF370Development/inf370-team6

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {

        //    private readonly DataContextcs _context;
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ProductController(DataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost]
        [Route("post")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Product>> PostVendorProduct(VendorProduct vendorprod)
        {

            // cannot insert duplicate key in object asp net microsoft entity framework core.
            // https://stackoverflow.com/questions/29272581/why-ef-navigation-property-return-null
            // _context.Users.Attach(prod.Vendor); // attaches to make sure that stage is unchanged...
            //prod.
            //https://stackoverflow.com/questions/75600798/inserting-a-dependent-entity-while-inserting-the-principal-entity-with-entity-fr
            // prod.Vendor = prod.Vendor;
            //var productLookUp = await _context.Products.FindAsync(vendorprod.Product.Id);
            //if (productLookUp != null)
            //{

            //    _context.VendorProducts.a
            //    // probably not necessary. as entity is already tracked
            //    //_context.Products.Attach(vendorprod.Product);
            //}

             _context.Vendors.Attach(vendorprod.Vendor);
            //_context.Products.Attach(vendorprod.Product);
            var product = _context.VendorProducts.Add(vendorprod);
            
                
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = vendorprod }, vendorprod);
        }

        [HttpPost]
        [Route("assignProductToVendor")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Product>> AssignProductToVendor(VendorProduct vendorprod)
        {

            // cannot insert duplicate key in object asp net microsoft entity framework core.
            // https://stackoverflow.com/questions/29272581/why-ef-navigation-property-return-null
            // _context.Users.Attach(prod.Vendor); // attaches to make sure that stage is unchanged...
            //prod.
            //https://stackoverflow.com/questions/75600798/inserting-a-dependent-entity-while-inserting-the-principal-entity-with-entity-fr
            // prod.Vendor = prod.Vendor;
            //var productLookUp = await _context.Products.FindAsync(vendorprod.Product.Id);
            //if (productLookUp != null)
            //{

            //    _context.VendorProducts.a
            //    // probably not necessary. as entity is already tracked
            //    //_context.Products.Attach(vendorprod.Product);
            //}

            _context.Vendors.Attach(vendorprod.Vendor);
            _context.Products.Attach(vendorprod.Product);
            var product = _context.VendorProducts.Add(vendorprod);


            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = vendorprod }, vendorprod);
        }

        [HttpPut]
        [Route("put")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Product>> PutProduct(Product prod)
        {
            // cannot insert duplicate key in object asp net microsoft entity framework core.
            // https://stackoverflow.com/questions/29272581/why-ef-navigation-property-return-null
            //_context.Users.Attach(prod.Vendor); // attaches to make sure that stage is unchanged...
                                                //prod.
                                                //https://stackoverflow.com/questions/75600798/inserting-a-dependent-entity-while-inserting-the-principal-entity-with-entity-fr
                                                // prod.Vendor = prod.Vendor;


            var product = _context.Products.Update(prod);


            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = prod.Id }, prod);
        }

        [HttpPost]
        [Route("post2")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> PostProductViaVendorICollection([FromBody] Product prod)
        {
            // cannot insert duplicate key in object asp net microsoft entity framework core.
            // https://stackoverflow.com/questions/29272581/why-ef-navigation-property-return-null
            // attaches to make sure that stage is unchanged...
            //prod.
            //https://stackoverflow.com/questions/75600798/inserting-a-dependent-entity-while-inserting-the-principal-entity-with-entity-fr
            // prod.Vendor = prod.Vendor;
            //var vendor = _context.Vendors.Where(p => p == prod.Vendor).FirstOrDefault();
            //var products = _context.Vendors.prd
          //  var prods = await _context.Vendors.Include(vendor => vendor.Products).ToListAsync();



            //if (vendor == null)
            //{
            //    return NotFound("Vendor not found");
            //}
            //else
            //{

            //    _context.Products.Add(prod);
            //    vendor.Products.Add(prod);
            //    await _context.SaveChangesAsync();
            //    //vendor.\
            return Ok();
        }

        [HttpPost]
        [Route("UnassignedVendors")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> ReturnUnassignedVendorstoproduct([FromBody] Product prod)
        {
            // cannot insert duplicate key in object asp net microsoft entity framework core.
            // https://stackoverflow.com/questions/29272581/why-ef-navigation-property-return-null
            // attaches to make sure that stage is unchanged...
            //prod.
            //https://stackoverflow.com/questions/75600798/inserting-a-dependent-entity-while-inserting-the-principal-entity-with-entity-fr
            // prod.Vendor = prod.Vendor;
            //var vendor = _context.Vendors.Where(p => p == prod.Vendor).FirstOrDefault();
            //var products = _context.Vendors.prd
            // https://stackoverflow.com/questions/5640259/linq-select-where-object-does-not-contain-items-from-list
            // var rejectList = await _context.Vendors.Select(c => c.Products).Where(i => i.Id == ).ToListAsync();
            var rejectList = _context.VendorProducts.Where(i => i.Product.Id == prod.Id).Select(c => c.Vendor); // needs to be Iqueryable
            var vendors = await _context.Vendors.Except(rejectList).ToListAsync();
            return Ok(vendors);
        }


        // This is for when you place an order
        [HttpPost]
        [Route("GetVendorsOfferingProduct")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetVendorsOfferingproduct([FromBody] Product prod)
        {
            // excluding product from select.
            var vendorProducts = _context.VendorProducts.Where(c => c.Product == prod && c.QuantityOnHand > 0).Select(c => new VendorProduct { Vendor = c.Vendor, Price = c.Price, QuantityOnHand = c.QuantityOnHand }).OrderBy(c => c.Price).ToList();

            return Json(vendorProducts);
        }

















        [HttpGet]
        [Route("get")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts() {

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            AppUser currentUser = await _userManager.FindByIdAsync(userId);

           var products = currentUser.GetProducts(_context);
            //https://stackoverflow.com/questions/67890726/ef-core-trying-to-insert-relational-data-that-already-exists
          
         var vendors = await _context.Vendors.ToListAsync(); // gets the reference to the object.
            // https://stackoverflow.com/questions/28745798/how-should-i-return-two-lists-of-objects-from-one-controller-action
            return Json(new
            {
                products = products,
                vendors = vendors
            });
        }

        //[HttpGet]
        //[Route("get")]
        //public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        //{
        //    //https://stackoverflow.com/questions/67890726/ef-core-trying-to-insert-relational-data-that-already-exists
        //    var prods = await _context.Products.ToListAsync();
        //    var vendors = await _context.Vendors.ToListAsync(); // gets the reference to the object.
        //    // https://stackoverflow.com/questions/28745798/how-should-i-return-two-lists-of-objects-from-one-controller-action
        //    return Json(new
        //    {
        //        products = prods,
        //        vendors = vendors
        //    });
        //}
        //[HttpGet]
        //[Route("get")]
        //public async Task<ActionResult<IEnumerable<Product>>> GetVendorProducts()
        //{
        //    //https://stackoverflow.com/questions/67890726/ef-core-trying-to-insert-relational-data-that-already-exists
        //    var prods = await _context.Products.ToListAsync();
        //    var vendors = await _context.Vendors.ToListAsync(); // gets the reference to the object.
        //    // https://stackoverflow.com/questions/28745798/how-should-i-return-two-lists-of-objects-from-one-controller-action
        //    return Json(new
        //    {
        //        products = prods,
        //        vendors = vendors
        //    });
        //}


    }
}

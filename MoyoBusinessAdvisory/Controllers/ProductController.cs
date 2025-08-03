using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoyoBusinessAdvisory.Models;
using Microsoft.EntityFrameworkCore;
namespace MoyoBusinessAdvisory.Controllers
{

    // https://github.com/INF370Development/inf370-team6

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {

        //    private readonly DataContextcs _context;
        private readonly DataContext _context;
        public ProductController(DataContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("post")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Product>> PostProduct(Product prod)
        {
        // cannot insert duplicate key in object asp net microsoft entity framework core.
       // https://stackoverflow.com/questions/29272581/why-ef-navigation-property-return-null
            _context.Users.Attach(prod.Vendor); // attaches to make sure that stage is unchanged...
                                                //prod.
                                                //https://stackoverflow.com/questions/75600798/inserting-a-dependent-entity-while-inserting-the-principal-entity-with-entity-fr
                                                // prod.Vendor = prod.Vendor;
           

                var product = _context.Products.Add(prod);
            
                
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = prod.Id }, prod);
        }

        [HttpPut]
        [Route("put")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Product>> PutProduct(Product prod)
        {
            // cannot insert duplicate key in object asp net microsoft entity framework core.
            // https://stackoverflow.com/questions/29272581/why-ef-navigation-property-return-null
            _context.Users.Attach(prod.Vendor); // attaches to make sure that stage is unchanged...
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
        public async Task<ActionResult<Product>> PostProductViaVendorICollection(Product prod)
        {
            // cannot insert duplicate key in object asp net microsoft entity framework core.
            // https://stackoverflow.com/questions/29272581/why-ef-navigation-property-return-null
            _context.Users.Attach(prod.Vendor); // attaches to make sure that stage is unchanged...
                                                //prod.
                                                //https://stackoverflow.com/questions/75600798/inserting-a-dependent-entity-while-inserting-the-principal-entity-with-entity-fr
                                                // prod.Vendor = prod.Vendor;


            var vendors = await _context.Users.Where(x => x is Vendor).ToArrayAsync();
            var vendor = vendors.FirstOrDefault(x => x.Id == prod.Vendor.Id);
            if (vendor == null)
            {
                return NotFound("Vendor not found");
            }
            else
            {
                //vendor.
            }
            return CreatedAtAction("GetProduct", new { id = prod.Id }, prod);
        }



      

[HttpGet]
        [Route("get")]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
            //https://stackoverflow.com/questions/67890726/ef-core-trying-to-insert-relational-data-that-already-exists
        { var prods = await _context.Products.Include(c => c.Vendor).ToListAsync(); // gets the reference to the object.
            return prods;
        }


    }
}

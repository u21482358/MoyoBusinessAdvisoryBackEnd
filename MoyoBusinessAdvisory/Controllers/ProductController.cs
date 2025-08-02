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
             _context.Users.Attach(prod.Vendor); // attaches to make sure that stage is unchanged...
            //prod.
            //https://stackoverflow.com/questions/75600798/inserting-a-dependent-entity-while-inserting-the-principal-entity-with-entity-fr
           // prod.Vendor = prod.Vendor;
            
            var product = _context.Products.Add(prod);
            
                
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = prod.Id }, prod);
        }

        [HttpGet]
        [Route("get")]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        { var prods = await _context.Products.Include(c => c.Vendor).ToListAsync(); // gets the reference to the object.
            return prods;
        }
    }
}

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
            _context.Products.Add(prod);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = prod.Id }, prod);
        }

        [HttpGet]
        [Route("get")]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }
    }
}

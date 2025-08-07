using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MoyoBusinessAdvisory.Models
{
    public class Vendor : AppUser
    {
        //public int Id { get; set; } // doesnt need ID as already in AppUser
        //public string Name { get; set; }

        //public int userRoleID { get; set; } = 1;

        public ICollection<VendorProduct>? VendorProducts { get; set; }

        

        public override List<ProductOrder> GetOrders(DataContext _context)
        {
           return _context.Orders.Where(c => c.VendorProduct.Vendor.Id == Id).Include(c => c.VendorProduct.Product).Include(c => c.Client).ToList();
          //  Console.WriteLine("Drawing a generic shape.");
        }

        public override void GetProducts(DataContext _context,out object products)
        {
            // https://stackoverflow.com/questions/2537823/distinct-by-property-of-class-with-linq
            // maybe this will work?
            // already grouped.
            products = _context.VendorProducts.Where(c => c.Vendor.Id == Id).Include(p => p.Product).Select(c => new { name = c.Product.Name,productId= c.ProductId, Price = c.Price, QuantityOnHand = c.QuantityOnHand }).ToList();
            //var products = _context.Products.Where(c => c.Id == vendorproducts.pr)
            // selecting from all vendor products var val = _context.VendorProducts.Include(c => c.Product).Where(c => c.Vendor.Id == Id).Select(c => c.Product).ToList();
            //return variable;
            // https://stackoverflow.com/questions/32436699/how-do-you-use-firstordefault-with-include
            
                //WhToList();
            //  Console.WriteLine("Drawing a generic shape.");
        }
    }
}

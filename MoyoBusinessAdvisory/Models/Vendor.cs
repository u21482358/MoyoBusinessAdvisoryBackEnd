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
           return _context.Orders.Where(c => c.VendorProduct.Vendor.Id == Id).Include(c => c.VendorProduct.Product).Include(c => c.Client).Include(c => c.OrderStatus).ToList();
            //  Console.WriteLine("Drawing a generic shape.");
        }

        public override void GetProducts(DataContext _context,out object products)
        {
            // https://stackoverflow.com/questions/2537823/distinct-by-property-of-class-with-linq
            products = _context.VendorProducts.Where(c => c.Vendor.Id == Id).Include(p => p.Product).Select(c => new { name = c.Product.Name,productId= c.ProductId, Price = c.Price, QuantityOnHand = c.QuantityOnHand }).ToList();
            
            // https://stackoverflow.com/questions/32436699/how-do-you-use-firstordefault-with-include
            
                //WhToList();
            //  Console.WriteLine("Drawing a generic shape.");
        }
    }
}

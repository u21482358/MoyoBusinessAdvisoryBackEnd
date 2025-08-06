using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MoyoBusinessAdvisory.Models
{
    public class Vendor : AppUser
    {
        //public int Id { get; set; } // doesnt need ID as already in AppUser
        //public string Name { get; set; }

        //public int userRoleID { get; set; } = 1;

        public ICollection<VendorProduct> VendorProducts { get; } = [];

      //  public ICollection<VendorProduct> VendorProduct { get; } = [];

        public override List<ProductOrder> GetOrders(DataContext _context)
        {
           return _context.Orders.Where(c => c.VendorProduct.Vendor.Id == Id).ToList();
          //  Console.WriteLine("Drawing a generic shape.");
        }

        public override List<Product> GetProducts(DataContext _context)
        {

            var val = _context.VendorProducts.Include(c => c.Product).Where(c => c.Vendor.Id == Id).Select(c => c.Product).ToList();
            return val;
            // https://stackoverflow.com/questions/32436699/how-do-you-use-firstordefault-with-include
            
                //WhToList();
            //  Console.WriteLine("Drawing a generic shape.");
        }
    }
}

using ConcurrencyExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ConcurrencyExample.Controllers
{
    public class ProductController : Controller
    {
        private readonly MssqlDbContext _mssqlDbContext;
        public ProductController(MssqlDbContext mssqlDbContext)
        {
            _mssqlDbContext = mssqlDbContext;
        }
        public async Task<IActionResult> List()
        {
            return View(await _mssqlDbContext.Products.ToListAsync());
        }
        public async Task<IActionResult> Update(int id)
        {
            var product = await _mssqlDbContext.Products.FindAsync(id);
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            try
            {
                _mssqlDbContext.Update(product);
                await _mssqlDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var exceptionEntry = ex.Entries.First();
                var currentValues = exceptionEntry.Entity as Product;
                var dbValues = exceptionEntry.GetDatabaseValues();
               
                var clientValues = exceptionEntry.CurrentValues;
                if (dbValues == null)
                {
                    ModelState.AddModelError(string.Empty, "Ürün başka bir kullanıcı tararfından silindi");
                }
                else
                {
                    var dbProducts = dbValues.ToObject() as Product;
                    ModelState.AddModelError(string.Empty, "Ürün başka bir kullanıcı tararfından güncellendi");
                    ModelState.AddModelError(string.Empty, $"Ad:{dbProducts.Name}, Ücret:{dbProducts.Price}, Stok:{dbProducts.Stock}");
                }
                return View();
            }
        }
    }
}

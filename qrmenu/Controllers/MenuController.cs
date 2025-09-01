using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qrmenu.Data;
using qrmenu.Models;

namespace qrmenu.Controllers
{
    public class MenuController : Controller
    {
        private readonly AppDbContext _context;

        public MenuController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("Menu/Welcome")]
        public async Task<IActionResult> Welcome()
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync();
            var scanLog = new ScanLog
            {
                ScannedAt = DateTime.UtcNow
            };
            _context.ScanLogs.Add(scanLog);
            await _context.SaveChangesAsync();

            return View(restaurant);
        }
        public IActionResult GetProductsByCategory(int categoryId)
        {
            var products = _context.Products.Where(p => p.CategoryId == categoryId).OrderByDescending(p => p.IsActive).ToList();
            return PartialView("_ProductsGrid", products);
        }


        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.OrderBy(c => c.DisplayOrder).ToListAsync();
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync();

            ViewBag.Categories = categories;
            if (restaurant != null)
            {
                ViewBag.RestaurantLogo = restaurant.LogoUrl;
                ViewBag.FacebookUrl = restaurant.FacebookUrl;
                ViewBag.InstagramUrl = restaurant.InstagramUrl;
            }

            return View(); 
        }

        public async Task<IActionResult> Products(int categoryId)
        {
            var products = await _context.Products
                                         .Where(p => p.CategoryId == categoryId)
                                         .OrderByDescending(p => p.IsActive)
                                         .ToListAsync();
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync();
            ViewBag.RestaurantLogo = restaurant?.LogoUrl;
            ViewBag.FacebookUrl = restaurant?.FacebookUrl;
            ViewBag.InstagramUrl = restaurant?.InstagramUrl;

            return View(products);
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qrmenu.Data;
using qrmenu.Models;

[Authorize]
public class RestaurantController : Controller
{
    private readonly AppDbContext _context;

    public RestaurantController(AppDbContext context)
    {
        _context = context;
    }

   
    public async Task<IActionResult> Index()
    {
        var restaurant = await _context.Restaurants.FirstOrDefaultAsync();
        if (restaurant == null)
        {
            restaurant = new Restaurant { Name = "Restoran" };
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
        }

        ViewBag.RestaurantLogo = restaurant.LogoUrl;
        ViewBag.FacebookUrl = restaurant.FacebookUrl;
        ViewBag.InstagramUrl = restaurant.InstagramUrl;

        return View(restaurant);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(Restaurant restaurant, IFormFile logoFile)
    {
        var existing = await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == restaurant.Id);
        if (existing == null) return NotFound();

         Console.WriteLine($"FacebookUrl (Form): {restaurant.FacebookUrl}");
        Console.WriteLine($"InstagramUrl (Form): {restaurant.InstagramUrl}");

        existing.Name = restaurant.Name;
        existing.FacebookUrl = restaurant.FacebookUrl;
        existing.InstagramUrl = restaurant.InstagramUrl;

        if (logoFile != null && logoFile.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(logoFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await logoFile.CopyToAsync(stream);
            }

            existing.LogoUrl = "/uploads/" + uniqueFileName;
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

}

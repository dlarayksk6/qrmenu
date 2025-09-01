using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qrmenu.Data;
using qrmenu.Models;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
[Authorize]
public class CategoryController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public CategoryController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

   
    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        ViewBag.Count = categories.Count;
        return View(categories);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCategory(string name, IFormFile imageFile)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            var maxOrder = _context.Categories.Any() ? _context.Categories.Max(c => c.DisplayOrder) : 0;

            var category = new Category
            {
                Name = name,
                DisplayOrder = maxOrder + 1
            };

            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/categories");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);

                using (var image = Image.Load<Rgba32>(imageFile.OpenReadStream()))
                {
                    
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(800, 800),
                        Mode = ResizeMode.Max
                    }));

                    await image.SaveAsJpegAsync(filePath, new JpegEncoder
                    {
                        Quality = 90
                    });
                }

                category.ImageUrl = "/uploads/categories/" + fileName;
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
        [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
        
            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                var filePath = Path.Combine("wwwroot", category.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public async Task<IActionResult> EditCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCategory(int id, string name, IFormFile imageFile)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();

        category.Name = name;

        if (imageFile != null && imageFile.Length > 0)
        {
            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                var oldPath = Path.Combine("wwwroot", category.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/categories");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            category.ImageUrl = "/uploads/categories/" + fileName;
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public async Task<IActionResult> MoveUp(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();

        var above = await _context.Categories
            .Where(c => c.DisplayOrder < category.DisplayOrder)
            .OrderByDescending(c => c.DisplayOrder)
            .FirstOrDefaultAsync();

        if (above != null)
        {
            int temp = category.DisplayOrder;
            category.DisplayOrder = above.DisplayOrder;
            above.DisplayOrder = temp;

            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }


    [HttpPost]
    public async Task<IActionResult> MoveDown(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();

        var below = await _context.Categories
            .Where(c => c.DisplayOrder > category.DisplayOrder)
            .OrderBy(c => c.DisplayOrder)
            .FirstOrDefaultAsync();

        if (below != null)
        {
            int temp = category.DisplayOrder;
            category.DisplayOrder = below.DisplayOrder;
            below.DisplayOrder = temp;

            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}

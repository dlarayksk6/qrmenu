using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qrmenu.Data;
using qrmenu.Models;

[Authorize]
public class PreviewController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public PreviewController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public async Task<IActionResult> Index(int? categoryId)
    {
        var categories = await _context.Categories
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        
        if (!categories.Any())
        {
            ViewBag.Categories = new List<Category>();
            ViewBag.SelectedCategoryId = null;
            return View(new List<Product>());
        }

        
        var selectedCategory = categoryId.HasValue
            ? categories.FirstOrDefault(c => c.Id == categoryId.Value)
            : categories.First();

        var products = await _context.Products
            .Where(p => selectedCategory != null && p.CategoryId == selectedCategory.Id)
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync();

        ViewBag.Categories = categories;
        ViewBag.SelectedCategoryId = selectedCategory?.Id;

        return View(products);
    }


    [HttpPost]
    public async Task<IActionResult> UpdateOrder([FromBody] List<ProductOrderUpdate> items)
    {
        foreach (var i in items)
        {
            var product = await _context.Products.FindAsync(i.Id);
            if (product != null)
            {
                product.DisplayOrder = i.Order;
            }
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    public class ProductOrderUpdate
    {
        public int Id { get; set; }
        public int Order { get; set; }
    }
}

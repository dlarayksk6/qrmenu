using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qrmenu.Data;
using qrmenu.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace qrmenu.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model, IFormFile imageFile)
        {
            if (imageFile is { Length: > 0 })
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var savePath = Path.Combine("wwwroot/uploads", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

                using var mem = new MemoryStream();
                await imageFile.CopyToAsync(mem);
                mem.Position = 0;

                using (var image = Image.Load(mem))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(600, 600),
                        Mode = ResizeMode.Crop
                    }));

                    await image.SaveAsync(savePath);
                }

                model.ImageUrl = "/uploads/" + fileName;
            }

            model.IsActive = true;

            _context.Products.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            ViewBag.Categories = await _context.Categories
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product model, IFormFile imageFile)
        {
            if (id != model.Id) return BadRequest();

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            product.Name = model.Name;
            product.Price = model.Price;
            product.Description = model.Description;
            product.CategoryId = model.CategoryId;
            product.IsActive = model.IsActive;

            if (imageFile is { Length: > 0 })
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var savePath = Path.Combine("wwwroot/uploads", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

                using var mem = new MemoryStream();
                await imageFile.CopyToAsync(mem);
                mem.Position = 0;

                using (var image = Image.Load(mem))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(600, 600),
                        Mode = ResizeMode.Crop
                    }));

                    await image.SaveAsync(savePath);
                }

                product.ImageUrl = "/uploads/" + fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var filePath = Path.Combine("wwwroot", product.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.IsActive = !product.IsActive;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> MoveUp(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            var above = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.DisplayOrder < product.DisplayOrder)
                .OrderByDescending(p => p.DisplayOrder)
                .FirstOrDefaultAsync();

            if (above != null)
            {
                int temp = product.DisplayOrder;
                product.DisplayOrder = above.DisplayOrder;
                above.DisplayOrder = temp;

                _context.Products.Update(product);
                _context.Products.Update(above);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> MoveDown(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            var below = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.DisplayOrder > product.DisplayOrder)
                .OrderBy(p => p.DisplayOrder)
                .FirstOrDefaultAsync();

            if (below != null)
            {
                int temp = product.DisplayOrder;
                product.DisplayOrder = below.DisplayOrder;
                below.DisplayOrder = temp;

                _context.Products.Update(product);
                _context.Products.Update(below);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
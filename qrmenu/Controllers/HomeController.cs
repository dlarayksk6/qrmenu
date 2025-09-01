using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qrmenu.Data;


namespace qrmenu.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var scanLogs = await _context.ScanLogs.ToListAsync();

 
            ViewBag.CategoryCount = await _context.Categories.CountAsync();
            ViewBag.ProductCount = await _context.Products.CountAsync();
            ViewBag.PassiveProductCount = await _context.Products.CountAsync(p => !p.IsActive);

    
            ViewBag.DailyTotalClicks = scanLogs.Count(s => s.ScannedAt.Date == DateTime.Today);

         
            var lastNDays = 14;
            var dateRange = Enumerable.Range(0, lastNDays)
                                      .Select(i => DateTime.Today.AddDays(-(lastNDays - 1) + i).Date)
                                      .ToList();

            ViewBag.GeneralDailyLabels = dateRange.Select(d => d.ToString("dd.MM")).ToList();
            ViewBag.GeneralDailyCounts = dateRange.Select(d => scanLogs.Count(s => s.ScannedAt.Date == d)).ToList();

            var hours = Enumerable.Range(0, 24).ToList();
            ViewBag.GeneralHourLabels = hours.Select(h => h.ToString()).ToList();
            ViewBag.GeneralHourlyCounts = hours.Select(h => scanLogs.Count(s => s.ScannedAt.Hour == h)).ToList();

          
            var weekRange = Enumerable.Range(0, 7).Select(i => DateTime.Today.AddDays(-6 + i)).ToList();
            ViewBag.GeneralWeeklyLabels = weekRange.Select(d => d.ToString("dd.MM")).ToList();
            ViewBag.GeneralWeeklyCounts = weekRange.Select(d => scanLogs.Count(s => s.ScannedAt.Date == d)).ToList();

            return View();
        }
    }
}

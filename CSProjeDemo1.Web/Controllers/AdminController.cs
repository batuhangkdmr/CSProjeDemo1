using CSProjeDemo1.Core.Entities;
using CSProjeDemo1.Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSProjeDemo1.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly KutuphaneContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(KutuphaneContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> OduncList()
        {
            var oduncAlinanKitaplar = await _context.OduncAlmalar
                .Include(o => o.Kitap)
                .Include(o => o.ApplicationUser)
                .ToListAsync();

            return View(oduncAlinanKitaplar);
        }
        public async Task<IActionResult> UserList()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }
        [HttpPost] // Burada HTTP POST veya DELETE kullanmamız gerekiyor
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                TempData["Error"] = "Kullanıcı silinemedi.";
                return RedirectToAction("UserList");
            }

            TempData["Success"] = "Kullanıcı başarıyla silindi.";
            return RedirectToAction("UserList");
        }

    }
}

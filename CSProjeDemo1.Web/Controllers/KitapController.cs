using CSProjeDemo1.Core.Entities;
using CSProjeDemo1.Service.Interfaces;
using CSProjeDemo1.Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CSProjeDemo1.Web.Controllers
{
    public class KitapController : Controller
    {
        private readonly IKitapService _kitapService;
        private readonly KutuphaneContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public KitapController(IKitapService kitapService, KutuphaneContext context, UserManager<ApplicationUser> userManager)
        {
            _kitapService = kitapService;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var kitaplar = await _kitapService.GetAvailableBooksAsync(); // Sadece ödünç alınabilir kitapları getir
            return View(kitaplar);
        }

        [Authorize] // Kullanıcı girişi zorunlu
        public async Task<IActionResult> OduncAl(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Auth");

            var result = await _kitapService.OduncAlAsync(id, user.Id);

            if (!result)
                TempData["Error"] = "Kitap ödünç alınamadı. Lütfen tekrar deneyiniz.";

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> IadeEt(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Auth");

            var result = await _kitapService.IadeEtAsync(id, user.Id);

            if (!result)
                TempData["Error"] = "Kitap iade edilemedi. Lütfen tekrar deneyiniz.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // Sadece adminler ekleme yapabilir
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(Kitap kitap)
        {
            if (ModelState.IsValid)
            {
                await _kitapService.AddKitapAsync(kitap);
                return RedirectToAction("Index");
            }
            return View(kitap);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var kitap = await _kitapService.GetBookByIdAsync(id);
            if (kitap == null) return NotFound();

            return View(kitap);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Kitap kitap)
        {
            if (id != kitap.Id) return BadRequest();
            if (!ModelState.IsValid) return View(kitap);

            await _kitapService.UpdateKitapAsync(kitap);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var kitap = await _kitapService.GetBookByIdAsync(id);
            if (kitap == null) return NotFound();

            await _kitapService.DeleteKitapAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

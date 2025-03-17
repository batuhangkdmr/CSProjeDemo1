using CSProjeDemo1.Core.Entities;
using CSProjeDemo1.Data.Contexts;
using CSProjeDemo1.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CSProjeDemo1.Web.Controllers
{
    [Authorize] // Giriş yapmış kullanıcılar işlem yapabilir.
    public class OduncController : Controller
    {
        private readonly KutuphaneContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOduncAlmaService _oduncAlmaService;

        public OduncController(KutuphaneContext context, UserManager<ApplicationUser> userManager, IOduncAlmaService oduncAlmaService)
        {
            _context = context;
            _userManager = userManager;
            _oduncAlmaService = oduncAlmaService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Console.WriteLine("HATA: Kullanıcı bilgisi alınamadı!");
                return RedirectToAction("Login", "Auth");
            }

            Console.WriteLine($"DEBUG: Ödünç alınan kitapları getir - KullanıcıID: {user.Id}");

            var oduncAlinanKitaplar = await _context.OduncAlmalar
                .Where(o => o.ApplicationUserId == user.Id && o.IadeTarihi == null)
                .Include(o => o.Kitap)
                .ToListAsync();

            Console.WriteLine($"DEBUG: {oduncAlinanKitaplar.Count} adet ödünç alınan kitap bulundu.");

            return View(oduncAlinanKitaplar);
        }


        [HttpPost]
        public async Task<IActionResult> OduncAl(int kitapId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Console.WriteLine("DEBUG: Kullanıcı giriş yapmamış!");
                return RedirectToAction("Login", "Auth");
            }

            Console.WriteLine($"DEBUG: Kullanıcı {user.Id} için kitap ödünç alınacak. KitapID: {kitapId}");

            bool sonuc = await _oduncAlmaService.KitapOduncAl(user.Id, kitapId);
            if (!sonuc)
            {
                Console.WriteLine("DEBUG: Ödünç alma başarısız oldu!");
            }
            else
            {
                Console.WriteLine("DEBUG: Ödünç alma başarılı!");
            }

            return RedirectToAction("Index", "Kitaplar");
        }


        [HttpPost]
        public async Task<IActionResult> IadeEt(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var oduncAlma = await _context.OduncAlmalar
                .Include(o => o.Kitap)
                .FirstOrDefaultAsync(o => o.Id == id && o.ApplicationUserId == user.Id && o.IadeTarihi == null);

            if (oduncAlma == null)
            {
                return NotFound();
            }

            oduncAlma.IadeTarihi = DateTime.Now;
            oduncAlma.Kitap.KitapDurumu = Core.Enums.Durum.OduncAlabilir;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}

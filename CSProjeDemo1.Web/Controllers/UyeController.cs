using CSProjeDemo1.Core.Entities;
using CSProjeDemo1.Data.Contexts;
using CSProjeDemo1.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSProjeDemo1.Web.Controllers
{
    public class UyeController : Controller
    {
        private readonly IUyeService _uyeService;
        private readonly KutuphaneContext _context;

        public UyeController(IUyeService uyeService, KutuphaneContext context)
        {
            _uyeService = uyeService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var uyeler = await _uyeService.GetAllUyelerAsync();
            return View(uyeler);
        }

        public async Task<IActionResult> OduncAldigiKitaplar(int id)
        {
            var kitaplar = await _uyeService.GetOduncAldigiKitaplarAsync(id);
            return View(kitaplar);
        }
        // Yeni Üye Ekleme Sayfası (GET)
        public IActionResult Ekle()
        {
            return View();
        }

        // Yeni Üye Ekleme İşlemi (POST)
        [HttpPost]
        public async Task<IActionResult> Ekle(Uye uye)
        {
            if (ModelState.IsValid)
            {
                _context.Uyeler.Add(uye);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(uye);
        }
        // Üye Düzenleme Sayfasını Aç
        public async Task<IActionResult> Duzenle(int id)
        {
            var uye = await _context.Uyeler.FindAsync(id);
            if (uye == null)
            {
                return NotFound();
            }
            return View(uye);
        }

        // Üye Bilgilerini Güncelle
        [HttpPost]
        public async Task<IActionResult> Duzenle(Uye uye)
        {
            if (ModelState.IsValid)
            {
                _context.Update(uye);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uye);
        }

        // Üye Silme İşlemi
        public async Task<IActionResult> Sil(int id)
        {
            var uye = await _context.Uyeler.FindAsync(id);
            if (uye != null)
            {
                _context.Uyeler.Remove(uye);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

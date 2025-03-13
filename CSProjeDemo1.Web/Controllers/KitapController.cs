using CSProjeDemo1.Core.Entities;
using CSProjeDemo1.Data.Contexts;
using CSProjeDemo1.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSProjeDemo1.Web.Controllers
{
    public class KitapController : Controller
    {
        private readonly IKitapService _kitapService;
        private readonly KutuphaneContext _context;
        public KitapController(IKitapService kitapService, KutuphaneContext context)
        {
            _kitapService = kitapService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var kitaplar = await _kitapService.GetAllBooksAsync();
            return View(kitaplar);
        }

        public async Task<IActionResult> OduncAl(int id, int uyeId)
        {
            var result = await _kitapService.OduncAlAsync(id, uyeId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> IadeEt(int id)
        {
            var result = await _kitapService.IadeEtAsync(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
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
        public async Task<IActionResult> Edit(int id)
        {
            Console.WriteLine($"DEBUG: Edit Sayfası Açılıyor, Kitap ID: {id}");
            var kitap = await _kitapService.GetBookByIdAsync(id);

            if (kitap == null)
            {
                Console.WriteLine("DEBUG: Kitap Bulunamadı!");
                return NotFound();
            }

            Console.WriteLine($"DEBUG: Kitap Bulundu - {kitap.Baslik}");
            return View(kitap);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Kitap kitap)
        {
            if (id != kitap.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                // ModelState hatalarını loglamak
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"ModelState Hatası: {key} - {error.ErrorMessage}");
                    }
                }

                return View(kitap);
            }

            await _kitapService.UpdateKitapAsync(kitap);
            return RedirectToAction(nameof(Index));
        }





        // Kitap Silme İşlemi
        public async Task<IActionResult> Delete(int id)
        {
            var kitap = await _context.Kitaplar.FindAsync(id);
            if (kitap != null)
            {
                _context.Kitaplar.Remove(kitap);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

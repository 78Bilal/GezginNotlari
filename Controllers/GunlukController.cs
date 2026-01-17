using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeyahatGunlugu.Data;
using SeyahatGunlugu.Models;

namespace SeyahatGunlugu.Controllers
{
    public class GunlukController : Controller
    {
        private readonly GunlukContext _context;

        public GunlukController(GunlukContext context)
        {
            _context = context;
        }

        // 🔐 ADMIN KONTROL
        private bool AdminMi()
        {
            return HttpContext.Session.GetString("Admin") != null;
        }

        // 📸 RESİM YÜKLEME HELPER
        private async Task<string?> ResimYukle(IFormFile? resim)
        {
            if (resim == null || resim.Length == 0)
                return null;

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var dosyaAdi = Guid.NewGuid() + Path.GetExtension(resim.FileName);
            var dosyaYolu = Path.Combine(uploadPath, dosyaAdi);

            using (var stream = new FileStream(dosyaYolu, FileMode.Create))
            {
                await resim.CopyToAsync(stream);
            }

            return dosyaAdi;
        }

        // 🗑️ ESKİ RESİM SİLME HELPER
        private void EskiResmiSil(string? resimYolu)
        {
            if (string.IsNullOrEmpty(resimYolu))
                return;

            var dosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", resimYolu);
            if (System.IO.File.Exists(dosyaYolu))
                System.IO.File.Delete(dosyaYolu);
        }

        // 📄 LİSTE (HERKES GÖREBİLİR)
        public async Task<IActionResult> Index()
        {
            return View(await _context.Gunlukler.ToListAsync());
        }

        // 📄 DETAY (HERKES GÖREBİLİR)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var gunluk = await _context.Gunlukler.FirstOrDefaultAsync(m => m.Id == id);
            if (gunluk == null) return NotFound();

            return View(gunluk);
        }

        // ➕ CREATE (SADECE ADMIN)
        [HttpGet]
        public IActionResult Create()
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Admin");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GunlukModel gunluk, IFormFile Resim)
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Admin");

            if (ModelState.IsValid)
            {
                var yuklenenResim = await ResimYukle(Resim);
                if (yuklenenResim != null)
                {
                    gunluk.ResimYolu = yuklenenResim;
                }

                _context.Gunlukler.Add(gunluk);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(gunluk);
        }

        // ✏️ EDIT (SADECE ADMIN)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Admin");

            var gunluk = await _context.Gunlukler.FindAsync(id);
            if (gunluk == null) return NotFound();

            return View(gunluk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GunlukModel gunluk, IFormFile Resim)
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Admin");

            if (id != gunluk.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var mevcutGunluk = await _context.Gunlukler.FindAsync(id);
                if (mevcutGunluk == null) return NotFound();

                mevcutGunluk.Baslik = gunluk.Baslik;
                mevcutGunluk.Not = gunluk.Not;
                mevcutGunluk.Tarih = gunluk.Tarih;

                var yuklenenResim = await ResimYukle(Resim);
                if (yuklenenResim != null)
                {
                    EskiResmiSil(mevcutGunluk.ResimYolu);
                    mevcutGunluk.ResimYolu = yuklenenResim;
                }

                _context.Update(mevcutGunluk);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(gunluk);
        }

        // 🗑️ DELETE (SADECE ADMIN)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!AdminMi())
                return RedirectToAction("Login", "Admin");

            var gunluk = await _context.Gunlukler.FindAsync(id);
            if (gunluk != null)
            {
                EskiResmiSil(gunluk.ResimYolu);
                _context.Gunlukler.Remove(gunluk);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

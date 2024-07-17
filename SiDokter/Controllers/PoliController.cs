using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Services;
using System.Reflection;

namespace SiDokter.Controllers
{
    [Authorize]
    public class PoliController : Controller
    {
        private readonly ISiDokterService _siDokterService;

        public PoliController(ISiDokterService siDokterService)
        {
            _siDokterService = siDokterService;
        }
        public async Task<IActionResult> Index()
        {
            List<Poli> poli = await _siDokterService.getAllPoliAsync();
            return View(poli);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("id,nama,lokasi")] Poli poli)
        {
            var hasil = await _siDokterService.addPoliAsync(poli);
            if (hasil == "Poli berhasil ditambahkan.")
            {
                return RedirectToAction(nameof(Index));
            } 
            else
            {
                return View();
            }
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poli = await _siDokterService.getPoliAsync((int)id);
            
            if (poli == null)
            {
                return NotFound();
            }
            
            return View(poli);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,nama,lokasi")] Poli poli)
        {
            if (id != poli.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var hasil = await _siDokterService.updatePoliAsync(poli);
                return RedirectToAction(nameof(Index));
            }
            return View(poli);
        }

        public async Task<IActionResult> Detail(int id)
        {
            List<Dokter> dokters = await _siDokterService.getDoctorOnPoliAsync(id);

            var poli = await _siDokterService.getPoliAsync(id);

            // Create and populate the ViewModel
            var viewData= new PoliView
            {
                poli = poli,
                Dokters = dokters
            };

            return View(viewData);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dokter dokter = await _siDokterService.getDokterAsync((int)id);
            if (dokter == null)
            {
                return NotFound();
            }

            return View(dokter);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _siDokterService.removeDokterAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}

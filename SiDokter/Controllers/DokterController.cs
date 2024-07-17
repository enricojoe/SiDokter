using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Services;
using SiDokter.CustomAttribute;
using System.Reflection;

namespace SiDokter.Controllers
{
    [Authorize]
    public class DokterController : Controller
    {
        private readonly ISiDokterService _siDokterService;
        private readonly IOpenSearchService _openSearchService;

        public DokterController(ISiDokterService siDokterService, IOpenSearchService openSearchService)
        {
            _siDokterService = siDokterService;
            _openSearchService = openSearchService;
        }
        public async Task<IActionResult> Index([FromQuery] string query)
        {
            if (query != null)
            {
                var results = await _openSearchService.SearchAsync(query);
                return View(results);
            }
            List<Dokter> dokter = await _siDokterService.getAllDokterAsync();
            return View(dokter);
        }

        //public async Task<IActionResult> Search([FromQuery]string q)
        //{
        //    var results = await _openSearchService.SearchAsync(q);
        //    return View(results);
        //}

        public async Task<IActionResult> Create()
        {
            var jenis_kelamin = new Dictionary<int, string>
                {
                    { 1, "Laki-Laki" },
                    { 2, "Perempuan" }
                };

            ViewBag.jenis_kelamin = new SelectList(jenis_kelamin.Select(d => new SelectListItem
            {
                Value = d.Key.ToString(),
                Text = d.Value
            }).ToList(), "Value", "Text");

            ViewBag.spesialisasi = new SelectList(await _siDokterService.getSpesialisasiAsync(), "id", "nama");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Dokter dokter)
        {
            var hasil = await _siDokterService.addDokterAsync(dokter);
            if (hasil == "Dokter berhasil ditambahkan.")
            {
                await _openSearchService.IndexDocumentAsync(dokter);
                return RedirectToAction(nameof(Index));
            } 
            else
            {
                return View();
            }
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            var dokter = await _siDokterService.getDokterAsync(id);
            
            if (dokter == null)
            {
                return NotFound();
            }
            var jenis_kelamin = new Dictionary<int, string>
                {
                    { 1, "Laki-Laki" },
                    { 2, "Perempuan" }
                };

            ViewBag.jenis_kelamin = new SelectList(jenis_kelamin.Select(d => new SelectListItem
            {
                Value = d.Key.ToString(),
                Text = d.Value
            }).ToList(), "Value", "Text");
            ViewBag.spesialisasi = new SelectList(await _siDokterService.getSpesialisasiAsync(), "id", "nama");
            return View(dokter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,nama,tanggal_lahir,tempat_lahir,jenis_kelamin,SpesialisasiId")] Dokter dokter)
        {
            if (id != dokter.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var hasil = await _siDokterService.updateDokterAsync(dokter);
                await _openSearchService.UpdateDocumentAsync(id, dokter);
                return RedirectToAction(nameof(Index));
            }
            return View(dokter);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Dokter dokter = await _siDokterService.getDokterAsync(id);
            return View(dokter);
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
            await _siDokterService.removeDokterAsync(id);
            await _openSearchService.DeleteDocumentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

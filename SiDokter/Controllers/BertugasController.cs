using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Services;
using System.Dynamic;
using System.Reflection;

namespace SiDokter.Controllers
{
    public class BertugasController : Controller
    {
        private readonly ISiDokterService _siDokterService;

        public BertugasController(ISiDokterService siDokterService)
        {
            _siDokterService = siDokterService;
        }

        public async Task<IActionResult> Create([FromQuery]int id)
        {
            try
            {
                ViewBag.dokter = await _siDokterService.getDokterAsync(id);
                ViewBag.poli = new SelectList(await _siDokterService.getAllPoliAsync(), "id", "nama");
                var hari = new List<string>
                {
                    "Senin",
                    "Selasa",
                    "Rabu",
                    "Kamis",
                    "Jumat"
                };
                ViewBag.hari = new SelectList(hari.Select(d => new SelectListItem
                {
                    Value = d,
                    Text = d
                }).ToList(), "Value", "Text");

                List<BertugasPoli> bertugas = await _siDokterService.getBertugasOnPoliAsync(id);
                BertugasCreateView bertugas_view = new BertugasCreateView();
                bertugas_view.BertugasPoli = bertugas;

                return View(bertugas_view);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("id_dokter,id_poli,hari")] Bertugas bertugas)
        {
            try
            {
                var hasil = await _siDokterService.addBertugasAsync(bertugas);
                if (hasil == "Tugas berhasil ditambahkan.")
                {
                    return RedirectToAction("Index", "Dokter");
                } 
                else
                {
                    return View(bertugas);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return View();
            }
        } 
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dokter = await _siDokterService.getDokterAsync((int)id);
            
            if (dokter == null)
            {
                return NotFound();
            }
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
            var result = await _siDokterService.removeDokterAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}

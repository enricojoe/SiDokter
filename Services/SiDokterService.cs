using Entities;
using Microsoft.Extensions.Caching.Memory;
using Repositories;

namespace Services
{
    public class SiDokterService : ISiDokterService
    {
        private readonly SiDokterRepository _siDokterRepository;
        private readonly IMemoryCache _cache;

        public SiDokterService(SiDokterRepository siDokterRepository, IMemoryCache cache)
        {
            _siDokterRepository = siDokterRepository;
            _cache = cache;
        }

        public async Task<List<Dokter>> getAllDokterAsync()
        {
            if (_cache.TryGetValue("get-all-dokter", out List<Dokter> data))
            {
                return data.ToList();
            }
            List<Dokter> dokter = await _siDokterRepository.getAllDokterAsync();
            _cache.Set("get-all-dokter", dokter);
            return dokter.ToList();
        }

        public async Task<Dokter> getDokterAsync(int id)
        {
            return await _siDokterRepository.getDokterAsync(id);
        }

        public async Task<string> addDokterAsync(Dokter dokter)
        {
            _cache.Remove("get-all-dokter");
            string kode_tahun = (DateTime.Now.Year + 5).ToString();
            string kode_lahir = dokter.tanggal_lahir.ToString("ddMMyy");
            string kode_jenis_kelamin = dokter.jenis_kelamin.ToString();
            Random random = new Random();
            char randomChar1 = (char)random.Next('A', 'Z' + 1);
            char randomChar2 = (char)random.Next('A', 'Z' + 1);
            string kode_random = (randomChar1.ToString() + randomChar2.ToString()).ToString();
            dokter.NIP = kode_tahun + kode_lahir + kode_jenis_kelamin + kode_random;
            return await _siDokterRepository.addDokterAsync(dokter);
        }

        public async Task<string> updateDokterAsync(Dokter dokter)
        {
            _cache.Remove("get-all-dokter");
            return await _siDokterRepository.updateDokterAsync(dokter);
        }

        public async Task<string> removeDokterAsync(int id)
        {
            _cache.Remove("get-all-dokter");
            return await _siDokterRepository.removeDokterAsync(id);
        }

        public async Task<List<Spesialisasi>> getSpesialisasiAsync()
        {
            return await _siDokterRepository.getSpesialisasiAsync();
        }

        public async Task<List<Poli>> getAllPoliAsync()
        {
            if (_cache.TryGetValue("get-all-poli", out List<Poli> data))
            {
                return data.ToList();
            }
            List<Poli> poli = await _siDokterRepository.getAllPoliAsync();
            _cache.Set("get-all-poli", poli);
            return poli.ToList();
        }
        public async Task<Poli> getPoliAsync(int id)
        {
            return await _siDokterRepository.getPoliAsync(id);
        }
        public async Task<List<Dokter>> getDoctorOnPoliAsync(int idPoli)
        {
            return await _siDokterRepository.getDoctorOnPoliAsync(idPoli);
        }
        public async Task<List<BertugasPoli>> getBertugasOnPoliAsync(int id_dokter)
        {
            return await _siDokterRepository.getBertugasOnPoliAsync(id_dokter);
        }
        public async Task<string> addPoliAsync(Poli poli)
        {
            _cache.Remove("get-all-poli");
            return await _siDokterRepository.addPoliAsync(poli);
        }
        public async Task<string> updatePoliAsync(Poli poli)
        {
            _cache.Remove("get-all-poli");
            return await _siDokterRepository.updatePoliAsync(poli);
        }
        public async Task<string> addBertugasAsync(Bertugas bertugas)
        {
            return await _siDokterRepository.addBertugasAsync(bertugas);
        }
    }
}

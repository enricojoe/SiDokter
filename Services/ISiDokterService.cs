using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ISiDokterService
    {
        public Task<List<Dokter>> getAllDokterAsync();
        public Task<Dokter> getDokterAsync(int id);
        public Task<string> addDokterAsync(Dokter dokter);
        public Task<string> updateDokterAsync(Dokter dokter);
        public Task<string> removeDokterAsync(int id);

        public Task<List<Spesialisasi>> getSpesialisasiAsync();
        public Task<List<Poli>> getAllPoliAsync();
        public Task<Poli> getPoliAsync(int id);
        public Task<List<Dokter>> getDoctorOnPoliAsync(int idPoli);
        public Task<List<BertugasPoli>> getBertugasOnPoliAsync(int id_dokter);
        public Task<string> addPoliAsync(Poli poli);
        public Task<string> updatePoliAsync(Poli poli);
        public Task<string> addBertugasAsync(Bertugas bertugas);

    }
}

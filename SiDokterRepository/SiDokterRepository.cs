using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Repositories;
using System.Data;
using Dapper;
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Repositories
{
    public class SiDokterRepository : ISiDokterRepository
    {
        private readonly IConfiguration _configuration;

        public SiDokterRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            }
        }
        public async Task<List<Dokter>> getAllDokterAsync()
        {
            using IDbConnection dbConnection = Connection;
            string sql = @"
                    SELECT d.*, s.nama AS Spesialisasi
                    FROM [dokter] d
                    INNER JOIN [spesialisasi] s ON d.SpesialisasiId = s.id";

            var result = await dbConnection.QueryAsync<Dokter>(sql);
            return result.ToList();
        }
        public async Task<Dokter> getDokterAsync(int id)
        {
            using IDbConnection dbConnection = Connection;
            var result = await dbConnection.QueryFirstOrDefaultAsync<Dokter>("SELECT * FROM [dokter] WHERE id = @id", new { id });
            return result;
        }
        public async Task<string> addDokterAsync(Dokter dokter)
        {
            using IDbConnection dbConnection = Connection;
            
            var query = "INSERT INTO [dokter] (NIK, NIP, nama, tanggal_lahir, tempat_lahir, jenis_kelamin, SpesialisasiId) VALUES(@NIK, @NIP, @nama, @tanggal_lahir, @tempat_lahir, @jenis_kelamin, @SpesialisasiId)";
            var result = await dbConnection.ExecuteAsync(query, dokter);
            if (result > 0)
            {
                return "Dokter berhasil ditambahkan.";
            }
            else
            {
                return "Gagal menambahkan data dokter.";
            }
        }
        public async Task<string> updateDokterAsync(Dokter dokter)
        {
            using IDbConnection dbConnection = Connection;
            var query = "UPDATE [dokter] SET nama = @nama, tanggal_lahir = @tanggal_lahir , tempat_lahir = @tempat_lahir, jenis_kelamin = @jenis_kelamin, SpesialisasiId = @SpesialisasiId WHERE id = @id";
            var result = await dbConnection.ExecuteAsync(query, dokter);
            if (result > 0)
            {
                return "Data dokter berhasil diupdate.";
            }
            else
            {
                return "Gagal mengupdate data dokter.";
            }
        }
        public async Task<string> removeDokterAsync(int id)
        {
            using IDbConnection dbConnection = Connection;
            var query = "DELETE FROM [dokter] WHERE id = @id";
            var result = await dbConnection.ExecuteAsync(query, new { id });
            if (result > 0)
            {
                return "Dokter berhasil dihapus.";
            }
            else
            {
                return "Gagal menghapus data dokter.";
            }
        }

        public async Task<List<Spesialisasi>> getSpesialisasiAsync()
        {
            using IDbConnection dbConnection = Connection;
            var result = await dbConnection.QueryAsync<Spesialisasi>("SELECT * FROM [spesialisasi]");
            return result.ToList();
        }

        public async Task<List<Poli>> getAllPoliAsync()
        {
            using IDbConnection dbConnection = Connection;
            var result = await dbConnection.QueryAsync<Poli>("SELECT * FROM [poli]");
            return result.ToList();
        }

        public async Task<Poli> getPoliAsync(int id)
        {
            using IDbConnection dbConnection = Connection;
            var result = await dbConnection.QueryFirstOrDefaultAsync<Poli>("SELECT * FROM [poli] WHERE id = @id", new { id });
            return result;
        }

        public async Task<string> updatePoliAsync(Poli poli)
        {
            using IDbConnection dbConnection = Connection;
            var query = "UPDATE [poli] SET nama = @nama, lokasi = @lokasi WHERE id = @id";
            var result = await dbConnection.ExecuteAsync(query, poli);
            if (result > 0)
            {
                return "Data poli berhasil diupdate.";
            }
            else
            {
                return "Gagal mengupdate data poli.";
            }
        }

        public async Task<List<Dokter>> getDoctorOnPoliAsync(int idPoli)
        {
            using IDbConnection dbConnection = Connection;
            string sql = @"
                    SELECT d.*, s.nama as Spesialisasi
                    FROM [dokter] d
                    INNER JOIN [bertugas] b ON d.id = b.id_dokter
                    INNER JOIN [spesialisasi] s ON d.SpesialisasiId = s.id
                    WHERE b.id_poli = @IdPoli";

            var dokters = await dbConnection.QueryAsync<Dokter>(sql, new { IdPoli = idPoli });

            return dokters.ToList();
        }

        public async Task<List<BertugasPoli>> getBertugasOnPoliAsync(int id_dokter)
        {
            using IDbConnection dbConnection = Connection;
            string sql = @"
                    SELECT p.nama AS nama_poli, b.hari
                    FROM [bertugas] b
                    INNER JOIN [poli] p ON b.id_poli = p.id
                    WHERE b.id_dokter = @IdDokter";

            var bertugas = await dbConnection.QueryAsync<BertugasPoli>(sql, new { IdDokter =  id_dokter });

            return bertugas.ToList();
        }

        public async Task<string> addPoliAsync(Poli poli)
        {
            using IDbConnection dbConnection = Connection;
            var query = "INSERT INTO [poli] (nama, lokasi) VALUES(@nama, @lokasi)";
            var result = await dbConnection.ExecuteAsync(query, poli);
            if (result > 0)
            {
                return "Poli berhasil ditambahkan.";
            }
            else
            {
                return "Gagal menambahkan data poli.";
            }
        }

        public async Task<string> addBertugasAsync(Bertugas bertugas)
        {
            using IDbConnection dbConnection = Connection;
            var query = "INSERT INTO [bertugas] (id_dokter, id_poli, hari) VALUES(@id_dokter, @id_poli, @hari)";
            var result = await dbConnection.ExecuteAsync(query, bertugas);
            if (result > 0)
            {
                return "Tugas berhasil ditambahkan.";
            }
            else
            {
                return "Gagal menambahkan data bertugas.";
            }
        }
    }
}

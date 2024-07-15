using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Entities
{
    public class Dokter
    {
        public int id { get; set; }
        public string? NIK { get; set; }
        public string? NIP { get; set; }
        [DisplayName("Nama")]
        public string nama { get; set; }
        [DisplayName("Tanggal Lahir")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime tanggal_lahir { get; set; }
        [DisplayName("Tempat Lahir")]
        public string tempat_lahir { get; set; }
        [DisplayName("Jenis Kelamin")]
        public int jenis_kelamin { get; set; }
        public int SpesialisasiId { get; set; }
        public string? Spesialisasi { get; set; }
    }
}

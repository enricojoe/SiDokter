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
    public class Bertugas
    {
        public int id { get; set; }
        [DisplayName("Dokter")]
        public int id_dokter { get; set; }
        [DisplayName("Poli")]
        public int id_poli { get; set; }
        public string hari { get; set; }
    }
}

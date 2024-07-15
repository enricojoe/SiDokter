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
    public class Poli
    {
        public int id { get; set; }
        [DisplayName("Nama")]
        public string nama { get; set; }
        [DisplayName("Lokasi")]
        public string lokasi { get; set; }
    }
}

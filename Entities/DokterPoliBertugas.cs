using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class DokterPoliBertugas
    {
        public Dokter dokter {  get; set; }
        public List<Poli> polis { get; set; }
        public Bertugas bertugas { get; set; }
    }
}

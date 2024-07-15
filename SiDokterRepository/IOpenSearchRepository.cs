using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IOpenSearchRepository
    {
        public Task IndexDataAsync(IEnumerable<Dokter> data);
        public Task<IEnumerable<Dokter>> SearchAsync(string query);
        public Task IndexDocumentAsync(Dokter data);
    }
}

using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IOpenSearchService
    {
        public Task IndexDataAsync(IEnumerable<Dokter> data);
        public Task<IEnumerable<Dokter>> SearchAsync(string query);
        public Task IndexDocumentAsync(Dokter data);
        public Task UpdateDocumentAsync(int id, Dokter data);
        public Task DeleteDocumentAsync(int id);
    }
}

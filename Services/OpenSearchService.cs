using Entities;
using OpenSearch.Client;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OpenSearchService : IOpenSearchService
    {
        private readonly IOpenSearchRepository _openSearchRepository;

        public OpenSearchService(IOpenSearchRepository openSearchRepository)
        {
            _openSearchRepository = openSearchRepository;
        }

        public async Task IndexDataAsync(IEnumerable<Dokter> data)
        {
            await _openSearchRepository.IndexDataAsync(data);
        }

        public async Task<IEnumerable<Dokter>> SearchAsync(string query)
        {
            return await _openSearchRepository.SearchAsync(query);
        }

        public async Task IndexDocumentAsync(Dokter data)
        {
            await _openSearchRepository.IndexDocumentAsync(data);
        }

        public async Task UpdateDocumentAsync(int id, Dokter data)
        {
            await _openSearchRepository.UpdateDocumentAsync(id, data);
        }

        public async Task DeleteDocumentAsync(int id)
        {
            await _openSearchRepository.DeleteDocumentAsync(id);
        }
    }
}

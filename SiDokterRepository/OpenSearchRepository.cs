using Entities;
using Nest;
using OpenSearch.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class OpenSearchRepository : IOpenSearchRepository
    {
        private readonly IOpenSearchClient _openSearchClient;

        public OpenSearchRepository(IOpenSearchClient openSearchClient)
        {
            _openSearchClient = openSearchClient;
        }
        public async Task IndexDataAsync(IEnumerable<Dokter> data)
        {
            var bulkIndexResponse = await _openSearchClient.BulkAsync(b => b
                .Index("dokterku")
                .IndexMany(data));

            if (bulkIndexResponse.Errors)
            {
                Console.WriteLine(bulkIndexResponse.Errors);
            }
        }
        public async Task<IEnumerable<Dokter>> SearchAsync(string query)
        {
            var searchResponse = await _openSearchClient.SearchAsync<Dokter>(s => s
                .Query(q => q
                    .MultiMatch(m => m
                        .Fields(f => f
                            .Field(p => p.NIK)
                            .Field(p => p.NIP))
                        .Query(query))));

            return searchResponse.Documents;
        }
        public async Task IndexDocumentAsync(Dokter data)
        {
            await _openSearchClient.IndexDocumentAsync(data);
        }

        public async Task UpdateDocumentAsync(int id, Dokter data)
        {
            var res = await _openSearchClient.UpdateAsync<Dokter>(id, u => u
                            .Doc(data)
                            .Index("dokter"));
            return;
        }

        public async Task DeleteDocumentAsync(int id)
        {
            await _openSearchClient.DeleteAsync<Dokter>(id, d => d
            .Index("dokter"));
        }
    }
}

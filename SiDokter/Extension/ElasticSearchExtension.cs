using Entities;
using Nest;

namespace SiDokter.Extension
{
    public static class ElasticSearchExtension
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration["Elasticsearch:Url"];
            var index = configuration["Elasticsearch:Index"];
            var settings = new ConnectionSettings(new Uri(baseUrl ?? ""))
                .PrettyJson()
                .CertificateFingerprint("f9c4e70e3120c981b94c24237fd20195984c18e92b7a06a43c9353919dc2f93c")
                .BasicAuthentication("elastic", "lZY8Ol7cf*x3Oa8k-5VT")
                .DefaultIndex(index);
            settings.EnableApiVersioningHeader();
            //AddDefaultMappings(settings);
            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);
            CreateIndex(client, index);
        }
        //private static void AddDefaultMappings(ConnectionSettings settings)
        //{
        //    settings.DefaultMappingFor<Dokter>(m => { m.AutoMap() });
        //}
        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName, index => index.Map<Dokter>(x => x.AutoMap()));
        }
    }
}

//Buat elastic search in the app using NoSQL

using Entities;
using OpenSearch.Client;
using OpenSearch.Net;
using Repositories;
using Services;
using System;

namespace SiDokter
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            var url = builder.Configuration["Opensearch:Url"];
            var index = builder.Configuration["Opensearch:Index"];

            var node = new Uri(url);
            var config = new ConnectionSettings(node)
                .BasicAuthentication("admin", "ThisQwerty@21")
                .ServerCertificateValidationCallback(CertificateValidations.AllowAll)
                .DefaultIndex(index);
            var client = new OpenSearchClient(config);

            builder.Services.AddSingleton<IOpenSearchClient>(client);
            client.Indices.Create(index, index => index.Map<Dokter>(x => x.AutoMap()));
            builder.Services.AddScoped<IOpenSearchRepository, OpenSearchRepository>();
            builder.Services.AddScoped<IOpenSearchService, OpenSearchService>();


            //var settings = new ConnectionSettings(new Uri(url))
            //    .CertificateFingerprint("f9c4e70e3120c981b94c24237fd20195984c18e92b7a06a43c9353919dc2f93c")
            //    .BasicAuthentication("elastic", "lZY8Ol7cf*x3Oa8k-5VT")
            //    .DefaultIndex(index);
            //settings.EnableApiVersioningHeader();

            //var client = new ElasticClient(settings);
            //builder.Services.AddSingleton<IElasticClient>(client);
            //client.Indices.Create(index, index => index.Map<Dokter>(x => x.AutoMap()));
            //builder.Services.AddSingleton<IElasticsearchService, ElasticsearchService>();

            // Add services to the container.
            builder.Services.AddMemoryCache();
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ISiDokterService, SiDokterService>();
            builder.Services.AddTransient<SiDokterRepository>();

            var app = builder.Build();

            PopulateInitialData(app).GetAwaiter().GetResult();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Dokter}/{action=Index}/{id?}");

            app.Run();
        }
        private static async Task PopulateInitialData(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var openSearchService = services.GetRequiredService<IOpenSearchService>();
                var siDokterService = services.GetRequiredService<ISiDokterService>();

                var data = await siDokterService.getAllDokterAsync();
                await openSearchService.IndexDataAsync(data);
            }
        }
    }
}


//var settings = new ConnectionSettings(new Uri(url))
//                .CertificateFingerprint("f9c4e70e3120c981b94c24237fd20195984c18e92b7a06a43c9353919dc2f93c")
//                .BasicAuthentication("elastic", "lZY8Ol7cf*x3Oa8k-5VT")
//                .DefaultIndex(index);
//settings.EnableApiVersioningHeader();

//var client = new ElasticClient(settings);
//builder.Services.AddSingleton<IElasticClient>(client);
//client.Indices.Create(index, index => index.Map<Dokter>(x => x.AutoMap()));
//builder.Services.AddSingleton<IElasticsearchService, ElasticsearchService>();

//PopulateInitialData(app).GetAwaiter().GetResult();

//private static async Task PopulateInitialData(WebApplication app)
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var services = scope.ServiceProvider;
//        var elasticsearchService = services.GetRequiredService<IElasticsearchService>();
//        var siDokterService = services.GetRequiredService<ISiDokterService>();

//        var data = await siDokterService.getAllDokterAsync();
//        await elasticsearchService.IndexDataAsync(data);
//    }
//}
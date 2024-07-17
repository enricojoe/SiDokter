//Buat elastic search in the app using NoSQL
//(localdb)\MSSQLLocalDB
using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using OpenSearch.Client;
using OpenSearch.Net;
using Repositories;
using Services;
using SiDokter.CustomAttribute;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SiDokter
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            var key = Encoding.ASCII.GetBytes(builder.Configuration["AppSettings:Secret"]);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true, // Validate who created the token (issuer)
                            ValidateAudience = true, // Validate who receives the token (audience)
                            ValidateLifetime = true, // Validate token expiration
                            ValidateIssuerSigningKey = true, // Validate the token signature using a secret key
                            IssuerSigningKey = new SymmetricSecurityKey(key), // Replace with your secret key retrieval logic
                            ValidIssuer = "AuthApp", // Replace with your issuer string
                            ValidAudience = "SiDokterApp"  // Replace with your audience string
                        };
                    });
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
            });

            var openSearchHosts = Environment.GetEnvironmentVariable("OPENSEARCH_HOSTS")?
                .Split(',')
                .Select(uri => new Uri(uri))
                .ToArray();

            // If the environment variable is not set, fallback to the configuration file
            if (openSearchHosts == null || openSearchHosts.Length == 0)
            {
                var url = builder.Configuration["Opensearch:Url"];
                openSearchHosts = new[] { new Uri(url) };
            }

            var index = builder.Configuration["Opensearch:Index"];
            var connectionPool = new StaticConnectionPool(openSearchHosts);
            //var url = builder.Configuration["Opensearch:Url"];
            //var index = builder.Configuration["Opensearch:Index"];

            //var node = new Uri(url);
            var config = new ConnectionSettings(connectionPool)
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
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new CookiesAuthAttribute());
            });
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ISiDokterService, SiDokterService>();
            builder.Services.AddTransient<SiDokterRepository>();
            builder.Services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(@"/var/data-protection-keys/"))
                    .SetApplicationName("SiDokter");

            // builder.WebHost.ConfigureKestrel(serverOptions =>
            // {
            //     serverOptions.ConfigureHttpsDefaults(listenOptions =>
            //     {
            //         listenOptions.ServerCertificate = new X509Certificate2("/etc/ssl/certs/certificate.crt", "/etc/ssl/private/private.key");
            //     });
            // });

            var app = builder.Build();

            PopulateInitialData(app).GetAwaiter().GetResult();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
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

//,
//  "Kestrel": {
//    "Endpoints": {
//        "Http": {
//            "Url": "http://*:8000"
//        }
//    }
//}

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
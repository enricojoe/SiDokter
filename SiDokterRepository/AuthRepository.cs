using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity.Data;
using Newtonsoft.Json.Linq;

namespace Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _configuration;
        public AuthRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            }
        }
        public async Task<string> AuthenticateUserAsync(User user)
        {
            using var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            Console.WriteLine("**************");
            Console.WriteLine(content);
            Console.WriteLine("**************");
            using var response = await httpClient.PostAsync($"http://sidokterdocker-sidokterauthapp-1:8010/auth/User/Login", content);
            Console.WriteLine("==============");
            Console.WriteLine(response);
            Console.WriteLine("==============");
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return token;
            }
            else
            {
                return "error";
            }
        }
        public async Task<string> AddUserAsync(User user)
        {
            try
            {
                using var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var response = await httpClient.PostAsync($"http://sidokterdocker-sidokterauthapp-1:8010/auth/User/Register", content);
                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsStringAsync();
                    return res;
                }
                else
                {
                    return "error";
                }
            }
            catch (HttpRequestException ex)
            {
                // Log detailed exception information
                Console.WriteLine("++++++++++++++++++");
                Console.WriteLine(ex.ToString());
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.ToString());
                }
                return "";
            }
        }
    }
}
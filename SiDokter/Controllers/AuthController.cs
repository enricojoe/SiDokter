using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace SiDokter.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        // GET: AuthController
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        // GET: AuthController/Details/5
        public ActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> VerifyUser(User user)
        {
            var token = await _authService.AuthenticateUserAsync(user);
            if (token != null)
            {
                SetCookie(token);
                return RedirectToAction("Index", "Dokter");
            }
            //if berhasil -> dokter index
            //else ke login lagi
            return View(user);
        }

        // GET: AuthController/Create
        public IActionResult LogOut()
        {
            // Name of the cookie to remove
            string cookieName = "token";

            // Remove the cookie
            Response.Cookies.Delete(cookieName);

            return RedirectToAction("Login");
        }
        private string SetCookie(string token)
        {
            HttpContext.Response.Cookies.Append("token", token);
            return "Cookie has been set.";
        }
    }
}

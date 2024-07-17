using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace SiDokter.CustomAttribute
{
    public class CookiesAuthAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                return;
            }
            var httpContext = context.HttpContext;
            var cookie = httpContext.Request.Cookies["token"];

            if (string.IsNullOrEmpty(cookie))
            {
                //context.Result = new UnauthorizedResult();
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            var token = new AuthenticationHeaderValue("Bearer", cookie);

            if (!ValidateToken(token.Parameter, out ClaimsPrincipal principal))
            {
                //context.Result = new UnauthorizedResult();
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            httpContext.User = principal;
        }

        private bool ValidateToken(string token, out ClaimsPrincipal principal)
        {
            principal = null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("INI_RAHASIA_LHOOOOOO_TAMBAHIN_PANGJANG_LAGI_TUUUUUUUU"); // Replace with your secret key
            var issuer = "AuthApp"; // Replace with your issuer
            var audience = "SiDokterApp"; // Replace with your audience

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience // Optional: to set clock skew to zero, adjust as necessary
                };

                principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

    }

}

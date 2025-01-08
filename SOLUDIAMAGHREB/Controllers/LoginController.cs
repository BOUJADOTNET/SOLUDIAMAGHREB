using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SOLUDIAMAGHERB.Resources;
using SOLUDIAMAGHERB.Services.Contract;
using SOLUDIAMAGHREB.Models;
using System.Security.Claims;

namespace SOLUDIAMAGHERB.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        public LoginController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(Utilisateur model)
        {
            model.Clave = Utilities.EncriptarClave(model.Clave);

            Utilisateur Utilisateur_created = await _userService.SaveUser(model);

            if (Utilisateur_created.IdUser > 0)
                return RedirectToAction("LoginSession", "Login");

            ViewData["Message"] = "Failed to create user";
            return View();
        }

        public IActionResult LoginSession()
        {
            // Redirect to Home if the user is already authenticated
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginSessionAsync(string username, string clave)
        {
            Utilisateur user_found = await _userService.GetUser(username, Utilities.EncriptarClave(clave));

            if (user_found == null)
            {
                ViewData["Message"] = "No matches found";
                return View();
            }

            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, user_found.UserName)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Index", "Home");
        }
    }

}

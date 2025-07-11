using Microsoft.AspNetCore.Mvc;
using PersonaKey.BusinessLayer.Abstract;
using PersonaKey.CoreLayer.Services;
using PersonaKey.WebUI.Models;

namespace PersonaKey.WebUI.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly TokenService _tokenService;

        public AuthenticationController(IAppUserService appUserService, TokenService tokenService)
        {
            _appUserService = appUserService;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _appUserService.GetByUsernameAsync(model.UserName);

            if (user == null || user.Password != model.Password)
            {
                ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
                return View(model);
            }

            var token = _tokenService.GenerateToken(user.UserName, user.Role?.Name);

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(1)
            });

            // Kullanıcı login olduktan sonra admin dashboard’a yönlendirme
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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

            // Generate token including role and claims (e.g. permissions)
            var token = _tokenService.GenerateToken(user.UserName, user.Role?.Name);

            // Set JWT token in secure HttpOnly cookie
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,           // Prevent JavaScript access
                Secure = true,             // Send only over HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(1)
            });

            // Redirect to Admin Dashboard after successful login
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    }
}

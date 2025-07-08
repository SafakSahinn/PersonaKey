using Microsoft.AspNetCore.Mvc;
using PersonaKey.CoreLayer.Services;

namespace PersonaKey.WebUI.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly TokenService _tokenService;

        public AuthenticationController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string role)
        {
            var token = _tokenService.GenerateToken(email, role);
            ViewBag.Token = token;
            return View();
        }
    }
}

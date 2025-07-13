using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            // Return the login view
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Check if model state is valid
            if (!ModelState.IsValid)
                return View(model);

            // Retrieve user from database using username
            var user = await _appUserService.GetByUsernameAsync(model.UserName);

            // Validate user existence and password
            if (user == null || user.Password != model.Password)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            // Check if the user's role allows login
            if (user.Role?.RoleAccess?.CanLogin != true)
            {
                ModelState.AddModelError(string.Empty, "User does not have login permission.");
                return View(model);
            }

            // Generate JWT token
            var token = _tokenService.GenerateToken(user);

            // Set the JWT as a secure, HttpOnly session cookie (deleted when browser is closed)
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,                // Prevent access by JavaScript
                Secure = true,                  // Send only over HTTPS
                SameSite = SameSiteMode.Strict  // Prevent cross-site usage
                // No Expires => session cookie (deleted on browser close)
            });

            // Redirect to the Admin dashboard upon successful login
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
    }
}

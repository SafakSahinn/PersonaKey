// AuthenticationController.cs
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

        [HttpGet]
        public IActionResult Login()
        {
            // Return the login view
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Validate the model state
            if (!ModelState.IsValid)
                return View(model);

            // Retrieve user by username
            var user = await _appUserService.GetByUsernameAsync(model.UserName);

            // Check if user exists and password is correct
            if (user == null || user.Password != model.Password)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            // Check if user has login permission (CanLogin claim)
            if (user.Role?.RoleAccess?.CanLogin != true)
            {
                ModelState.AddModelError(string.Empty, "User does not have login permission.");
                return View(model);
            }

            // Generate JWT token with user claims
            var token = _tokenService.GenerateToken(user);

            // Set JWT token as a secure HttpOnly cookie
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,       // Prevent JavaScript access to the cookie
                Secure = true,         // Send cookie only over HTTPS
                SameSite = SameSiteMode.Strict,  // Strict SameSite policy
                Expires = DateTime.UtcNow.AddDays(1)  // Cookie expiration
            });

            // Redirect user to Admin Dashboard upon successful login
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
    }
}

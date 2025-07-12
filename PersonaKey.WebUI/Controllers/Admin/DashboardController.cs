using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PersonaKey.WebUI.Controllers.Admin
{
    public class DashboardController : Controller
    {
        [Authorize(Policy = "OnlyLoggedInUsers")] // This restricts access to users who satisfy the policy
        public IActionResult Index()
        {
            return View();
        }
    }
}

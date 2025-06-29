using Microsoft.AspNetCore.Mvc;
using PersonaKey.BusinessLayer.Abstract;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.WebUI.Controllers
{
    public class PermissionController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly IRoleService _roleService;
        private readonly IDoorService _doorService;

        public PermissionController(IPermissionService permissionService, IRoleService roleService, IDoorService doorService)
        {
            _permissionService = permissionService;
            _roleService = roleService;
            _doorService = doorService;
        }

        public async Task<IActionResult> Index()
        {
            var permissions = await _permissionService.GetAllAsync();
            return View(permissions);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await _roleService.GetAllAsync();
            ViewBag.Doors = await _doorService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Permission permission)
        {
            if (ModelState.IsValid)
            {
                await _permissionService.AddAsync(permission);
                return RedirectToAction(nameof(Index));
            }

            // Eğer validasyon başarısızsa dropdown verilerini tekrar gönder
            ViewBag.Roles = await _roleService.GetAllAsync();
            ViewBag.Doors = await _doorService.GetAllAsync();
            return View(permission);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var roles = await _roleService.GetAllAsync();
            var doors = await _doorService.GetAllAsync();

            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            ViewBag.Doors = new SelectList(doors, "Id", "Name");

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

            // If validation fails, refill comboboxes
            var roles = await _roleService.GetAllAsync();
            var doors = await _doorService.GetAllAsync();

            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            ViewBag.Doors = new SelectList(doors, "Id", "Name");

            return View(permission);
        }
    }
}

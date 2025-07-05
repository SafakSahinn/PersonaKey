using Microsoft.AspNetCore.Mvc;
using PersonaKey.BusinessLayer.Abstract;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.WebUI.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _departmentService.GetAllAsync();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentService.AddAsync(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null)
                return NotFound();

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentService.UpdateAsync(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
    }
}

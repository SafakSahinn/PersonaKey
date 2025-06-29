using Microsoft.AspNetCore.Mvc;
using PersonaKey.BusinessLayer.Abstract;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.WebUI.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;
        private readonly IDepartmentService _departmentService;
        private readonly IRoleService _roleService;

        public PersonController(IPersonService personService, IDepartmentService departmentService, IRoleService roleService)
        {
            _personService = personService;
            _departmentService = departmentService;
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            var persons = await _personService.GetAllAsync();
            return View(persons);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _departmentService.GetAllAsync();
            ViewBag.Roles = await _roleService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Person person)
        {
            if (ModelState.IsValid)
            {
                await _personService.AddAsync(person);
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, we resend the dropdown data.
            ViewBag.Departments = await _departmentService.GetAllAsync();
            ViewBag.Roles = await _roleService.GetAllAsync();
            return View(person);
        }
    }
}

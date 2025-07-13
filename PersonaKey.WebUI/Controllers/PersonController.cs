using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonaKey.BusinessLayer.Abstract;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.WebUI.Controllers
{
    [Authorize(Policy = "OnlyLoggedInUsers")]
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
            var departments = await _departmentService.GetAllAsync();
            var roles = await _roleService.GetAllAsync();

            ViewBag.Departments = departments;
            ViewBag.Roles = roles;

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

            ViewBag.Departments = await _departmentService.GetAllAsync();
            ViewBag.Roles = await _roleService.GetAllAsync();
            return View(person);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var person = await _personService.GetByIdAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            ViewBag.Departments = await _departmentService.GetAllAsync();
            ViewBag.Roles = await _roleService.GetAllAsync();
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Person person)
        {
            if (ModelState.IsValid)
            {
                await _personService.UpdateAsync(person);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = await _departmentService.GetAllAsync();
            ViewBag.Roles = await _roleService.GetAllAsync();
            return View(person);
        }
    }
}

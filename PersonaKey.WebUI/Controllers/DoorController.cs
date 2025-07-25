﻿using Microsoft.AspNetCore.Mvc;
using PersonaKey.BusinessLayer.Abstract;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.WebUI.Controllers
{
    public class DoorController : Controller
    {
        private readonly IDoorService _doorService;

        public DoorController(IDoorService doorService)
        {
            _doorService = doorService;
        }

        // INDEX
        public async Task<IActionResult> Index()
        {
            var doors = await _doorService.GetAllAsync();
            return View(doors);
        }

        // CREATE - GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Door door)
        {
            if (ModelState.IsValid)
            {
                await _doorService.AddAsync(door);
                return RedirectToAction(nameof(Index));
            }

            return View(door);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var door = await _doorService.GetByIdAsync(id);
            if (door == null)
            {
                return NotFound();
            }
            return View(door);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Door door)
        {
            if (ModelState.IsValid)
            {
                await _doorService.UpdateAsync(door);
                return RedirectToAction(nameof(Index));
            }
            return View(door);
        }

    }
}

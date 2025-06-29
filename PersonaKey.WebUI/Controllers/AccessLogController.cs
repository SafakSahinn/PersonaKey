using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonaKey.BusinessLayer.Abstract;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.WebUI.Controllers
{
    public class AccessLogController : Controller
    {
        private readonly IAccessLogService _accessLogService;
        private readonly ICardService _cardService;
        private readonly IDoorService _doorService;

        public AccessLogController(IAccessLogService accessLogService, ICardService cardService, IDoorService doorService)
        {
            _accessLogService = accessLogService;
            _cardService = cardService;
            _doorService = doorService;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await _accessLogService.GetAllAsync();
            var cards = await _cardService.GetAllAsync();
            var doors = await _doorService.GetAllAsync();

            ViewBag.Cards = cards;
            ViewBag.Doors = doors;

            return View(logs);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var cards = await _cardService.GetAllAsync();
            var doors = await _doorService.GetAllAsync();

            ViewBag.Cards = new SelectList(cards, "Id", "CardNumber");
            ViewBag.Doors = new SelectList(doors, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccessLog log)
        {
            if (ModelState.IsValid)
            {
                await _accessLogService.AddAsync(log);
                return RedirectToAction(nameof(Index));
            }

            var cards = await _cardService.GetAllAsync();
            var doors = await _doorService.GetAllAsync();

            ViewBag.Cards = new SelectList(cards, "Id", "CardNumber", log.CardId);
            ViewBag.Doors = new SelectList(doors, "Id", "Name", log.DoorId);

            return View(log);
        }
    }
}

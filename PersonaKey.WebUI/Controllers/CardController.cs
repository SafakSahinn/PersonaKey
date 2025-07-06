using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonaKey.BusinessLayer.Abstract;
using PersonaKey.EntityLayer.Concrete;
using System.Threading.Tasks;

namespace PersonaKey.WebUI.Controllers
{
    public class CardController : Controller
    {
        private readonly ICardService _cardService;
        private readonly IPersonService _personService;

        public CardController(ICardService cardService, IPersonService personService)
        {
            _cardService = cardService;
            _personService = personService;
        }

        public async Task<IActionResult> Index()
        {
            var cards = await _cardService.GetAllAsync();
            var persons = await _personService.GetAllAsync();

            ViewBag.Persons = persons;
            return View(cards);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var persons = await _personService.GetAllAsync();
            ViewBag.Persons = persons;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Card card)
        {
            if (ModelState.IsValid)
            {
                await _cardService.AddAsync(card);
                return RedirectToAction(nameof(Index));
            }

            var persons = await _personService.GetAllAsync();
            ViewBag.Persons = new SelectList(persons, "Id", "FullName", card.PersonId);
            return View(card);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var card = await _cardService.GetByIdAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            var persons = await _personService.GetAllAsync();
            ViewBag.Persons = new SelectList(persons, "Id", "FullName", card.PersonId);

            return View(card);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Card card)
        {
            if (ModelState.IsValid)
            {
                await _cardService.UpdateAsync(card);
                return RedirectToAction(nameof(Index));
            }

            var persons = await _personService.GetAllAsync();
            ViewBag.Persons = new SelectList(persons, "Id", "FullName", card.PersonId);

            return View(card);
        }

    }
}

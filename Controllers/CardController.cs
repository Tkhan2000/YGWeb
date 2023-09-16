using Microsoft.AspNetCore.Mvc;
using YGWeb.Data;
using YGWeb.Models;
using PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace YGWeb.Controllers
{
    public class CardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CardController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string searchString, string cardType)
        {
            IEnumerable<Card> objCardList = _db.Cards;
            if (!String.IsNullOrEmpty(searchString))
            {
                objCardList = objCardList.Where(card => card.name.Contains(searchString) || card.description.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(cardType))
            {
                objCardList = objCardList.Where(card => card.type.Contains(cardType));
            }
            return View(objCardList);
        }

        public IActionResult SelectCardType() 
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Monster", Value = "1" });
            items.Add(new SelectListItem { Text = "Spell", Value = "2" });
            items.Add(new SelectListItem { Text = "Trap", Value = "3" });
            ViewBag.CardType = items;
            return View(); 
        }
    }
}

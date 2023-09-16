using Microsoft.AspNetCore.Mvc;
using YGWeb.Data;
using YGWeb.Models;

namespace YGWeb.Controllers
{
    public class CardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CardController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Card> objCardList = _db.Cards;
            return View(objCardList);
        }
    }
}

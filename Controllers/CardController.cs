using Microsoft.AspNetCore.Mvc;
using YGWeb.Data;
using YGWeb.Models;

namespace YGWeb.Controllers
{
    public class CardController : Controller
    {
        private readonly ApplicationDbContext _db;
        private int _pageSize;
        private int _lastPage;

        public CardController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string searchString, string cardType, int? page)
        {
            /*
             * Main Page for Card List. Keeps track of Search Keyword, page Number, and other filter keywords 
             * and modifies database accordingly
            */
            IEnumerable<Card> objCardList = _db.Cards;
            int pageNumber = page ?? 1;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                objCardList = objCardList.Where(card => card.name.ToLower().Contains(searchString.ToLower()) || card.description.ToLower().Contains(searchString.ToLower()));
            }
            if (!String.IsNullOrEmpty(cardType))
            {
                objCardList = objCardList.Where(card => card.type.Contains(cardType));
            }
            var chunks = objCardList.Chunk(50);
            _lastPage = chunks.Count();

            ViewBag.pageNumber = pageNumber;
            ViewBag.searchString = searchString;
            ViewBag.cardType = cardType;
            ViewBag.lastPage = _lastPage;

            if(_lastPage == 0)
            {
                return View();
            }

            var currentPage = chunks.ElementAt(pageNumber-1);
            return View(currentPage);
        }
    }
}

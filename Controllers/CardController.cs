using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YGWeb.Data;
using YGWeb.Models;
using static YGWeb.Models.Card;

namespace YGWeb.Controllers
{
    public class CardController : Controller
    {
        private readonly ApplicationDbContext _db;
        private int _lastPage;
        private string _currentCardList = "";

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
            /*
            IEnumerable<Card> cardList = _db.Cards.Take(40);
            var deck = new Deck();
            string cards = "";
            foreach(Card objCard in objCardList)
            {
                cards += objCard.id.ToString() + "/";
            }
            deck.CardList = cards;
            _db.Decks.Add(deck);
            _db.SaveChanges();
            */
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

            if (_lastPage == 0)
            {
                return View();
            }

            var currentPage = chunks.ElementAt(pageNumber - 1);
            return View(currentPage);
        }
        [Authorize]
        public IActionResult DeckBuilder(string searchString, int? page)
        {
            IEnumerable<Card> objCardList = _db.Cards;
            int pageNumber = page ?? 1;

            if (!String.IsNullOrEmpty(searchString))
            {
                objCardList = objCardList.Where(card => card.name.ToLower().Contains(searchString.ToLower()) || card.description.ToLower().Contains(searchString.ToLower()));
            }
            var chunks = objCardList.Chunk(80);
            _lastPage = chunks.Count();

            ViewBag.pageNumber = pageNumber;
            ViewBag.searchString = searchString;
            ViewBag.lastPage = _lastPage;

            if (_lastPage == 0)
            {
                return View();
            }

            var currentPage = chunks.ElementAt(pageNumber - 1);

            return View(currentPage);
        }

        [HttpPost]
        public JsonResult addCard(int id)
        {
            _currentCardList += id.ToString() + "|";
            TempData["currentCardList"] = _currentCardList;
            return new JsonResult(_currentCardList);
        }

        public IActionResult CardDetails(int id, bool cardList)
        {
            ViewBag.cardList = cardList;
            Card card = _db.Cards.FirstOrDefault(card => card.id == id);
            return View(card);
        }
    }
}

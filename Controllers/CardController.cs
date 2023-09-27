using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
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
            IEnumerable<Card> baseCardList = _db.Cards.Distinct().OrderBy(card => card.name);
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
                baseCardList = baseCardList.Where(card => card.name.ToLower().Contains(searchString.ToLower()) || card.description.ToLower().Contains(searchString.ToLower()));
            }
            if (!String.IsNullOrEmpty(cardType))
            {
                baseCardList = baseCardList.Where(card => card.type.Contains(cardType));
            }
            var chunks = baseCardList.Chunk(50);
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
            IEnumerable<Card> baseCardList = _db.Cards.Distinct().OrderBy(card => card.name);
            int diff = objCardList.Count() - baseCardList.Count();

            List<Card> cardList = _db.Cards.ToList();
            foreach(Card card in baseCardList)
            {
                cardList.Remove(card);
            }
            IEnumerable<Card>  tempCardList = cardList;
            int pageNumber = page ?? 1;

            if (!String.IsNullOrEmpty(searchString))
            {
                baseCardList = baseCardList.Where(card => card.name.ToLower().Contains(searchString.ToLower()) || card.description.ToLower().Contains(searchString.ToLower()));
            }

            
            var chunks = baseCardList.Chunk(80);
            _lastPage = chunks.Count();

            ViewBag.pageNumber = pageNumber;
            ViewBag.searchString = searchString;
            ViewBag.lastPage = _lastPage;

            if (_lastPage == 0)
            {
                return View();
            }

            var currentPage = chunks.ElementAt(pageNumber - 1);

            return View(Tuple.Create(currentPage, tempCardList));
        }

        public IActionResult addCard(int id, string s, int p, int c)
        {
            Card card = _db.Cards.FirstOrDefault(card => card.id == id);

            for (int i = 0; i < c; i++)
            {
                // Add back any extra cards that were removed
                _db.Cards.Add(card);
                _db.SaveChanges();
            }
            return RedirectToAction("DeckBuilder", new {searchString = s, page = p});
        }

        public IActionResult removeCard(int id, string s, int p, int c)
        {
            Card card = _db.Cards.FirstOrDefault(card => card.id == id);
            int count = _db.Cards.Count(card => card.id == id) - c;
            _db.Cards.Remove(card);
            _db.SaveChanges();
            for (int i = 0; i < count; i++)
            {
                // Add back any extra cards that were removed
                _db.Cards.Add(card);
                _db.SaveChanges();
            }
            return RedirectToAction("DeckBuilder", new { searchString = s, page = p });
        }
        /*
        [HttpPost]
        public JsonResult addCard(int id)
        {
            Card card = _db.Cards.FirstOrDefault(card => card.id == id);
            _db.Cards.Add(card);
            _db.SaveChanges();
            return new JsonResult("");
        }*/

        public IActionResult CardDetails(int id, string cardList, string searchString, int? page, int count)
        {
            int pageNumber = page ?? 1;
            ViewBag.cardList = cardList;
            ViewBag.pageNumber = pageNumber;
            ViewBag.searchString = searchString;
            ViewBag.count = count;
            Card card = _db.Cards.FirstOrDefault(card => card.id == id);
            return View(card);
        }
    }
}
